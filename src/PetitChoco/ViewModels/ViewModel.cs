using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NuGet;
using PetitChoco.Models;
using PetitChoco.Nuspec;
using Reactive.Bindings;
using Reactive;

namespace PetitChoco
{
    class ViewModel
    {

        public ReactiveProperty<string> PackagePath { get; }
        public ReactiveProperty<Package> Package { get; }

        public ReactiveCommand LoadPackageCommand { get; }

        public ViewModel()
        {
            PackagePath = new ReactiveProperty<string>();
            Package =  new ReactiveProperty<Package>();
            LoadPackageCommand = new ReactiveCommand();
            LoadPackageCommand.Subscribe(() =>
            {
                MessageBox.Show("");
                Package.Value = NuspecReader.Read(PackagePath.Value);
            } );
        }
    }
}
