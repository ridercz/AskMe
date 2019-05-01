using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Web.DotVVM.Filters;
using Altairis.AskMe.Web.DotVVM.Presenters;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.AskMe.Web.DotVVM {
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator {
        public void Configure(DotvvmConfiguration config, string applicationPath) {
            config.RouteTable.Add("Questions", "questions/{pageNumber?}", "Views/Questions.dothtml", defaultValues: new { pageNumber = 1 });
            config.RouteTable.Add("HomeWithoutPaging", "", "Views/Index.dothtml", defaultValues: new { pageNumber = 1 });
            config.RouteTable.Add("Home", "{pageNumber:int}", "Views/Index.dothtml", defaultValues: new { pageNumber = 1 });
            config.RouteTable.Add("Login", "account/login", "Views/Account/Login.dothtml");
            config.RouteTable.Add("Logout", "account/logout", "Views/Account/Logout.dothtml");
            config.RouteTable.Add("ChangePassword", "admin/changePassword", "Views/Admin/ChangePassword.dothtml");
            config.RouteTable.Add("EditQuestion", "admin/{questionId:int}", "Views/Admin/Index.dothtml");
            config.RouteTable.Add("QuestionDetail", "question/{questionId:int}", "Views/Question.dothtml");
            config.RouteTable.Add("Error", "error/{errorCode:int}", "Views/Error.dothtml");
            config.RouteTable.Add("RssFeed", "feed.rss", typeof(RssPresenter));

            config.Resources.Register("Styles", new StylesheetResource(new FileResourceLocation("wwwroot/Styles/askme.css")));
            config.Markup.AddMarkupControl("my", "pager", "Controls/Pager.dotcontrol");
            config.Runtime.GlobalFilters.Add(new ErrorHandlingFilter());
        }

        public void ConfigureServices(IDotvvmServiceCollection options) {
            // TODO: Configure services
        }
    }
}
