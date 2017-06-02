using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace PetitChoco.Models
{
    class PackagesDirectoryModel : BindableBase
    {
        private string _packagesDirectioryPath;
        public string PackageDirectioryPath
        {   
            get { return _packagesDirectioryPath; }
            set
            {
                SetProperty(ref _packagesDirectioryPath, value);
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(Directories));
            }
        }

        private string[] _directories;
        public string[] Directories
        {
            get
            {
                if (PackageDirectioryPath == null || !Directory.Exists(PackageDirectioryPath)) return null;
                return Directory.GetDirectories(PackageDirectioryPath);
            }
        }
    }
}
