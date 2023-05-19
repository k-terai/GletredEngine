// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using GletredEdShare.CoreModule;

namespace GletredEdShare.LogModule
{
    public sealed class Log : NotifyPropertyChangedBase
    {
        private static readonly List<Log> SActiveLogs = new();
        private LogData _data;
        private ObservableCollection<LogData.LogPack> _displayLogs;
        private EditorCommon.LogType _displayLogType;

        public ObservableCollection<LogData.LogPack> DisplayLogs { get => _displayLogs; set { _displayLogs = value; NotifyPropertyChanged(); } }
        public int InfoCount => _data.Infos.Count;
        public int WarningCount => _data.Warnings.Count;
        public int ErrorCount => _data.Errors.Count;
        public int AllLogCount => InfoCount + WarningCount + ErrorCount;
        public EditorCommon.LogType DisplayLogType { get => _displayLogType; set { _displayLogType = value; NotifyPropertyChanged(); } }

        public bool IsEnableInfo => DisplayLogType.HasFlag(EditorCommon.LogType.Information);

        public bool IsEnableWarning => DisplayLogType.HasFlag(EditorCommon.LogType.Warning);
        public bool IsEnableError => DisplayLogType.HasFlag(EditorCommon.LogType.Error);

        public void ChangeDisplay(EditorCommon.LogType displayLog)
        {
            DisplayLogType = displayLog;
            DisplayLogs.Clear();

            if (DisplayLogType.HasFlag(EditorCommon.LogType.Information))
            {
                foreach (var d in _data.Infos)
                {
                    DisplayLogs.Add(d);
                }
            }

            if (DisplayLogType.HasFlag(EditorCommon.LogType.Warning))
            {
                foreach (var d in _data.Warnings)
                {
                    DisplayLogs.Add(d);
                }
            }

            if (DisplayLogType.HasFlag(EditorCommon.LogType.Error))
            {
                foreach (var d in _data.Errors)
                {
                    DisplayLogs.Add(d);
                }
            }

            NotifyCommonProperties();

        }

        public void Add(LogData.LogPack pack)
        {
            switch (pack.Type)
            {
                case EditorCommon.LogType.Information:
                    _data.Infos.Add(pack);
                    break;
                case EditorCommon.LogType.Warning:
                    _data.Warnings.Add(pack);
                    break;
                case EditorCommon.LogType.Error:
                    _data.Errors.Add(pack);
                    break;
                case EditorCommon.LogType.None:
                    break;
                case EditorCommon.LogType.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (DisplayLogType.HasFlag(pack.Type))
            {
                DisplayLogs.Add(pack);
            }

            NotifyCommonProperties();
        }

        public void Clear()
        {
            DisplayLogs.Clear();
            _data.Infos.Clear();
            _data.Warnings.Clear();
            _data.Errors.Clear();
            NotifyCommonProperties();
        }

        public bool Save(string filePath)
        {
            try
            {
                return SerializationModule.Serializer.Serialize(_data, filePath);
            }
            catch (IOException exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }

        public void Load(string filePath)
        {
            try
            {
                _data = SerializationModule.Serializer.Deserialize<LogData>(filePath);
                ChangeDisplay(DisplayLogType);
            }
            catch (IOException exception)
            {
                Debug.LogException(exception);
            }
        }

        public static Log Create()
        {
            var l = new Log(new LogData());

            Debug.OnLog += l.OnLog;
            Debug.OnWarning += l.OnWarning;
            Debug.OnError += l.OnError;

            SActiveLogs.Add(l);
            return l;
        }

        public static void Destroy(Log log)
        {
            if (!SActiveLogs.Contains(log))
            {
                return;
            }

            Debug.OnLog -= log.OnLog;
            Debug.OnWarning -= log.OnWarning;
            Debug.OnError -= log.OnError;
            SActiveLogs.Remove(log);
        }

        private void OnLog(string? message, string? from)
        {
            Add(new LogData.LogPack()
            {
                From = from!,
                Message = message!,
                Type = EditorCommon.LogType.Information,
                Time = DateTime.Now
            });
        }

        private void OnWarning(string? message, string? from)
        {
            Add(new LogData.LogPack()
            {
                From = from!,
                Message = message!,
                Type = EditorCommon.LogType.Warning,
                Time = DateTime.Now
            });
        }

        private void OnError(string? message, string? from)
        {
            Add(new LogData.LogPack()
            {
                From = from!,
                Message = message!,
                Type = EditorCommon.LogType.Error,
                Time = DateTime.Now
            });     
        }

        private Log(LogData data)
        {
            _data = data;
            _displayLogs = new();
            ChangeDisplay(EditorCommon.LogType.All);
        }

        private void NotifyCommonProperties()
        {
            NotifyPropertyChanged(nameof(InfoCount));
            NotifyPropertyChanged(nameof(WarningCount));
            NotifyPropertyChanged(nameof(ErrorCount));
            NotifyPropertyChanged(nameof(IsEnableInfo));
            NotifyPropertyChanged(nameof(IsEnableWarning));
            NotifyPropertyChanged(nameof(IsEnableError));
        }
    }
}
