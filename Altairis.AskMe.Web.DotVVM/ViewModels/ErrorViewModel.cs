using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Hosting;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public class ErrorViewModel : MasterPageViewModel {
        public ErrorViewModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
            : base(env) {
        }

        public string Message {
            get {
                var errorCode = Convert.ToInt32(this.Context.Parameters["errorCode"]);

                switch (errorCode) {
                    case 404:
                        return "Požadovaná stránka neexistuje!";
                    case 500:
                        return "Vnitøní chyba serveru";
                    default:
                        return "Pøi zpracování vašeho požadavku došlo k neèekané chybì.";
                }
            }
        }

        public override string PageTitle {
            get {
                var errorCode = Convert.ToInt32(this.Context.Parameters["errorCode"]);
                return $"HTTP chyba {errorCode}";
            }
        }
    }
}

