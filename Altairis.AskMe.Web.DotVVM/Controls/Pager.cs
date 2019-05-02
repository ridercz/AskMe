using DotVVM.Framework.Binding;
using DotVVM.Framework.Controls;

namespace Altairis.AskMe.Web.DotVVM.Controls {
    public class Pager : DotvvmMarkupControl {
        public string RouteName {
            get => (string)this.GetValue(RouteNameProperty);
            set => this.SetValue(RouteNameProperty, value);
        }
        public static readonly DotvvmProperty RouteNameProperty
            = DotvvmProperty.Register<string, Pager>(c => c.RouteName, null);

    }
}
