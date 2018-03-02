using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.PowerModule.UIStrategy
{
    public class BaseUI : INotifyPropertyChanged
    {
        #region 通用属性
        public string KeyIdentity { get; set; }//唯一的标标识

        #endregion

        #region 实现通知接口(通知属性改变)

        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
