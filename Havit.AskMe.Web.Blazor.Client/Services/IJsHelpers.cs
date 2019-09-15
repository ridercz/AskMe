using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services
{
	public interface IJsHelpers
	{
		ValueTask<object> SetElementAttributeAsync(string elementId, string attributeName, string attributeValue);
		ValueTask<object> SetElementAttributeAsync(ElementReference elementReference, string attributeName, string attributeValue);
		ValueTask<object> SetPageTitleAsync(string title);
	}
}