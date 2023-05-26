// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using GletredEdShare.AssetModule;
using GletredEdShare.ProjectModule;
using GletredWpfEditor.AssetBrowser;
using GletredWpfEditor.LogViewer;
using GletredWpfEditor.Main;
using GletredWpfEditor.ProjectWizard;
using GletredWpfEditor.Utilities.File;
using GletredWpfEditor.Utilities.Folder;
using GletredWpfEditor.Utilities.Save;
using GletredWpfEditor.Preview;

namespace GletredWpfEditor
{
    public static class EditorManager
    {
        public const string MainWindowUriPath = "Main/MainTabWindow.xaml";

        public static IMainWindow MainWindow => (Application.Current.MainWindow as IMainWindow)!;

        /// <summary>
        /// Editor startup.
        /// </summary>
        /// <param name="projectPath">.project file path</param>
        /// <returns>Return true if startup success.</returns>
        public static bool Startup(string projectPath)
        {
            GletredEdShare.LogModule.Debug.OnException += e =>
            {
                MessageBox.Show(e.Message, e.StackTrace, MessageBoxButton.OK, MessageBoxImage.Error);
            };

            Project.Load(projectPath);

            if (!AssetDatabase.Startup())
            {
                return false;
            }

            AssetDatabase.RegisterIconUrl<RootFolder>(ResourceService.Current.GetFluentIconUri(Resources.Icon_AssetBrowser));
            AssetDatabase.RegisterIconUrl<NormalFolder>(ResourceService.Current.GetFluentIconUri(Resources.Icon_Folder));
            AssetDatabase.RegisterIconUrl<Texture>(ResourceService.Current.GetFluentIconUri(Resources.Icon_Texture));

            return AssetDatabase.Initialize();
        }

        /// <summary>
        /// Editor shutdown.
        /// </summary>
        /// <returns>Return true if shutdown success.</returns>
        public static bool Shutdown()
        {
            Application.Current.Shutdown();
            return true;
        }

        public static void Restart(string projectPath)
        {
            var asm = Assembly.GetEntryAssembly();
            if (asm != null)
            {
                var directoryInfo = new FileInfo(asm.Location).Directory;
                if (directoryInfo != null)
                {
                    var exes = directoryInfo.GetFiles("*.exe", SearchOption.TopDirectoryOnly);

                    if (exes.Length == 0)
                    {
                        return;
                    }

                    Process.Start(exes[0].FullName, projectPath);
                }
            }

            Application.Current.Shutdown();
        }

        public static IProjectWizardWindow CreateProjectWizardWindow()
        {
            return new ProjectWizardWindow();
        }

        public static ISelectExternalFileWindow CreateSelectExternalFileWindow()
        {
            return new SelectExternalFileWindow();
        }

        public static ISelectExternalFolderWindow CreateSelectExternalFolderWindow()
        {
            return new SelectExternalFolderWindow();
        }

        public static ISaveFileDialog CreateSaveFileDialog()
        {
            return new SaveFileDialog();
        }

        public static ILogViewerControl CreateLogViewerControl()
        {
            return new LogViewerControl();
        }

        public static IAssetBrowserControl CreateAssetBrowserControl()
        {
            return new AssetBrowserControl();
        }

        public static IPreviewControl CreatePreviewControl()
        {
            return new PreviewControl();
        }

    }
}
