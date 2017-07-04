using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using PetitChoco.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace PetitChoco.ViewModels
{
    public class MetaDataViewModel : BindableBase
    {
        public ReactiveProperty<bool> IsExist { get; }
        public ReactiveProperty<string> Name { get; }
        public ReactiveProperty<string> Value { get; }
        public ReactiveProperty<string> Description { get; }
        public ReactiveProperty<bool> IsKwnown { get; }
        public ReactiveProperty<EditMode> EditMode { get; }


        private MetaData _model;
        public MetaDataViewModel(MetaData model)
        {
            _model = model;
            IsExist = _model.ToReactivePropertyAsSynchronized(x => x.IsExist);
            Name = _model.ToReactivePropertyAsSynchronized(x => x.Name);
            Value = _model.ToReactivePropertyAsSynchronized(x => x.Value);
            Description = _model.ObserveProperty(x => x.Description).ToReactiveProperty();
            IsKwnown= _model.ObserveProperty(x => x.IsKwnown).ToReactiveProperty();
            EditMode = _model.ObserveProperty(x => x.EditMode).ToReactiveProperty();
        }
    }
}
