using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using PetitChoco.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace PetitChoco.ViewModels
{
    public class PackageViewModel : BindableBase
    {

        public ReactiveProperty<string> DirectoryName { get; }
        public ReactiveCollection<MetaDataViewModel> MetaData { get; }
        public ReactiveProperty<DirectoryInfo> DirectoryInfo { get; }

        public PackageViewModel()
        {
            DirectoryName = new ReactiveProperty<string>();
            MetaData = new ReactiveCollection<MetaDataViewModel>();
            DirectoryInfo = new ReactiveProperty<DirectoryInfo>();
        }

        public PackageViewModel(Package model)
        {
            DirectoryName = model.ObserveProperty(x => x.DirectoryName).ToReactiveProperty();
            MetaData = model.MetaData.Select(x => new MetaDataViewModel(x)).ToObservable().ToReactiveCollection();
            DirectoryInfo = model.ObserveProperty(x => x.DirectoryInfo).ToReactiveProperty();
        }
    }

}
