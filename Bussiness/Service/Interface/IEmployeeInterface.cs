using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View=Model.ViewModel;

namespace Bussiness.Service.Interface
{
    public interface IEmployeeInterface
    {
        Task AddEmployee(View.EmployeeModel emp);
        Task<View.EmployeeModel> GetById(long id);
        Task<List<View.EmployeeModel>> GetAllUsers();
        Task UpdateUser(View.EmployeeModel userView);
        Task<bool> DeleteUser(long id);
    }
}
