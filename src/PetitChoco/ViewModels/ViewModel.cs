using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NuGet;
using PetitChoco.Models;
using PetitChoco.Nuspec;
using PetitChoco.ViewModels;
using Reactive.Bindings;
using Reactive;
using Reactive.Bindings.Extensions;

namespace PetitChoco
{
    class ViewModel
    {
        private PackagesDirectoryModel PackagesDirectoryModel { get;  } = new PackagesDirectoryModel();

        public ReactiveProperty<string> PackagePath { get; }
        public ReactiveProperty<Package> Package { get; }

        public ReactiveCommand LoadPackageCommand { get; }
        public ReactiveProperty<ToolViewModel> ToolViewModel { get; }
        public ReactiveProperty<string> PackageListPath { get; set; }
        public ReactiveProperty<string[]> PackageList { get; }

        public ReactiveProperty<IEnumerable<FileTreeItem>> PackageRootItem { get; }

        public ViewModel()
        {
            PackagePath = new ReactiveProperty<string>(@"c:\src\choco\pandoc-crossref");
            Package = new ReactiveProperty<Package>();
            LoadPackageCommand = new ReactiveCommand();
            LoadPackageCommand.Subscribe(() => Package.Value = new Package(PackagePath.Value));

            PackageListPath = PackagesDirectoryModel.ToReactivePropertyAsSynchronized(
                m => m.PackageDirectioryPath,
                x => x,
                s => s,
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe
                , true);


            PackageList = PackagesDirectoryModel.ObserveProperty(m => m.Directories).ToReactiveProperty();
            PackageRootItem = Package.Select(x => x == null
                ? null : FileTreeItem.GetChildren(x.DirectoryInfo)).ToReactiveProperty();

            ToolViewModel = new ReactiveProperty<ToolViewModel>(new ToolViewModel());
        }
    }
}
