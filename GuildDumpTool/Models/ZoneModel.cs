using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildDumpTool.Model
{
    public class ZoneModel : ObservableObject
    {
        public string fZoneName;
        public string ZoneName
        {
            get { return fZoneName; }
            set
            {
                if (value != fZoneName)
                {
                    fZoneName = value;
                    RaisePropertyChanged("ZoneName");
                }
            }
        }
    }
}
