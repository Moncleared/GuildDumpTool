using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using GuildDumpTool.Views;
using GalaSoft.MvvmLight.Ioc;
using System.IO;
using System.Linq;
using System.Windows;

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
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Title = "Original Gangsters Club - Guild Dump Tool";
            
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LogFile))
                LogFile = Properties.Settings.Default.LogFile;
            else
                LogFile = "Enter Guild Dump File Path here (or use browse)";
        }

        private string fTitle;
        public string Title
        {
            get { return fTitle; }
            set
            {
                if ( value != fTitle )
                {
                    fTitle = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        private string fLogFile;
        public string LogFile
        {
            get { return fLogFile; }
            set
            {
                if (value != fLogFile)
                {
                    fLogFile = value;
                    RaisePropertyChanged("LogFile");
                }
            }
        }

        private string fOutputConsole;
        public string OutputConsole
        {
            get { return fOutputConsole; }
            set
            {
                if (value != fOutputConsole)
                {
                    fOutputConsole = value;
                    RaisePropertyChanged("OutputConsole");
                }
            }
        }

        private RelayCommand fBrowseCommand;
        public RelayCommand BrowseCommand
        {
            get
            {
                if (fBrowseCommand == null)
                {
                    fBrowseCommand = new RelayCommand(this.PopBrowseDialog);
                }
                return fBrowseCommand;
            }
        }

        private void PopBrowseDialog()
        {
            //SelectFileView vWindow = new SelectFileView();
            //vWindow.ShowDialog();
            //return;
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
               string filename = dlg.FileName;
               LogFile = filename;
            }
        }

        private RelayCommand fMonitorLogCommand;
        public RelayCommand MonitorLogCommand
        {
            get
            {
                if (fMonitorLogCommand == null)
                {
                    fMonitorLogCommand = new RelayCommand(
                        async () =>
                        {
                            await GetDKPInfo();
                        });
                }
                return fMonitorLogCommand;
            }
        }

        private Task GetDKPInfo()
        {
            return Task.Run(() =>
            {
                if (File.Exists(LogFile))
                {
                    string[] vArrayOfValidZones = new string[Properties.Settings.Default.ValidZones.Count];
                    Properties.Settings.Default.ValidZones.CopyTo(vArrayOfValidZones, 0);
                    string vOutput = string.Empty;
                    using (StreamReader vStreamReader = new StreamReader(LogFile))
                    {
                        string vLine = string.Empty;
                        string vZoneName = string.Empty;
                        while ((vLine = vStreamReader.ReadLine()) != null)
                        {
                            //ZoneName is the 7th tab delimited item
                            vZoneName = vLine.Split("\t".ToCharArray())[6].Trim();
                            vZoneName = RemoveInstanceId(vZoneName);

                            if (vArrayOfValidZones.Contains(vZoneName, StringComparer.OrdinalIgnoreCase))
                            {
                                vLine = InsertDKP(vLine);
                                vOutput += vLine + Environment.NewLine;
                            }
                        }
                    }
                    OutputConsole = vOutput;
                }
            });
        }

        private string InsertDKP(string pDkpLine)
        {
            //1          Doakes    61    Enchanter    Group   Leader       Yes
            string[] vSplit = pDkpLine.Split("\t".ToCharArray());
            string vOutput = string.Format("{0}\t{1}\t{2}\t{3}\t\t\t\t\t",1, vSplit[0].Trim(), vSplit[1].Trim(), vSplit[2].Trim());
            return vOutput;
        }

        private string RemoveInstanceId(string pZoneName)
        {
            string vOutput = pZoneName;
            var vTmp = vOutput.Split(' ');
            int vInstanceId;
            if ( int.TryParse(vTmp[vTmp.Length-1], out vInstanceId) )
            {
                vOutput = vOutput.Replace(vInstanceId.ToString(), "");
            }

            return vOutput.Trim();
        }

        private RelayCommand fClearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                if (fClearCommand == null)
                {
                    fClearCommand = new RelayCommand(ClearLogs);
                }
                return fClearCommand;
            }
        }

        private void ClearLogs()
        {
            OutputConsole = string.Empty;
        }

        private RelayCommand fPrefCommand;
        public RelayCommand PrefCommand
        {
            get
            {
                if (fPrefCommand == null)
                {
                    fPrefCommand = new RelayCommand(PrefWindow);
                }
                return fPrefCommand;
            }
        }

        private void PrefWindow()
        {
            PreferenceView vWindow = new PreferenceView();
            vWindow.ShowDialog();
        }

        private RelayCommand fCopyCommand;
        public RelayCommand CopyCommand
        {
            get
            {
                if (fCopyCommand == null)
                {
                    fCopyCommand = new RelayCommand(CopyToClipboard);
                }
                return fCopyCommand;
            }
        }

        private void CopyToClipboard()
        {
            if (!string.IsNullOrWhiteSpace(OutputConsole)) Clipboard.SetText(OutputConsole);
        }
    }
}