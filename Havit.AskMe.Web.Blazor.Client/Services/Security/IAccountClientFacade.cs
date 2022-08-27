using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Account;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security {
	public interface IAccountClientFacade {
		Task<ChangePasswordVM> ChangePasswordAsync(ChangePasswordIM inputModel);
	}
}