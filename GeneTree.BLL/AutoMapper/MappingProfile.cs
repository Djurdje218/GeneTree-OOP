using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GeneTree.DAL.Entities;
using GeneTree.BLL.DTO;
using static System.Formats.Asn1.AsnWriter;

namespace GeneTree.BLL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>().ReverseMap();
            
        }
    }
}
