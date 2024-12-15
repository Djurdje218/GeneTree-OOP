using GeneTree.BLL.Interface;
using GeneTree.DAL.Repositories;
using GeneTree.BLL.DTO;
using AutoMapper;
using GeneTree.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneTree.BLL.Service
{
    public class GeneService : IGeneService
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public GeneService(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddParentChildRelationshipAsync(int parentId, int childId)
        {
            try
            {
                var people = await _repository.GetAllPeopleAsync();

                var parent = people.FirstOrDefault(p => p.Id == parentId);
                var child = people.FirstOrDefault(p => p.Id == childId);

                if (parent == null)
                {
                    throw new ArgumentException($"Parent with ID {parentId} not found.");
                }

                if (child == null)
                {
                    throw new ArgumentException($"Child with ID {childId} not found.");
                }

                Console.WriteLine($"Before adding: Parent {parentId} Children: {string.Join(", ", parent.Children)}");
                Console.WriteLine($"Before adding: Child {childId} Parents: {string.Join(", ", child.Parents)}");

                if (parent.Children.Contains(childId))
                {
                    throw new InvalidOperationException("This relationship already exists.");
                }

                if (child.Parents.Contains(parentId))
                {
                    throw new InvalidOperationException("This relationship already exists.");
                }

      
                parent.Children.Add(childId);  
                child.Parents.Add(parentId);   

                await _repository.SaveChangesAsync(people);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while adding relationship: {ex.Message}", ex);
            }
        }


        public async Task AddSpouseRelationshipAsync(int spouse1Id, int spouse2Id)
        {
            try
            {
                var people = await _repository.GetAllPeopleAsync();

                var spouse1 = people.FirstOrDefault(p => p.Id == spouse1Id);
                var spouse2 = people.FirstOrDefault(p => p.Id == spouse2Id);

                if (spouse1 == null)
                {
                    throw new ArgumentException($"Spouse 1 with ID {spouse1Id} not found.");
                }

                if (spouse2 == null)
                {
                    throw new ArgumentException($"Spouse 2 with ID {spouse2Id} not found.");
                }

                if (spouse1Id == spouse2Id)
                {
                    throw new InvalidOperationException("A person cannot be their own spouse.");
                }

                if (spouse1.spouseId != 0 || spouse2.spouseId != 0)
                {
                    throw new InvalidOperationException("One or both individuals are already married.");
                }

                spouse1.spouseId = spouse2Id;
                spouse2.spouseId = spouse1Id;

                await _repository.SaveChangesAsync(people);

                Console.WriteLine($"Spouse relationship established: {spouse1.FullName} ↔ {spouse2.FullName}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while adding spouse relationship: {ex.Message}", ex);
            }
        }



        public async Task<int> AddPersonAsync(PersonDto personDto)
        {
            var person = _mapper.Map<Person>(personDto);
            await _repository.AddPersonAsync(person);
            await _repository.SaveChangesAsync();

            return person.Id;
        }

        public async Task<int> CalculateAncestorAge(int ancestorId, int descendantId)
        {
            var ancestor = await _repository.GetPersonByIdAsync(ancestorId);
            var descendant = await _repository.GetPersonByIdAsync(descendantId);

            if (ancestor == null || descendant == null)
            {
                throw new ArgumentException("Ancestor or descendant not found.");
            }

            if (!await IsAncestorAsync(ancestor, descendant))
            {
                throw new InvalidOperationException("The specified ancestor is not an ancestor of the descendant.");
            }

            return (descendant.DateOfBirth - ancestor.DateOfBirth).Days / 365;
        }

        private async Task<bool> IsAncestorAsync(Person ancestor, Person descendant)
        {
            // Breadth first search to check if ancestor is connected to descendant.
            var queue = new Queue<Person>();

            
            foreach (var parentId in descendant.Parents)
            {
                var parent = await _repository.GetPersonByIdAsync(parentId);
                if (parent != null)
                {
                    queue.Enqueue(parent); 
                }
            }

            while (queue.Any())
            {
                var current = queue.Dequeue();
                if (current.Id == ancestor.Id)
                {
                    return true;
                }

                foreach (var parentId in current.Parents)
                {
                    var parent = await _repository.GetPersonByIdAsync(parentId);
                    if (parent != null)
                    {
                        queue.Enqueue(parent);  
                    }
                }
            }
            return false;
        }



        public async Task ClearTreeAsync()
        {
              await _repository.DeleteAllPeopleAsync();
        }


        public async Task<string> GenerateTreeAsync()
        {
            var people = await _repository.GetAllPeopleAsync();
            if (!people.Any())
            {
                return "No genealogy data available.";
            }

            var treeBuilder = new StringBuilder();
            var peopleById = people.ToDictionary(p => p.Id);
            var processed = new HashSet<int>(); 

            // Recursive method to build the tree
            void BuildTree(Person person, string indent, bool isLast)
            {
                if (processed.Contains(person.Id))
                    return; // avoid processing  same person twice

                processed.Add(person.Id);

                var spouse = people.FirstOrDefault(p => p.Id == person.spouseId);

              
                var connector = isLast ? "└── " : "├── ";
                if (spouse != null && !processed.Contains(spouse.Id))
                {
                    treeBuilder.AppendLine($"{indent}{connector}{person.FullName} (DOB: {person.DateOfBirth:yyyy-MM-dd}, Gender: {person.Gender})  {spouse.FullName} (DOB: {spouse.DateOfBirth:yyyy-MM-dd}, Gender: {spouse.Gender})");
                    processed.Add(spouse.Id); 
                }
                else
                {
                    treeBuilder.AppendLine($"{indent}{connector}{person.FullName} (DOB: {person.DateOfBirth:yyyy-MM-dd}, Gender: {person.Gender})");
                }

       
                var childIndent = isLast ? "    " : "│   ";


                var children = people.Where(p => p.Parents.Contains(person.Id)).ToList();
                for (int i = 0; i < children.Count; i++)
                {
                    BuildTree(children[i], indent + childIndent, i == children.Count - 1);
                }
            }

            var roots = people.Where(p => !p.Parents.Any()).ToList();
            for (int i = 0; i < roots.Count; i++)
            {
                BuildTree(roots[i], "", i == roots.Count - 1);
            }

            return treeBuilder.ToString();
        }


        public async Task<IEnumerable<PersonDto>> GetImmediateRelativesAsync(int personId)
        {
            var person = await _repository.GetPersonByIdAsync(personId);

            if (person == null)
            {
                throw new ArgumentException("Person not found.");
            }

            var relatives = new List<Person>();

            foreach (var parentId in person.Parents)
            {
                var parent = await _repository.GetPersonByIdAsync(parentId);
                if (parent != null)
                {
                    relatives.Add(parent);  
                }
            }

            foreach (var childId in person.Children)
            {
                var child = await _repository.GetPersonByIdAsync(childId);
                if (child != null)
                {
                    relatives.Add(child);  
                }
            }

            return _mapper.Map<IEnumerable<PersonDto>>(relatives);
        }

    }
}
