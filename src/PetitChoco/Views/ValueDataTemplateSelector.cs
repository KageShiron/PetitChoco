using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using PetitChoco.Models;
using PetitChoco.ViewModels;
using Prism.Mvvm;
using Reactive.Bindings;

namespace PetitChoco.Views
{
    public class ValueDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var vm = item as MetaData;
            if (vm == null) return null;
            var element = container as FrameworkElement;

            string template = null;
            switch (vm.EditMode)
            {
                case EditMode.MultiLineString:
                case EditMode.SingleLineString:
                case EditMode.Markdown:
                case EditMode.SpaceSeparatedStrings:
                case EditMode.CommaSeparatedStrings:
                case EditMode.Url:
                case EditMode.PackageId:
                case EditMode.Version:
                    template = "TextBoxTemplate";
                    break;
                case EditMode.Boolean:
                    template = "CheckBoxTemplate";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return element.FindResource(template) as DataTemplate;
        }
    }
    
}
