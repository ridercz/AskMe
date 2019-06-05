using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Client.Services
{
	public interface IJsHelpers
	{
		Task SetElementAttributeAsync(string elementId, string attributeName, string attributeValue);
		Task SetElementAttributeAsync(Microsoft.AspNetCore.Components.ElementRef elementRef, string attributeName, string attributeValue);
		Task SetPageTitleAsync(string title);
	}
}