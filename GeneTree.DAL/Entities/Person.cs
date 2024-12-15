using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;

namespace GeneTree.DAL.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        public List<int> Children { get; set; } = new List<int>();  // List of child IDs
        public List<int> Parents { get; set; } = new List<int>();    // List of parent IDs

        public int spouseId { get; set; }


    }
}
