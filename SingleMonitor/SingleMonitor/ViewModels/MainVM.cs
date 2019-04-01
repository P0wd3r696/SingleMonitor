using Rg.Plugins.Popup.Services;
using SingleMonitor.Models;
using SingleMonitor.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace SingleMonitor.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        private string areaName;

        public string AreaName
        {
            get { return areaName; }
            set
            {
                areaName = value;
                OnPropertyChanged("AreaName");
            }
        }

        private string faultReason;

        public string FaultReason
        {
            get { return faultReason; }
            set
            {
                faultReason = value;
                OnPropertyChanged("FaultReason");
            }
        }

        private int _days;
        public int Days
        {
            get { return _days; }
            set
            {
                _days = value;
                OnPropertyChanged("Days");
            }
        }

        private int _hours;
        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged("Hours");
            }
        }

        private int _minutes;
        public int Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged("Minutes");
            }
        }

        private int _seconds;
        public int Seconds
        {
            get { return _seconds; }
            set
            {
                _seconds = value;
                OnPropertyChanged("Seconds");
            }
        }
        private DateTime loggedDate;

        public DateTime LoggedDate
        {
            get { return loggedDate; }
            set
            {
                loggedDate = value;
                OnPropertyChanged("LoggedDate");
            }
        }

        private TimeSpan duration;

        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public event Action Completed;
        public event Action Ticked;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<string> Reasons { get; set; }
        public ObservableCollection<string> EquipName { get; set; }
        public Command LogNewFaultCommand { get; set; }
        public Command DisplayNewFaultPopupCommand { get; set; }
        public Command ResolvedFaultCommand { get; set; }

        public MainVM()
        {
            Reasons = new ObservableCollection<string>();
            EquipName = new ObservableCollection<string>();
            GetReasons();
            GetName();
            DisplayNewFaultPopupCommand = new Command(DisplayLogFaultPopup);
            LogNewFaultCommand = new Command(LogNewFault);
            ResolvedFaultCommand = new Command(FaultResolved);
        }

        private async void DisplayLogFaultPopup()
        {
            await PopupNavigation.Instance.PushAsync(new LogFaultPopUpView());
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void GetReasons()
        {
            Reasons.Clear();

            Reasons.Add("The Computer Won't Start");
            Reasons.Add("The Screen is Blank");
            Reasons.Add("Abnormally Functioning");
            Reasons.Add("OS Won't Boot");
            Reasons.Add("The Screen is Frozen");
            Reasons.Add("Computer is Slow.");
            Reasons.Add("Strange Noises");
            Reasons.Add("Slow Internet");
        }

        private void GetName()
        {
            EquipName.Clear();

            EquipName.Add("Windows Desktop");
            EquipName.Add("Mac Book Pro");
            EquipName.Add("Ubuntu");
        }
        
        private void StartCounterAsync()
        {
            if (Equipment.IsThereActiveFault())
            {
                SQLiteConnection conn = new SQLiteConnection(App.Database);
                conn.CreateTable<Equipment>();
                var faultyEquipment = conn.Table<Equipment>().Where(u => u.IsActive == true).FirstOrDefault();
                conn.Close();

                Duration = (DateTime.Now - faultyEquipment.StartDate);
                Ticked += MainVM_Ticked;
            }
        }

        private void MainVM_Ticked()
        {
            Days = Duration.Days;
            Hours = Duration.Hours;
            Minutes = Duration.Minutes;
            Seconds = Duration.Seconds;
        }

        public bool AreThereSeconds()
        {
            var activeSeconds = Duration.TotalSeconds > 0;

            if (activeSeconds)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void LogNewFault()
        {
            Equipment equipment = new Equipment()
            {
                Area = AreaName,
                Reason = FaultReason,
                StartDate = DateTime.Now,
                IsActive = true
            };
            Equipment.InsertFault(equipment);
            await PopupNavigation.Instance.PopAsync();
        }

        public void FaultResolved()
        {
            Equipment.DeactivateFault();
        }
    }
}
