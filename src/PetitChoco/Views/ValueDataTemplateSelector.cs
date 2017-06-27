using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Prism.Mvvm;
using Reactive.Bindings;

namespace PetitChoco.Views
{
    [ContentProperty(nameof(Triggers))]
    public class ValueDataTemplateSelector : DataTemplateSelector
    {
        public object Value { get; set; }
        public Collection<Template> Triggers { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return Triggers.FirstOrDefault(x => x.Value == item)?.DataTemplate;
        }
    }

    [ContentProperty(nameof(DataTemplate))]
    public class Template
    {


        public object Value { get; set; }
        public DataTemplate DataTemplate { get; set; }
    }
}
