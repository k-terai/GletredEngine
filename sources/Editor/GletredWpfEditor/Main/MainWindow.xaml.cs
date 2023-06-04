// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;

namespace GletredWpfEditor.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainWindow
    {
        public MainWindowViewModel ViewModel => (DataContext as MainWindowViewModel)!;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowWindow()
        {
            if (ViewModel.IsDialogMode)
            {
                ShowDialog();
            }
            else
            {
                Show();
            }
        }

        public void SetState(WindowState state)
        {
            ViewModel.State = state;
        }
        private void TopStackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.IsMaximize = WindowState == WindowState.Maximized;
        }

        private void MinimizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsDialogMode) return;

            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsDialogMode) return;

            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ViewModel.IsMaximize = false;
            }
            else
            {
                WindowState = WindowState.Maximized;
                ViewModel.IsMaximize = true;
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TopStackPanel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton != System.Windows.Input.MouseButtonState.Pressed ||
                WindowState != WindowState.Maximized)
            {
                return;
            }

            ViewModel.IsMaximize = false;
            WindowState = WindowState.Normal;
            Left = e.GetPosition(this).X - Width / 2;
            Top = e.GetPosition(this).Y;
            DragMove();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (EditorManager.Shutdown())
            {
                return;
            }

            e.Cancel = true;
        }

    }
}
