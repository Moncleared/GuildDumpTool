using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using GuildDumpTool.Model;
using System.Collections.Specialized;

namespace GuildDumpTool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PreferenceViewModel : ViewModelBase
    {
        public ObservableCollection<ZoneModel> ValidZones { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public PreferenceViewModel()
        {
            Title = "Preferences";
            GuildName = Properties.Settings.Default.GuildName;
            if (string.IsNullOrWhiteSpace(GuildName)) GuildName = "Original Gangsters Club";
            ValidZones = new ObservableCollection<ZoneModel>();
            if (Properties.Settings.Default.ValidZones != null && Properties.Settings.Default.ValidZones.Count > 0)
            {
                foreach (string vZone in Properties.Settings.Default.ValidZones)
                {
                    if ( !string.IsNullOrWhiteSpace(vZone)) ValidZones.Add(new ZoneModel { ZoneName = vZone.Trim() });
                }
            } else
            {
                ValidZones.Add(new ZoneModel { ZoneName = "Plane of Knowledge" });
            }
        }
        public Action CloseAction { get; set; }

        private string fTitle;
        public string Title
        {
            get { return fTitle; }
            set
            {
                if (value != fTitle)
                {
                    fTitle = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        private string fGuildName;
        public string GuildName
        {
            get { return fGuildName; }
            set
            {
                if (value != fGuildName)
                {
                    fGuildName = value.Trim();
                    Properties.Settings.Default.GuildName = value.Trim();
                    RaisePropertyChanged("GuildName");
                }
            }
        }

        private RelayCommand fSaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                if (fSaveCommand == null)
                {
                    fSaveCommand = new RelayCommand(this.Save);
                }
                return fSaveCommand;
            }
        }

        private void Save()
        {
            StringCollection vStringCollection = new StringCollection();
            foreach (ZoneModel vModel in ValidZones) vStringCollection.Add(vModel.ZoneName);
            Properties.Settings.Default.ValidZones = vStringCollection;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            this.CloseAction();
        }
    }
}