// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GletredEdShare.AssetModule;
using GletredEdShare.AssetModule.Contexts;
using GletredEdShare.ControlModule;
using GletredEdShare.CoreModule;
using GletredEdShare.TreeModule.Assets;

namespace GletredWpfEditor.AssetBrowser
{
    public class AssetBrowserControlViewModel : ControlViewModel
    {
        private AssetTreeComponent _displayTree;
        private readonly AssetTreeComponent _assetTree;
        private readonly AssetTreeComponent _searchResultRootTree;
        private AssetTreeComponent? _selectTree;
        private string _searchAssetName;

        private AssetViewModel _assetViewModel;

        private double _thumbnailSize;

        public AssetTreeComponent DisplayTree { get => _displayTree; set { _displayTree = value; NotifyPropertyChanged(); } }
        public List<AssetTreeComponent> MultiSelectTrees { get; }
        public AssetTreeComponent? SelectTree
        {
            get => _selectTree;
            set
            {
                _selectTree = value;
                if (_selectTree == null)
                {
                    return;
                }

                _selectTree.IsSelected = _selectTree.IsSubSelected = true;

                if (_selectTree.Parent != null) // root folder = parent is always null 
                {
                    _selectTree.Parent.IsExpanded = true;
                    AssetViewModel.SetParent(_selectTree.Parent as AssetTreeComponent ?? throw new InvalidOperationException());
                }

                NotifyPropertyChanged();
                OnSelectionChanged?.Invoke(value!);
            }
        }

        public double ThumbnailSize { get => _thumbnailSize; set { _thumbnailSize = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(ImageThumbnailSize)); } }

        public double ImageThumbnailSize => _thumbnailSize - 20;

        public bool IsTinyThumbnail => (int)ThumbnailSize == EditorConst.TinyThumbnailSize;

        public bool IsSmallThumbnail => (int)ThumbnailSize == EditorConst.SmallThumbnailSize;

        public bool IsMediumThumbnail => (int)ThumbnailSize == EditorConst.MediumThumbnailSize;

        public bool IsLargeThumbnail => (int)ThumbnailSize == EditorConst.LargeThumbnailSize;

        public bool IsHugeThumbnail => (int)ThumbnailSize == EditorConst.HugeThumbnailSize;


        public AssetViewModel AssetViewModel { get => _assetViewModel; set { _assetViewModel = value; NotifyPropertyChanged(); } }

        public event Action<AssetTreeComponent>? OnSelectionChanged;

        public Action<AssetTreeComponent>? OnEditTempNameEnd { get; private set; }

        public DelegateCommand CreateFolderCommand { get; set; } = null!;

        public DelegateCommand RenameCommand { get; set; } = null!;

        public DelegateCommand ImportCommand { get; set; } = null!;

        public DelegateCommand ChangeThumbnailSizeCommand { get; private set; } = null!;

        public string SearchAssetName
        {
            get => _searchAssetName;
            set
            {
                _searchAssetName = value;

                if (string.IsNullOrEmpty(value))
                {
                    DisplayTree = _assetTree;
                    SelectTree = null;
                }
                else
                {
                    _searchResultRootTree.Child.Clear();
                    DisplayTree = _searchResultRootTree;
                    var searchResults = _assetTree.GetChild(value);
                    foreach (var t in searchResults)
                    {
                        DisplayTree.Child.Add(t);
                    }
                    SelectTree = _searchResultRootTree;
                }

                NotifyPropertyChanged();
            }
        }

        public AssetBrowserControlViewModel()
        {
            _assetViewModel = new AssetViewModel();
            _searchAssetName = string.Empty;
            _assetTree = AssetTreeComponent.CreateNewRootTreeAsDummy();
            _assetTree.Child.Add(AssetDatabase.CreateNewRootTree());
            _searchResultRootTree = AssetTreeComponent.CreateNewRootTreeAsDummy();
            _displayTree = _assetTree;
            MultiSelectTrees = new List<AssetTreeComponent>();

            InitCommands();

            ChangeThumbnailSizeCommand.SafeExecute(true, "Medium");
        }

