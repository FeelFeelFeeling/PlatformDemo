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

using Customer.Runtime.UIAttributes;
using FirstFloor.ModernUI.Windows.Navigation;
namespace WPF.PowerModule.Pages
{
    /// <summary>
    /// UserManager.xaml 的交互逻辑
    /// </summary>
    public partial class UserManager : UserControl
    {
        public UserManager()
        {
            InitializeComponent();
            //动态生成绑定
            BindDataGrid();
        }
        private void BindDataGrid()
        {
            //使用viewModel.SelectedItem作为默认类型，也要求必须有此属性并且可以作为加载类型
            foreach (var item in this.viewModel.OperationObject.GetType().GetProperties())
            {
                var list = item.GetCustomAttributes(typeof(UIIsListShow), true);//获取是否显示

                if (list.Count() > 0)//只添加有显示属性的列
                {
                    UIIsListShow s = list[0] as UIIsListShow;
                    if (s.IsListShow)
                    {
                        //获取属性的UI显示名称
                        var l1 = item.GetCustomAttributes(typeof(UIName), true);
                        UIName p1 = l1.Count() > 0 ? l1[0] as UIName : null;
                        string uiName = p1 == null ? item.Name : p1.Name;//如果UI显示名称是空，则按照属性名称显示

                        //动态添加列
                        FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn col = new FirstFloor.ModernUI.Windows.Controls.DataGridTextColumn();
                        col.Header = uiName;

                        Binding bd = new Binding(item.Name);
                        bd.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        bd.Mode = BindingMode.TwoWay;

                        col.Binding = bd;

                        this.DG1.Columns.Add(col);
                    }
                }
            }
        }
    }
}
