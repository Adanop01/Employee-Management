using API.Helper;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.ViewModel.APIResponse;
using Model.ViewModel.Authentication;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using View = Model.ViewModel;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class AccountController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly APISettings _apiSettings;
		private readonly AppDBContext _dbContext;
		private readonly ILogger<AccountController> _logger;

		public AccountController(SignInManager<IdentityUser> signInManager,
		  UserManager<IdentityUser> userManager,
		  AppDBContext dbContext,
		  ILogger<AccountController> logger,
		  IOptions<APISettings> options)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_apiSettings = options.Value;
			_dbContext = dbContext;
			_logger = logger;
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> SignIn([FromBody] Authentication authentication)
		{
			try
			{

				var result = await _signInManager.PasswordSignInAsync(authentication.UserName, authentication.Password, false, false);
				if (result.Succeeded)
				{
					var user = await _userManager.FindByNameAsync(authentication.UserName);
					var userDetail = await _dbContext.EmployeeDetails.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

					if (user == null)
					{
						return Unauthorized(new AuthenticationResponse
						{
							IsAuthSuccessful = false,
							ErrorMessage = "Invalid Authentication."
						});
					}
					if (userDetail.Status == false)
					{
						return Unauthorized(new AuthenticationResponse
						{
							IsAuthSuccessful = false,
							ErrorMessage = "Account Deactivated."
						});
					}
					var signinCredentials = GetSigningCredentials();
					var claims = await GetClaimsAsync(user);

					var tokenOptions = new JwtSecurityToken(
						issuer: _apiSettings.ValidIssuer,
						audience: _apiSettings.ValidAudience,
						 claims: claims,
						expires: DateTime.Now.AddDays(30),
						signingCredentials: signinCredentials
						);

					var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

					return Ok(new AuthenticationResponse
					{
						IsAuthSuccessful = true,
						Token = token,
						UserName = userDetail.DisplayName != null ? userDetail.DisplayName : user.Email,
						User = new User
						{
							Id = user.Id,
							Email = user.Email,
							ContactNo = user.PhoneNumber,
						},
						
					});

				}
				else
				{
					return Unauthorized(new AuthenticationResponse
					{
						IsAuthSuccessful = false,
						ErrorMessage = "InvalidAuthentication"
					});
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(exception: ex, message: "SignIn Failed");
				return BadRequest(new ErrorResponse { Error = ex.Message });
			}
		}
		private SigningCredentials GetSigningCredentials()
		{
			var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}
		private async Task<List<Claim>> GetClaimsAsync(IdentityUser user)
		{

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name,user.Email),
				new Claim(ClaimTypes.Email,user.Email),
				new Claim("Id",user.Id),


			};
			var role = "Admin";
			claims.Add(new Claim(ClaimTypes.Role, role));

			return claims;
		}
	}
}
