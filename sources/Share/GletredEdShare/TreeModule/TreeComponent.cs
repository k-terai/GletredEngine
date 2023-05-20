// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GletredEdShare.CoreModule;


namespace GletredEdShare.TreeModule
{
    public class TreeComponent<T> : NotifyPropertyChangedBase
       where T : class
    {
        private string _name = string.Empty;
        private bool _isSelected;
        private bool _isSubSelected;
        private bool _isExpanded;
        private bool _isEditMode;
        private int _iconIndex;
        private Uri? _iconUri;
        private Uri? _thumbnailUri;
        private TreeComponent<T>? _parent;
        private ObservableCollection<TreeComponent<T>> _child;

        /// <summary>
        /// Does tree owner accept drops? (Ex Folder =  true, Texture = false)
        /// </summary>
        protected bool IsOwnerAllowDrop;

        public string Name { get => _name; set { _name = value; NotifyPropertyChanged(); } }

        public T Owner { get; }

        public bool IsSubSelected { get => _isSubSelected; set { _isSubSelected = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(IsAllowDrop)); } }
        public bool IsSelected { get => _isSelected; set { _isSelected = value; NotifyPropertyChanged(); NotifyPropertyChanged((nameof(IsAllowDrop))); } }
        public bool IsExpanded { get => _isExpanded; set { _isExpanded = value; NotifyPropertyChanged(); } }

        public bool IsEditMode { get => _isEditMode; set { _isEditMode = value; NotifyPropertyChanged(); } }

        public int IconIndex { get => _iconIndex; set { _iconIndex = value; NotifyPropertyChanged(); } }
        public Uri? IconUri { get => _iconUri; set { _iconUri = value; NotifyPropertyChanged(); } }
        public Uri? ThumbnailUri { get => _thumbnailUri; set { _thumbnailUri = value; NotifyPropertyChanged(); } }
        public TreeComponent<T>? Parent { get => _parent; set { _parent = value; NotifyPropertyChanged(); } }
        public ObservableCollection<TreeComponent<T>> Child { get => _child; set { _child = value; NotifyPropertyChanged(); } }
        public string Path => _parent == null ? Name : System.IO.Path.Combine(_parent.Path, Name);

        /// <summary>
        /// Related <see cref="IsOwnerAllowDrop"/> <seealso cref="IsSelected"/> <seealso cref="IsSubSelected"/>
        /// </summary>
        public virtual bool IsAllowDrop => IsOwnerAllowDrop && !IsSubSelected && !IsSelected;

        /// <summary>
        /// Called before the Tree's parent changes.
        /// </summary>
        public Func<bool>? OnCheckChangeParent { get; set; }

        /// <summary>
        /// Called when parent changed.
        /// </summary>
        /// <value> 1 = old parent 2 = new parent</value>
        public Action<TreeComponent<T>, TreeComponent<T>>? OnParentChanged { get; set; }

        /// <summary>
        /// Create new root tree component.
        /// </summary>
        /// <param name="owner">Root owner object.</param>
        /// <param name="ownerName">Root owner object name.</param>
        /// <returns>Return instance.</returns>
        public static TreeComponent<T> CreateNewRootTree(T owner, string ownerName)
        {
            return new TreeComponent<T>(owner, ownerName, null);
        }

        /// <summary>
        /// Select between tree1 and tree2.
        /// </summary>
        /// <param name="tree1"></param>
        /// <param name="tree2"></param>
        /// <returns>Select component list.</returns>
        public static List<TreeComponent<T>>? RangeSelect(TreeComponent<T>? tree1, TreeComponent<T>? tree2)
        {
            if (tree1 == null || tree2 == null)
            {
                return null;
            }

            var selects = new List<TreeComponent<T>>();
            TreeComponent<T> top;
            TreeComponent<T> under;

            var root = tree1.GetRoot();

            // Whether tree1 or tree2 is higher up
            if (root.GetHierarchy(tree1) < root.GetHierarchy(tree2))
            {
                top = tree1;
                under = tree2;
            }
            else
            {
                top = tree2;
                under = tree1;
            }

            selects.Add(top);
            top.IsSubSelected = true; //If Top is the root node, the  flag is not set, so set it here.

            var isEnd = false;
            RangeSelectRecursive(root, root, under, root.GetHierarchy(top), root.GetHierarchy(under), ref isEnd, selects);

            return selects;
        }

        /// <summary>
        /// Add child tree.
        /// </summary>
        /// <param name="owner">Child tree owner.</param>
        /// <param name="ownerName">Child tree owner name</param>
        /// <returns>Return new child tree component.</returns>
        public TreeComponent<T> AddChild(T owner, string ownerName)
        {
            var ct = new TreeComponent<T>(owner, ownerName, this);
            Child.Add(ct);
            return ct;
        }

