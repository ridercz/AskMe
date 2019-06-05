using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared
{
    public class ListItemVM
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public ListItemVM()
		{
			// NOOP
		}

		public ListItemVM(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public override bool Equals(object obj)
		{
			return (obj is ListItemVM vM) && (Id == vM.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
