// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredWpfEditor.Commands;
using System;
using System.Windows;
using GletredEdShare.CoreModule;
using GletredEdShare.LocalizationModule;

namespace GletredWpfEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            EditorCommand.Initialize();
            LocalizationManager.Initialize();

            if (e.Args.Length == 0)
            {
                EditorCommand.OpenProjectWizardWindowCommand.Execute(null);
                return;
            }

            foreach (var s in e.Args)
            {
                if (!s.Contains(EditorConst.ProjectDataExtension))
                {
                    continue;
                }

                if (EditorManager.Startup(s))
                {
                    StartupUri = new Uri(EditorManager.MainWindowUriPath, UriKind.RelativeOrAbsolute);
                }
                else
                {
                    EditorCommand.OpenProjectWizardWindowCommand.Execute(null);
                }
            }
        }
    }
}
