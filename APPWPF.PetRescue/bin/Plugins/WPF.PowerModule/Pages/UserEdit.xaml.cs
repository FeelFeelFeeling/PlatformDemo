using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WPF.PowerModule.ViewModel;
using System.Collections.ObjectModel;
using Component.EditControl;
namespace WPF.PowerModule.Pages
{
    /// <summary>
    /// UserEdit.xaml 的交互逻辑
    /// </summary>
    partial class UserEdit : UserControl
    {
        public UserEdit(ObservableCollection<ControlDataItem> source)
        {
            InitializeComponent();
            if (source != null)
            {
                this.DataShows.ItemsSource = source;
            }
        }
    }
}
