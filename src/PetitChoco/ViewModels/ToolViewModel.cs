using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetitChoco.Models;
using Reactive.Bindings;

namespace PetitChoco.ViewModels
{
    class ToolViewModel
    {
        public ToolViewModel()
        {
            Tools = new List<ToolModel>()
            {
                new ChocolateyToolModel("chocolatey","chocolatey","Chocolateyのコマンド","",true),
                new ChocolateyToolModel("Auto Updater","au","パッケージの自動更新を補助します","",true),
                new ChocolateyToolModel("Git","git","バージョン管理および一部の依存関係の解決に必要です。","",true),
                new ChocolateyToolModel("Vagrant", "vagrant", "パッケージの自動テストに必要です。","",false),
            };
        }

        public List<ToolModel> Tools { get; }
    }
}
