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
using Prism.Mvvm;
using Reactive.Bindings;

namespace PetitChoco.Views
{
    [ContentProperty(nameof(Triggers))]
    public class ValueDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var value = VisualTreeHelper.GetParent(container)
                .GetValue(ValueDataTemplateSelectorParamaters.ValueProperty); // HERE
            var triggers = VisualTreeHelper.GetParent(container)
                .GetValue(ValueDataTemplateSelectorParamaters.ValueProperty); // HERE
            return Triggers.FirstOrDefault(x => x.Value == item)?.DataTemplate;
        }
    }

    public static class ValueDataTemplateSelectorParamaters
    {


        public static object GetValue(DependencyObject obj)
        {
            return (object)obj.GetValue(ValueProperty);
        }

        public static void SetValue(DependencyObject obj, object value)
        {
            obj.SetValue(ValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(object), typeof(ValueDataTemplateSelectorParamaters), new PropertyMetadata(0));



        public static Collection<Template> GetTriggers(DependencyObject obj)
        {
            return (Collection<Template>)obj.GetValue(TriggersProperty);
        }

        public static void SetTriggers(DependencyObject obj, Collection<Template> value)
        {
            obj.SetValue(TriggersProperty, value);
        }

        // Using a DependencyProperty as the backing store for Triggers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached("Triggers", typeof(Collection<Template>), typeof(ValueDataTemplateSelectorParamaters), new PropertyMetadata(new Collection<Template>()));



    }

    [ContentProperty(nameof(DataTemplate))]
    public class Template
    {


        public object Value { get; set; }
        public DataTemplate DataTemplate { get; set; }
    }
}
