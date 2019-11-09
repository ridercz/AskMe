using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Havit.AskMe.Web.Blazor.Server.Controllers {
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class AccountController : ControllerBase {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly AppConfiguration appConfiguration;

		public AccountController(
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			IOptions<AppConfiguration> appConfiguration) {
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.appConfiguration = appConfiguration.Value;
		}

		[AllowAnonymous]
		[HttpPost("api/accounts/login")]
		public async Task<IActionResult> Login(LoginIM model) {
			var result = await signInManager.PasswordSignInAsync(
				model.UserName,
				model.Password,
				isPersistent: false,
				lockoutOnFailure: false);

			if (!result.Succeeded) {
				return Ok(new LoginVM { Successful = false, Error = "Přihlášení se nezdařilo." }); // BadRequest()?
			}

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, model.UserName)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.JwtSecurityKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.Now.AddDays(Convert.ToInt32(appConfiguration.JwtExpirationInDays));

			var token = new JwtSecurityToken(
				appConfiguration.JwtIssuer,
				appConfiguration.JwtAudience,
				claims,
				expires: expiration,
				signingCredentials: credentials
			);

			return Ok(new LoginVM { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
		}


		[HttpPost("api/accounts/changepassword")]
		public async Task<IActionResult> ChangePassword(ChangePasswordIM inputModel) {
			// Get current user
			var user = await userManager.FindByNameAsync(this.User.Identity.Name);

			// Try to change password
			var result = await userManager.ChangePasswordAsync(
				user,
				inputModel.OldPassword,
				inputModel.NewPassword);

			return Ok(new ChangePasswordVM() {
				Succeeded = result.Succeeded,
				Errors = result.Errors.Select(e => e.Description).ToArray()
			});
		}
	}
}
