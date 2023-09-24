using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View = Model.ViewModel;
using Bussiness.Service.Interface;
using Data.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Bussiness.Service.Employees
{
    public class EmployeeService : IEmployeeInterface
    {
        private readonly AppDBContext _dbContext;
        private readonly ILogger<EmployeeService> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
		private readonly IConfiguration _config;

		public EmployeeService(AppDBContext dbContext, ILogger<EmployeeService> logger, UserManager<IdentityUser> userManager, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }

        public async Task AddEmployee(View.EmployeeModel employee)
        {
            try
            {
                var res = await _userManager.FindByNameAsync(employee.Email);
                if (res == null)
                {
                    var user = new IdentityUser { UserName = employee.Email, Email = employee.Email,PhoneNumber=employee.ContactNo.ToString(), EmailConfirmed = true };
                    var result = await _userManager.CreateAsync(user, "P@ssword1");

                    if (!result.Succeeded)
                        throw new Exception(string.Join("|", result.Errors));

                    var dbUser = await _userManager.FindByNameAsync(employee.Email);
                    var dbUserDetails = _mapper.Map<View.EmployeeModel, Data.Entity.EmployeeDetail>(employee);
                    dbUserDetails.UserId = dbUser.Id;
                    dbUserDetails.CreatedDate = DateTime.Now;
                    dbUserDetails.ContactNumber=employee.ContactNo.Value;
                    dbUserDetails.Status=true;
                    dbUserDetails.DisplayName=employee.FirstName+" "+ employee.LastName;

                    employee.UserId = dbUser.Id;

                    var newUserDetail = await _dbContext.EmployeeDetails.AddAsync(dbUserDetails);
                   
                   
                    await _dbContext.SaveChangesAsync();
                   
                }
                else
                {
                    _logger.LogError("Employee already exists!!");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to add Employee");
                throw;
            }

        }
        public async Task<View.EmployeeModel> GetById(long id)
        {
            try
            {
                View.EmployeeModel User = new View.EmployeeModel();
                if (id != 0)
                {
                    var dbUserDetails = await _dbContext.EmployeeDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
                    _mapper.Map(dbUserDetails, User);
                    User.ContactNo = dbUserDetails.ContactNumber;
                    User.DisplayName=dbUserDetails.FirstName+" "+ dbUserDetails.LastName;

                }
                return User;
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to get User");
                throw;
            }
        }
        public async Task<List<View.EmployeeModel>> GetAllUsers()
        {
            List<View.EmployeeModel> userList = new();
            try
            {
                userList = await _dbContext.EmployeeDetails
                    .Select(y => new View.EmployeeModel()
                    {
                        Id = y.Id,
                        DisplayName = y.FirstName+" "+y.LastName,
                        ContactNo = y.ContactNumber,
                        Email = y.Email,
                        UserId=y.UserId

                    }).OrderByDescending(y => y.Id).ToListAsync();


            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Get All Employee");
            }
            return userList;
        }
        public async Task UpdateUser(View.EmployeeModel userView)
        {
            
           
            try
            {
                var identityUser = await _userManager.FindByIdAsync(userView.UserId);
                var dbUser = await _dbContext.AspNetUsers.FindAsync(userView.UserId);

                if (dbUser.Email != userView.Email)
                {
                    if (_dbContext.AspNetUsers.Any(i => i.Email == userView.Email))
                        throw new Exception("Email already exists");

                }
                var dbUserDetail = await _dbContext.EmployeeDetails
                                    .FirstOrDefaultAsync(u => u.Id == userView.Id);
                var isActive = dbUserDetail.Status;

                var userDataDetail = _mapper.Map(userView, dbUserDetail);
               
               

                _dbContext.EmployeeDetails.Update(userDataDetail);
               
              
                
                await _dbContext.SaveChangesAsync();

                var token = await _userManager.GenerateChangeEmailTokenAsync(identityUser, userView.Email);
                await _userManager.ChangeEmailAsync(identityUser, userView.Email, token);
                await _userManager.SetUserNameAsync(identityUser, userView.Email);

                
               
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Update Employee");
            }
            
        }
        public async Task<bool> DeleteUser(long id)
        {
            bool result = false;
            try
            {
                var dbUserDetails = await _dbContext.EmployeeDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
                if(dbUserDetails != null)
                {
                    
                    var userId = dbUserDetails.UserId;
                 
                    _dbContext.EmployeeDetails.Remove(dbUserDetails);
                    await _dbContext.SaveChangesAsync();
                    var dele = RemoveUserById(userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "Failed to Update Employee");
            }
            return result;
        }
        private async Task<bool> RemoveUserById(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    // handle the case where the user is not found
                    return false;
                }

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // handle the case where the user is successfully deleted
                    return true;
                }
                else
                {
                    // handle the case where the deletion fails
                    return false;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(exception: ex, message: "Failed to Delete Employee");
                return false;// return null to indicate that an error occurred
            }
        }
    }
}

