using GuildDumpTool.ViewModel;
using System;
using System.Windows;

namespace GuildDumpTool.Views
{
    /// <summary>
    /// Interaction logic for PreferenceView.xaml
    /// </summary>
    public partial class PreferenceView : Window
    {
        public PreferenceView()
        {
            InitializeComponent();
            PreferenceViewModel vm = new PreferenceViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }
    }
}
