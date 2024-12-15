using GeneTree.DAL.Entities;
using GeneTree.BLL.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneTree.BLL.DTO;

namespace GeneTree.BLL.Interface
{
    public interface IGeneService
    {
        Task<int> AddPersonAsync(PersonDto person);
        Task AddParentChildRelationshipAsync(int parentId, int childId);
        Task<IEnumerable<PersonDto>> GetImmediateRelativesAsync(int personId);
        Task<string> GenerateTreeAsync();
        Task ClearTreeAsync();
        Task<int> CalculateAncestorAge(int ancestorId, int intId);

        Task AddSpouseRelationshipAsync(int spouse1Id, int spouse2Id);

    }
}