        /// <summary>
        /// Root self and return the number of hierarchy of tree specified by the argument.
        /// </summary>
        /// <param name="childTree">Child tree.</param>
        /// <returns>Returns the number of hierarchy (returns -1 if not in hierarchy)</returns>
        public int GetHierarchy(TreeComponent<T> childTree)
        {
            var count = 0;
            var isFind = false;

            if (childTree == this)
            {
                return count;
            }

            GetHierarchyRecursive(this, childTree, ref count, ref isFind);

            if (!isFind)
            {
                count = -1;
            }

            return count;
        }

        /// <summary>
        /// Checks if tree specified by the argument is in the hierarchy.
        /// </summary>
        /// <param name="checkTree">Check tree.</param>
        /// <returns>Return true if child.</returns>
        public bool IsChild(TreeComponent<T> checkTree)
        {
            return GetHierarchy(checkTree) != -1; //-1 no child.
        }

        /// <summary>
        /// Get root tree.
        /// </summary>
        /// <returns>Return root instance.</returns>
        public TreeComponent<T> GetRoot()
        {
            return _parent == null ? this : _parent.GetRoot();
        }

        /// <summary>
        /// Reset select status.
        /// </summary>
        /// <param name="topHierarchyOnly">True = reset select status only components of yourself and top hierarchy</param>
        public void ResetSelectIncludeChild(bool topHierarchyOnly = false)
        {
            IsSelected = IsSubSelected = false;

            if (topHierarchyOnly)
            {
                foreach (var c in Child)
                {
                    c.IsSelected = c.IsSubSelected = false;
                }
            }
            else
            {
                ResetSelectIncludeChildRecursive(this);
            }
        }

        /// <summary>
        /// Change parent tree.
        /// </summary>
        /// <param name="newParent">New parent.</param>
        /// <returns>Return true if change success.</returns>
        public bool ChangeParent(TreeComponent<T> newParent)
        {
            Debug.Assert(Parent != null, nameof(Parent) + " != null");

            var beforeParent = Parent;

            //Check new parent allow drop currently.
            if (!newParent.IsAllowDrop)
            {
                return false;
            }

            //No change same parent.
            if (beforeParent == newParent)
            {
                return false;
            }

            //Not make child a new parent.
            if (IsChild(newParent))
            {
                return false;
            }

            //additional check.
            var result = OnCheckChangeParent?.Invoke();
            if (result.HasValue && !result.Value) return false;

            Parent.Child.Remove(this);
            Parent = newParent;
            Parent.Child.Add(this);

            OnParentChanged?.Invoke(beforeParent, newParent);
            return true;
        }

        public List<TreeComponent<T>> GetChild(string searchPattern, bool isTopTreeOnly = false)
        {
            var result = new List<TreeComponent<T>>();
            result.AddRange(Child);

            if (isTopTreeOnly == false)
            {
                GetAllChildRecursive(this, result);
            }

            if (string.IsNullOrEmpty(searchPattern) == false)
            {
                result = result.Where(tree => tree.Name.Contains(searchPattern)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Constructor.(As child)
        /// </summary>
        /// <param name="owner">Owner object.</param>
        /// <param name="name">Owner name.</param>
        /// <param name="parent">Parent.(Null == Root)</param>
        protected TreeComponent(T owner, string name, TreeComponent<T>? parent)
        {
            Owner = owner;
            _child = new ObservableCollection<TreeComponent<T>>();

            Name = name;
            Parent = parent;
            IsSelected = IsExpanded;
            IsOwnerAllowDrop = true;
        }


        private static void GetHierarchyRecursive(TreeComponent<T> parent, TreeComponent<T> target, ref int count, ref bool isFind)
        {
            foreach (var c in parent.Child)
            {
                if (c == target)
                {
                    ++count;
                    isFind = true;
                    return;
                }
                else if (!isFind)
                {
                    ++count;
                    GetHierarchyRecursive(c, target, ref count, ref isFind);
                }
            }

        }

        private static void RangeSelectRecursive(TreeComponent<T> root, TreeComponent<T> top, TreeComponent<T> under, int topHierarchy, int underHierarchy, ref bool isEnd, ICollection<TreeComponent<T>> selects)
        {

            foreach (var c in top.Child)
            {
                if (isEnd) return;

                var hierarchy = root.GetHierarchy(c);
                if (topHierarchy <= hierarchy && underHierarchy >= hierarchy)
                {
                    c.IsSubSelected = true;
                    selects.Add(c);

                }

                if (c == under)
                {
                    isEnd = true;
                    return;
                }

                RangeSelectRecursive(root, c, under, topHierarchy, underHierarchy, ref isEnd, selects);
            }
        }

        private static void ResetSelectIncludeChildRecursive(TreeComponent<T> parent)
        {
            foreach (var c in parent.Child)
            {
                c.IsSelected = c.IsSubSelected = false;
                ResetSelectIncludeChildRecursive(c);
            }
        }

        private static void GetAllChildRecursive(TreeComponent<T> parent, List<TreeComponent<T>> result)
        {
            Debug.Assert(parent != null, nameof(parent) + " != null");

            foreach (var c in parent.Child)
            {
                result.AddRange(c.Child);
                GetAllChildRecursive(c, result);
            }
        }
    }
}
