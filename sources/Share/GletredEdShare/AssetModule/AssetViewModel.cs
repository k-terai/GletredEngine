// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;
using System.Text;
using GletredEdShare.CoreModule;
using GletredEdShare.LocalizationModule;
using GletredEdShare.TreeModule.Assets;

namespace GletredEdShare.AssetModule
{
    public class AssetViewModel : ViewModelBase
    {
        private string _assetName = string.Empty;

        public NormalFolder? Parent { get; protected set; }

        public AssetTreeComponent? ParentTree { get; protected set; }

        public string AssetName
        {
            get => _assetName;
            set
            {
                _assetName = value;
                if (Parent != null)
                {
                    var result = AssetDatabase.IsValidName(_assetName, Parent);

                    if (result == EditorCommon.Result.Ok)
                    {
                        Errors["AssetName"] = null!;
                    }
                    else
                    {
                        Errors["AssetName"] = new[] { LocalizationManager.GetString(result) };
                    }
                }

                NotifyPropertyChanged();
                NotifyErrorsChanged();
            }
        }

        public DelegateCommand CreateFolderCommand { get; set; } = null!;

        public DelegateCommand RenameCommand { get; set; } = null!;

        public AssetViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            InitCommands();
        }

        public void SetParent(AssetTreeComponent tree)
        {
            Debug.Assert(tree.Owner is NormalFolder);

            Parent = tree.Owner as NormalFolder;
            ParentTree = tree;
        }

        private void InitCommands()
        {
            CreateFolderCommand = new DelegateCommand(_ =>
                {
                    if (Parent == null || ParentTree == null)
                    {
                        return;
                    }

                    var folder = Parent.CreateFolder(AssetName);
                    ParentTree.AddChild(folder, AssetName);
                },
            _ => Parent != null && HasErrors == false);

            RenameCommand = new DelegateCommand(p =>
            {
                var tree = p as AssetTreeComponent;
                Debug.Assert(tree != null);
                tree.Rename(AssetName);
            },
            _ => HasErrors == false);
        }

    }
}
