namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public class ContactViewModel : MasterPageViewModel {
        public ContactViewModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
            : base(env) {
        }

        public override string PageTitle => "Kontakt";
    }
}

