/*using GeneTree.DAL.Data;
using GeneTree.DAL.Entities;
using GeneTree.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneTree.DAL.Repository
{
    public class DatabasePersonRepository : IPersonRepository
    {
        private readonly TreeContext _context;

        public DatabasePersonRepository(TreeContext context)
        {
            _context = context;
        }

        public async Task AddPersonAsync(Person person)
        {
           await _context.People.AddAsync(person);
        }

        public Task DeleteAllPeopleAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync()
        {
            return await _context.People.Include(p => p.Parents).Include(p => p.Children)
                .ToListAsync();
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
           return await _context.People.Include(p => p.Parents)
                .Include(p=> p.Children)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
             await _context.SaveChangesAsync();
        }
    }
}
*/