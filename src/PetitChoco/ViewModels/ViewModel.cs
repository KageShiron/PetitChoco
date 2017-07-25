using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public ReactiveCommand OpenPackageCommand { get; }
        public ReactiveCommand EditPackageCommand { get; }
        public ReactiveProperty<ToolViewModel> ToolViewModel { get; }
        public ReactiveProperty<string> PackageListPath { get; set; }
        public ReactiveProperty<string[]> PackageList { get; }
        public ReactiveProperty<ReactiveCollection<KeyValuePair<string, string>>> PackageFile { get; }

        public ReactiveProperty<ObservableCollection<MetaData>> PackageMetaData { get; }

        public ReactiveProperty<IEnumerable<FileTreeItem>> PackageRootItem { get; }
        public ReactiveCommand SaveNuspecFileCommand { get; }
        public ReactiveCommand<string> OpenInBrowserCommand { get; }
        public ReactiveCommand<Uri> WebBrowserNavigateCommand { get; set; }

        public ReactiveProperty<Uri> ChocolateyOrgUrl { get; }

        public ViewModel()
        {
            PackagePath = new ReactiveProperty<string>(@"c:\src\choco\pandoc-crossref");
            Package = new ReactiveProperty<PackageViewModel>(new PackageViewModel());
            LoadPackageCommand = new ReactiveCommand();
            LoadPackageCommand.Subscribe(() => Package.Value = new PackageViewModel(new Package(PackagePath.Value)));
            OpenPackageCommand = new ReactiveCommand();
            OpenPackageCommand.Subscribe(() => Process.Start("explorer",PackagePath.Value));
            EditPackageCommand = new ReactiveCommand();
            EditPackageCommand.Subscribe(() => Process.Start("code", PackagePath.Value));

            SaveNuspecFileCommand = new ReactiveCommand();
            SaveNuspecFileCommand.Subscribe(() =>
            {
                XNamespace ns = XNamespace.Get("http://schemas.microsoft.com/packaging/2015/06/nuspec.xsd");

                XElement dependencies = Package.Value?.Dependencies.Count == 0 ? null
                : new XElement(ns + "dependencies", Package.Value.Dependencies
                .Select(d => new XElement(ns + "dependency"
                      , new XAttribute(ns + "id", d.Id)
                      , new XAttribute(ns + "version", d.VersionRange.ToString()))));
                XElement metadata = new XElement(ns + "metadata", Package.Value.MetaData.Where(m => !string.IsNullOrWhiteSpace(m.Value))
                    .Select(m => new XElement(ns + m.Name, m.Value)), dependencies);

                XElement files = new XElement(ns + "files",
                    Package.Value.Files.Select(f => new XElement( ns+ "file"
                    ,new XAttribute("src", f.Source)
                    ,new XAttribute("target", f.Target))));

                XElement package = new XElement(ns + "package", metadata, files);


                XDocument doc = new XDocument(package);
                using (var w = new FileStream(Package.Value.NuspecFileName.Value, FileMode.Create, FileAccess.Write, FileShare.None))
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
            OpenInBrowserCommand = new ReactiveCommand<string>();
            OpenInBrowserCommand.Subscribe(s =>
            {
                System.Diagnostics.Process.Start(s);
            });
            Package.Subscribe(pack => WebBrowserNavigateCommand?.Execute(new Uri(
                "https://chocolatey.org/packages/" + pack.MetaData.Where(x => x.Name == "id").FirstOrDefault()?.Value)));
        }
    }
}
