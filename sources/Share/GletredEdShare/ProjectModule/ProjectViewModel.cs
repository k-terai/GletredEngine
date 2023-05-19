// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using System.Text;
using GletredEdShare.CoreModule;
using GletredEdShare.LocalizationModule;

namespace GletredEdShare.ProjectModule
{
    public class ProjectViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _location = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                var location = Path.Combine(_location, _name);
                var result = Project.IsValidName(_name, location);

                if (result == EditorCommon.Result.Ok)
                {
                    ClearError();
                }
                else
                {
                    SetErrors(new[] { LocalizationManager.GetString(result) });
                }

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ProjectNameError));
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                var result = Project.IsValidLocation(_location);

                //Check project parent directory exists.
                if (result == EditorCommon.Result.Ok)
                {
                    ClearError();
                }
                else
                {
                    SetErrors(new[] { LocalizationManager.GetString(result) });
                }

                //Since we want to check the entire path, enter a value in Name and perform a property check.
                Name = _name;

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LocationError));
            }
        }

        public string? ProjectNameError
        {
            get
            {
                var e = GetErrors(nameof(Name));
                return (e as string?[])?[0];
            }
        }

        public string? LocationError
        {
            get
            {
                var e = GetErrors(nameof(Location));
                return (e as string?[])?[0];
            }
        }

        public DelegateCommand? CreateProjectCommand { get; set; }

        public event Action<Project>? OnProjectCreated;

        public ProjectViewModel()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Location = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Name = "NewProject";

            InitCommands();
        }

        private void InitCommands()
        {
            CreateProjectCommand = new DelegateCommand
            (
                _ =>
                {
                    var p = Project.Create(Name, Location);
                    OnProjectCreated?.Invoke(p);
                }
                ,
                _ => !HasErrors);
        }

    }
}
