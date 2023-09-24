using Bussiness.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModel.APIResponse;
using View = Model.ViewModel;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeInterface _employee;
        private readonly ILogger<EmployeeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

         public EmployeeController(IEmployeeInterface emp,ILogger<EmployeeController> logger, UserManager<IdentityUser> userManager)
        {
            _employee = emp;
            _logger = logger;
            _userManager = userManager;
        }
        [HttpPost]
      
        public async Task<IActionResult> AddEmployee( View.EmployeeModel userView)
        {
            try
            {
                await _employee.AddEmployee(userView);
                return Created("", userView);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Add Employee");
                return BadRequest(new  ErrorResponse { Error = ex.Message });
            }
        }
        [HttpGet]
       
        public async Task<IActionResult> GetUserById(long id)
        {

            try
            {
               
                var user = await _employee.GetById(id);
                return Ok(user);

            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Get Employee");
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
        }
        [HttpPatch]

        public async Task<IActionResult> UpdateUser( View.EmployeeModel userView)
        {
            try
            {
                await _employee.UpdateUser(userView);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Update Employee");
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }

        }
        [HttpGet]
        
        public async Task<IActionResult> GetUserList()
        {

            try
            {

                var user = await _employee.GetAllUsers();
                return Ok(user);

            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Get Employee");
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
        }
        [HttpDelete]

        public async Task<IActionResult> DeleteUser(long Id)
        {
            try
            {
                await _employee.DeleteUser(Id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, message: "Failed to Remove Employee");
                return BadRequest(new ErrorResponse() { Error = e.Message });
            }
        }
    }
}
