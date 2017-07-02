using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
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
        private PackagesDirectoryModel PackagesDirectoryModel { get; } = new PackagesDirectoryModel();

        public ReactiveProperty<string> PackagePath { get; }
        public ReactiveProperty<PackageViewModel> Package { get; }

        public ReactiveCommand LoadPackageCommand { get; }
        public ReactiveProperty<ToolViewModel> ToolViewModel { get; }
        public ReactiveProperty<string> PackageListPath { get; set; }
        public ReactiveProperty<string[]> PackageList { get; }

        public ReactiveProperty<ReactiveCollection<MetaDataViewModel>> PackageMetaData { get; }

        public ReactiveProperty<IEnumerable<FileTreeItem>> PackageRootItem { get; }
        public ReactiveCommand SaveNuspecFileCommand { get; }

        public ViewModel()
        {
            PackagePath = new ReactiveProperty<string>(@"c:\src\choco\pandoc-crossref");
            Package = new ReactiveProperty<PackageViewModel>(new PackageViewModel());
            LoadPackageCommand = new ReactiveCommand();
            LoadPackageCommand.Subscribe(() => Package.Value = new PackageViewModel(new Package(PackagePath.Value)));

            SaveNuspecFileCommand = new ReactiveCommand();
            SaveNuspecFileCommand.Subscribe(() =>
            {
                XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/packaging/2015/06/nuspec.xsd");


                XElement metadata = new XElement(ns + "metadata", Package.Value.MetaData.Where(m => !string.IsNullOrWhiteSpace(m.Value.Value)).Select(m => new XElement(ns + m.Name.Value, m.Value.Value)));

                XElement package = new XElement(ns + "package",metadata);


                XDocument doc = new XDocument(package);
                using (var w = new FileStream("c:/temp/test.xml", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    doc.Save(w);
                }
            });

            PackageListPath = PackagesDirectoryModel.ToReactivePropertyAsSynchronized(
                m => m.PackageDirectioryPath,
                x => x,
                s => s,
                ReactivePropertyMode.DistinctUntilChanged | ReactivePropertyMode.RaiseLatestValueOnSubscribe
                , true);


            PackageList = PackagesDirectoryModel.ObserveProperty(m => m.Directories).ToReactiveProperty();
            PackageRootItem = Package.Select(x => x == null
                ? null
                : FileTreeItem.GetChildren(x.DirectoryInfo.Value)).ToReactiveProperty();

            ToolViewModel = new ReactiveProperty<ToolViewModel>(new ToolViewModel());
            PackageMetaData = Package.Select(x => x.MetaData).ToReactiveProperty();
        }
    }
}