        private void InitCommands()
        {
            CreateFolderCommand = new DelegateCommand(_ =>
            {
                if (SelectTree != null)
                {
                    var temp = SelectTree.AddChild(null!, "NewFolder");
                    temp.IsSelected = true;

                    SelectTree.IsExpanded = true;

                    //NOTE : IsEditMode sets on in render query execute timing because same frame sets not working.
                    EditorDispatcher.Execute(() =>
                    {
                        temp.IsEditMode = true;
                    }, System.Windows.Threading.DispatcherPriority.Render);
                }

                OnEditTempNameEnd = null;
                OnEditTempNameEnd += tree =>
                {

                    //NOTE: tree is a dummy, so parent to set.
                    AssetViewModel.SetParent(tree.Parent as AssetTreeComponent ?? throw new InvalidOperationException());
                    AssetViewModel.AssetName = tree.Name;

                    AssetViewModel.CreateFolderCommand.SafeExecute(true, true);

                    // The selected tree item will change when remove it, so we needs to call it last.
                    var parent = tree.Parent;
                    if (parent == null)
                    {
                        return;
                    }

                    parent.Child.Remove(tree);
                };


            },
            _ => SelectTree != null);

            RenameCommand = new DelegateCommand(_ =>
            {
                //NOTE : IsEditMode sets on in render query execute timing because same frame sets not working.
                EditorDispatcher.Execute(() =>
                {
                    Debug.Assert(SelectTree != null, nameof(SelectTree) + " != null");
                    SelectTree.IsEditMode = true;
                }, System.Windows.Threading.DispatcherPriority.Render);

                OnEditTempNameEnd = null;
                OnEditTempNameEnd += tree =>
                {
                    AssetViewModel.AssetName = tree.Name;
                    if (AssetViewModel.RenameCommand.SafeExecute(tree, tree) == false)
                    {
                        tree.Name = tree.Owner.Name;
                    }

                    Debug.Assert(SelectTree != null, nameof(SelectTree) + " != null");
                    SelectTree.IsEditMode = false;
                };

            },
            _ => SelectTree != null && SelectTree.Owner is RootFolder == false);

            ChangeThumbnailSizeCommand = new DelegateCommand(

              p =>
              {
                  ThumbnailSize = (string)p! switch
                  {
                      "Tiny" => EditorConst.TinyThumbnailSize,
                      "Small" => EditorConst.SmallThumbnailSize,
                      "Medium" => EditorConst.MediumThumbnailSize,
                      "Large" => EditorConst.LargeThumbnailSize,
                      "Huge" => EditorConst.HugeThumbnailSize,
                      _ => ThumbnailSize
                  };

                  NotifyPropertyChanged(nameof(IsTinyThumbnail));
                  NotifyPropertyChanged(nameof(IsSmallThumbnail));
                  NotifyPropertyChanged(nameof(IsMediumThumbnail));
                  NotifyPropertyChanged(nameof(IsLargeThumbnail));
                  NotifyPropertyChanged(nameof(IsHugeThumbnail));
              }
              ,
              _ => true);

            ImportCommand = new DelegateCommand(_ =>
            {
                var importer = EditorManager.CreateSelectExternalFileWindow();
                var results = importer.ShowWindow(Resources.AseetBrowserToolTip, EditorConst.AssetImportFilter, true);

                if (results == null)
                {
                    return;
                }

                foreach (var r in results)
                {
                    Debug.Assert(SelectTree != null, nameof(SelectTree) + " != null");
                    var context = new AssetContext(r, SelectTree.Owner);
                    var asset = AssetDatabase.ImportAsset(context);
                    SelectTree.AddChild(asset, asset.Name);
                }
            },
            _ => true);
        }

    }
}
