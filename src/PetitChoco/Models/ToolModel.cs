using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetitChoco.Models
{
    public abstract class ToolModel
    {
        /// <summary>
        /// ツール名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// ツールのパス
        /// </summary>
        public virtual string Path { get; set; }

        public virtual string Description { get; set; }

        /// <summary>
        /// ツールがインストールされているか
        /// </summary>
        public abstract bool IsInstalled { get; }
    
        /// <summary>
        /// インストールを試みます
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> TryInstall();
    }

    public class ChocolateyToolModel : ToolModel
    {
        public ChocolateyToolModel(string name,string path, string description, string packageId, bool isInstalled)
        {
            Name = name;
            Path = path;
            Description = description;
            IsInstalled = isInstalled;
            PackageId = packageId;
        }
        
        public override bool IsInstalled { get; }
        public string PackageId { get; }
        public override Task<bool> TryInstall()
        {
            throw new NotImplementedException();
        }
    }
}