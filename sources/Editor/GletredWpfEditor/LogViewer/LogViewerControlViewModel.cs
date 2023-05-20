// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.ControlModule;
using GletredEdShare.CoreModule;
using GletredEdShare.LogModule;

namespace GletredWpfEditor.LogViewer
{
    public class LogViewerControlViewModel : ControlViewModel
    {
        private Log _targetLog = null!;
        private string _commandInput = null!;

        public Log TargetLog
        {
            get => _targetLog;
            set { _targetLog = value; NotifyPropertyChanged(); }
        }

        public DelegateCommand ToggleDisplayLogCommand { get; set; } = null!;
        public DelegateCommand ClearLogCommand { get; set; } = null!;

        public DelegateCommand PlayConsoleCommand { get; set; } = null!;

        public string CommandInput { get => _commandInput; set { _commandInput = value; NotifyPropertyChanged(); } }

        public LogViewerControlViewModel()
        {
            TargetLog = Log.Create();
            InitCommands();
        }

        private void InitCommands()
        {
            ToggleDisplayLogCommand = new DelegateCommand(

                p =>
                {
                    var flag = (EditorCommon.LogType)((p) ?? EditorCommon.LogType.All);
                    var current = TargetLog.DisplayLogType;

                    if (TargetLog.DisplayLogType.HasFlag(flag))
                    {
                        TargetLog.ChangeDisplay(current & ~flag);
                    }
                    else
                    {
                        TargetLog.ChangeDisplay(current | flag);
                    }

                }
                ,
                _ => true);

            ClearLogCommand = new DelegateCommand(

              _ =>
              {
                  TargetLog.Clear();
              }
              ,
              _ => true);


            PlayConsoleCommand = new DelegateCommand(

              _ =>
              {

              }
              ,
              _ => true);
        }
    }
}
