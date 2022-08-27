using System.Collections.Generic;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts;

namespace Havit.AskMe.Web.Blazor.Client.Services {
	public interface ICategoryClientFacade {
		Task<List<ListItemVM>> GetAll();
	}
}