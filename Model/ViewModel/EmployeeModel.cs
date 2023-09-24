using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class EmployeeModel
    {
        public long? Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage ="First Name is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact Number is required")]
        public long? ContactNo { get; set; }
        public string Address { get; set; }

    }
}
