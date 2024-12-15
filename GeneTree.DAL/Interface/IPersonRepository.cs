using GeneTree.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneTree.DAL.Repositories
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person person);
        Task<Person> GetPersonByIdAsync (int id);
        Task<IEnumerable<Person>> GetAllPeopleAsync();
        Task SaveChangesAsync(IEnumerable<Person> people);

        Task SaveChangesAsync();

        Task DeleteAllPeopleAsync();
    }
}
