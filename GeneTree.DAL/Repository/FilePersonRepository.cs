using GeneTree.DAL.Entities;
using GeneTree.DAL.Repositories;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneTree.DAL.Repository
{
    public class FilePersonRepository : IPersonRepository
    {
        private readonly string _filePath;

        public FilePersonRepository(string filePath)
        {
            _filePath = filePath;
        }
        public async Task AddPersonAsync(Person person)
        {
            var people = await GetAllPeopleAsync();
            var peopleList = people.ToList();

            // Generate new ID
            person.Id = peopleList.Any() ? peopleList.Max(p => p.Id) + 1 : 1;

            peopleList.Add(person);
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(peopleList));
        }

        public async Task DeleteAllPeopleAsync()
        {
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(new List<Person>()));
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Person>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
            var people = await GetAllPeopleAsync();
            return people.FirstOrDefault(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            var people = await GetAllPeopleAsync();
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true }));
        }

        public async Task SaveChangesAsync(IEnumerable<Person> people)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true // Pretty print the JSON 
            };

            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(people, jsonOptions));
        }



    }
}
