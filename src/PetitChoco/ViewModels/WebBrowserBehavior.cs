using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using Prism.Commands;
using Reactive.Bindings;

namespace PetitChoco.ViewModels
{
    class WebBrowserBehavior : Behavior<WebBrowser>
    {
        //http://d.hatena.ne.jp/trapemiya/20110128
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DataContextChanged += new DependencyPropertyChangedEventHandler(AssociatedObject_DataContextChanged);
            var axIWebBrowser2 = typeof(WebBrowser).GetProperty("AxIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            var comObj = axIWebBrowser2.GetValue(AssociatedObject, null);
            comObj.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, comObj, new object[] { true });
            SetCommand();
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetCommand();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DataContextChanged -= new DependencyPropertyChangedEventHandler(AssociatedObject_DataContextChanged);
        }

        public ReactiveCommand<Uri> ViewCommand { get; private set; }

        void SetCommand()
        {
            ViewCommand = new ReactiveCommand<Uri>();
            ViewCommand.Subscribe(ViewCommandHandler);

            if (AssociatedObject.DataContext != null)
                ((ViewModel)AssociatedObject.DataContext).WebBrowserNavigateCommand = ViewCommand;
        }

        private void ViewCommandHandler(Uri obj)
        {
            ((WebBrowser)AssociatedObject).Navigate(obj);
        }
    }
}
