using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.AutoMapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile() 
        {
            CreateMap<Data.Entity.EmployeeDetail, Model.ViewModel.EmployeeModel>().ReverseMap();
        }
    }
}
