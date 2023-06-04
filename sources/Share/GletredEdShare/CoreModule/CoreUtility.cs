// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GletredEdShare.CoreModule
{
    public static class CoreUtility
    {
        public static DirectoryInfo GetExecutingAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return new FileInfo(assembly.Location).Directory ?? throw new InvalidOperationException();
        }

        public static FileInfo? GetFileInfoInParent(string searchFileNameWithExtension,DirectoryInfo directoryInfo)
        {
            while (directoryInfo != null)
            {
                var info = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly).FirstOrDefault(
                    t => t.Name.Equals(searchFileNameWithExtension));
                if (info != null)
                {
                    return info;
                }

                if (directoryInfo.Parent == null)
                {
                    break;
                }

                directoryInfo = directoryInfo.Parent;
            }

            return null;
        }  
    }
}
