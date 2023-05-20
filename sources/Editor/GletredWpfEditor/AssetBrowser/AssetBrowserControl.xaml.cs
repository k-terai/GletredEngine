// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GletredEdShare.AssetModule;
using GletredEdShare.TreeModule;
using GletredEdShare.TreeModule.Assets;
using Debug = System.Diagnostics.Debug;

namespace GletredWpfEditor.AssetBrowser
{
    /// <summary>
    /// AssetBrowserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class AssetBrowserControl : IAssetBrowserControl
    {
        private const double MinimumDragDistance = 10.0;
        private Point _lastLeftMouseButtonDownPoint;

        public UserControl Control => this;
        public AssetBrowserControlViewModel ViewModel => DataContext as AssetBrowserControlViewModel ?? throw new InvalidOperationException();

        public AssetBrowserControl()
        {
            InitializeComponent();
        }

        private void AssetTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            var vm = ViewModel;

            //Check ctrl key not input.
            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) != KeyStates.Down & (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) != KeyStates.Down)
            {
                //Reset.
                foreach (var a in vm.MultiSelectTrees)
                {
                    a.IsSubSelected = false;
                }

                vm.MultiSelectTrees.Clear();
            }

            //Check shift key input.
            if ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down || (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down)
            {
                //Range select.
                if (vm.SelectTree != null)
                {
                    var newTree = e.NewValue as AssetTreeComponent;
                    var oldTree = vm.SelectTree;
                    var selects = TreeComponent<Asset>.RangeSelect(oldTree, newTree);
                    if (selects == null) return;

                    foreach (var t in selects.Where(t => !vm.MultiSelectTrees.Contains(t)))
                    {
                        vm.MultiSelectTrees.Add(t as AssetTreeComponent ?? throw new InvalidOperationException());
                    }

                    return;
                }
            }

            vm.SelectTree = (e.NewValue as AssetTreeComponent);
            Debug.Assert(vm.SelectTree != null, "vm.SelectTree != null");

            vm.SelectTree.IsSubSelected = true;

            if (!vm.MultiSelectTrees.Contains(vm.SelectTree))
            {
                vm.MultiSelectTrees.Add(vm.SelectTree);
            }
        }

        private void AssetTreeView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) { return; }
            _lastLeftMouseButtonDownPoint = e.GetPosition(AssetTreeView);
        }

        private void AssetTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Released) { return; }
                if (ViewModel.SelectTree == null) return;

                var currentPosition = e.GetPosition(AssetTreeView);
                if (Math.Abs(currentPosition.X - _lastLeftMouseButtonDownPoint.X) <= MinimumDragDistance &&
                    Math.Abs(currentPosition.Y - _lastLeftMouseButtonDownPoint.Y) <= MinimumDragDistance)
                {
                    return;
                }

                //No drag root folder.
                if (ViewModel.MultiSelectTrees.Count(t => t.Owner is RootFolder) != 0)
                {
                    return;
                }

                DragDrop.DoDragDrop(AssetTreeView, ViewModel.SelectTree, DragDropEffects.Move);
            }
            catch (Exception ex)
            {
                GletredEdShare.LogModule.Debug.LogException(ex);
            }

        }

        private void AssetTreeView_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;

            var destItem = e.OriginalSource as FrameworkElement;
            Debug.Assert(destItem != null, nameof(destItem) + " != null");

            var destTree = destItem.DataContext as AssetTreeComponent;
            _ = e.Data.GetData(typeof(AssetTreeComponent)) as AssetTreeComponent;

            var removeList = new List<AssetTreeComponent>();

            foreach (var check in from at in ViewModel.MultiSelectTrees from check in ViewModel.MultiSelectTrees where check != at && !removeList.Contains(check) where at.IsChild(check) select check)
            {
                removeList.Add(check);
            }

            var moveAssetArray = ViewModel.MultiSelectTrees.Where(t => !removeList.Contains(t)).ToArray();
            foreach (var ma in moveAssetArray)
            {
                Debug.Assert(destTree != null, nameof(destTree) + " != null");
                ma.ChangeParent(destTree);
            }

        }

        private void AssetButton_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button b)
            {
                return;
            }

            if (b.DataContext is AssetTreeComponent at)
            {
                ViewModel.SelectTree = at;
            }
        }

        private void TreeViewTextbox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            Debug.Assert(textBox != null);

            var assetView = textBox.DataContext as AssetTreeComponent;
            Debug.Assert(assetView != null);

            if (!assetView.IsEditMode || textBox.Visibility != Visibility.Visible)
            {
                return;
            }

            textBox.Focus();
            textBox.SelectAll();
        }

        private void TreeViewTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            Debug.Assert(textBox != null);

            var assetView = textBox.DataContext as AssetTreeComponent;

            //Null is possible when preview key down event direct call this method.
            if (assetView is not { IsEditMode: true })
            {
                return;
            }

            ViewModel.OnEditTempNameEnd?.Invoke(assetView);
        }

        private void TreeViewTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            Debug.Assert(textBox != null);

            var assetView = textBox.DataContext as AssetTreeComponent;
            Debug.Assert(assetView != null);

            if (assetView.IsEditMode && (Keyboard.GetKeyStates(Key.Enter) & KeyStates.Down) == KeyStates.Down)
            {
                TreeViewTextbox_LostFocus(sender, null!);
            }
        }
    }
}
