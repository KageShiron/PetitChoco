using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PetitChoco.ViewModels
{
    public class FileTreeItem
    {
        private readonly DirectoryInfo dir;
        private readonly FileInfo file;

        public FileTreeItem(DirectoryInfo info)
        {
            dir = info;
        }
        public FileTreeItem(FileInfo info)
        {
            file = info;
        }

        public string Label => (dir?.Name) ?? file.Name;
        public bool IsDirectory => dir != null;
        public string FullPath => dir?.FullName ?? file.FullName;

        public IEnumerable<FileTreeItem> Children => GetChildren(dir);

        public static IEnumerable<FileTreeItem> GetChildren(DirectoryInfo dir)
        {
            return dir?.GetDirectories().Select(x => new FileTreeItem(x))
                .Concat(dir.GetFiles().Select(x => new FileTreeItem(x)));
        }
    }
}
