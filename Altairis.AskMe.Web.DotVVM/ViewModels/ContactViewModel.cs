using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Hosting;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public class ContactViewModel : MasterPageViewModel {
        public ContactViewModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
            : base(env) {
        }

        public override string PageTitle => "Kontakt";
    }
}

