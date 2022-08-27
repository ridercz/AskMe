using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts {
	public abstract class QueryFilterBase : IValidatableObject {
		public const int MaxPageSize = 500;
		public const int DefaultPageSize = 10;
		public const int DefaultPageIndex = 0;

		public string OrderBy { get; set; } = string.Empty;

		public bool Descending { get; set; }

		public int PageIndex { get; set; } = DefaultPageIndex;
		public int PageSize { get; set; } = DefaultPageSize;

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
			if (this.PageIndex < 0) {
				this.PageIndex = DefaultPageIndex;
			}

			if (this.PageSize < 1) {
				this.PageSize = DefaultPageSize;
			}

			if (this.PageSize > MaxPageSize) {
				this.PageSize = MaxPageSize;
			}

			yield break;
		}

	}
}
