using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuildDumpTool.Services
{
    class ErrorMessageService
    {
        public void ShowMessageBox(string pMessage)
        {
            MessageBox.Show(pMessage);
        }
    }
}
