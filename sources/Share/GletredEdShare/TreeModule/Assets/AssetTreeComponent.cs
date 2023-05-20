// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.AssetModule;

namespace GletredEdShare.TreeModule.Assets
{
    public sealed class AssetTreeComponent : TreeComponent<Asset>
    {
        public Asset? ParentAsset => Parent?.Owner;

        /// <summary>
        /// Create new root tree component.
        /// </summary>
        /// <param name="owner">Root owner asset.</param>
        /// <param name="ownerName">Root owner object name.</param>
        /// <returns>New instance.</returns>
        public new static AssetTreeComponent CreateNewRootTree(Asset owner, string ownerName)
        {
            var root = new AssetTreeComponent(owner, ownerName, null);
            root.ThumbnailUri = root.IconUri = AssetDatabase.GetIconUri<RootFolder>();

            return root;
        }

        /// <summary>
        /// Create new root tree component as dummy.
        /// Ex : using search tree root 
        /// </summary>
        /// <returns>New instance,</returns>
        public static AssetTreeComponent CreateNewRootTreeAsDummy()
        {
            var root = new AssetTreeComponent();
            return root;
        }

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="owner">Owner <see cref="Asset"/>.</param>
        /// <param name="ownerName">Owner asset name.</param>
        /// <returns>Return AssetTreeComponent added as a child of this tree.</returns>
        public new AssetTreeComponent AddChild(Asset owner, string ownerName)
        {
            var ct = new AssetTreeComponent(owner, ownerName, this);
            Child.Add(ct);
            return ct;
        }

        public void Rename(string name)
        {
            Owner.Rename(name);
        }

        private AssetTreeComponent() : base(null!, null!, null!)
        {
            IsOwnerAllowDrop = false;
        }

        private AssetTreeComponent(Asset owner, string name, TreeComponent<Asset>? parent) : base(owner, name, parent)
        {
            if (owner is not NormalFolder)
            {
                return;
            }

            IsOwnerAllowDrop = true;
            ThumbnailUri = IconUri = AssetDatabase.GetIconUri<NormalFolder>();
        }

    }
}
