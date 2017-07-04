using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using NuGet;
using PetitChoco.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using PackageDependency = NuGet.Packaging.Core.PackageDependency;

namespace PetitChoco.ViewModels
{
    public class PackageViewModel : BindableBase
    {

        public ReactiveProperty<string> DirectoryName { get; }
        public ObservableCollection<MetaData> MetaData { get; }
        public ReactiveProperty<DirectoryInfo> DirectoryInfo { get; }
        public ObservableCollection<PackageFile> Files { get; }
        public ObservableCollection<PackageDependency> Dependencies { get; }
        public ReactiveProperty<string> NuspecFileName { get; set; }

        public PackageViewModel()
        {
            DirectoryName = new ReactiveProperty<string>();
            MetaData = new ObservableCollection<MetaData>();
            DirectoryInfo = new ReactiveProperty<DirectoryInfo>();
            Files = new ObservableCollection<PackageFile>();
            Dependencies = new ObservableCollection<PackageDependency>();
        }

        public PackageViewModel(Package model)
        {
            DirectoryName = model.ObserveProperty(x => x.DirectoryName).ToReactiveProperty();
            MetaData = model
                .MetaData; // model.MetaData.Select(x => new MetaDataViewModel(x)).ToObservable().ToReactiveCollection();
            DirectoryInfo = model.ObserveProperty(x => x.DirectoryInfo).ToReactiveProperty();
            Files = model.Files; //model.MetaData;new ObservableCollection<ContentFilesEntry>(model.MetaData.Select(x => new MetaDataViewModel(x)));
            Dependencies = model.Dependencies;
            NuspecFileName = model.ToReactivePropertyAsSynchronized(x => x.NuspecFileName);

            //new ObservableCollection<ContentFilesEntry>()
        }
    }
}
