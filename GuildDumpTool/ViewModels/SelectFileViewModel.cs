using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GuildDumpTool.Model;
using System;
using System.Collections.ObjectModel;

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
    public class SelectFileViewModel : ViewModelBase
    {
        public ObservableCollection<ZoneModel> ValidZones { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public SelectFileViewModel()
        {
            ValidZones = new ObservableCollection<ZoneModel>();
            ValidZones.Add(new ZoneModel { ZoneName = @"C:\Bullshit.txt1" });
            ValidZones.Add(new ZoneModel { ZoneName = @"C:\Bullshit.txt2" });
            ValidZones.Add(new ZoneModel { ZoneName = @"C:\Bullshit.txt3" });
            ValidZones.Add(new ZoneModel { ZoneName = @"C:\Bullshit.txt4" });
            Console.WriteLine("Good to go!");
        }

        private ZoneModel fFileName;
        public ZoneModel FileName
        {
            get { return fFileName; }
            set
            {
                if (value != fFileName)
                {
                    fFileName = value;
                    RaisePropertyChanged("FileName");
                }
            }
        }
        

        private RelayCommand fSelectFileCommand;
        public RelayCommand SelectFileCommand
        {
            get
            {
                if (fSelectFileCommand == null)
                {
                    fSelectFileCommand = new RelayCommand(this.Save);
                }
                return fSelectFileCommand;
            }
        }

        private void Save()
        {
            Console.WriteLine("THIS IS A TEST: " + FileName.ZoneName);
        }
    }
}