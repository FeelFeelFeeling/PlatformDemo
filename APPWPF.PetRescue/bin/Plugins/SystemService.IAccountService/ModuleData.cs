using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Customer.Utility;
namespace SystemService.IAccountService
{
    /// <summary>
    /// 模块数据类型
    /// </summary>
    public class ModuleData : XMLData
    {
        public string KeyIdentity { get; set; }
        public string DisplayName { get; set; }
        public string ModuleURL { get; set; }
        public int OrderBy { get; set; }
        public ObservableCollection<LinkData> Links { get; set; }

        //业务方法
        public override void FromXmlData(XmlNode node)
        {
            if (Links == null)
            {
                Links = new ObservableCollection<LinkData>();
            }

            this.KeyIdentity = IdentityUtility.NewClientIdentity();
            this.DisplayName = base.GetAttribute("DisplayName", node);
            this.ModuleURL = base.GetAttribute("DefaultContentSource", node);
            this.OrderBy = 1;

            foreach (XmlNode item in node.ChildNodes)
            {
                LinkData temp = new LinkData();
                temp.FromXmlData(item);
                this.Links.Add(temp);
            }
        }
    }
}
