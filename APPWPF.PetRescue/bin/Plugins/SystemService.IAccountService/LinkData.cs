using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Customer.Utility;
namespace SystemService.IAccountService
{
    public class LinkData : XMLData
    {
        public bool IsAllowed { get; set; }
        public string KeyIdentity { get; set; }
        public string DisplayName { get; set; }
        public string LinkURL { get; set; }

        //业务方法
        public override void FromXmlData(XmlNode node)
        {
            this.KeyIdentity = IdentityUtility.NewClientIdentity();
            this.DisplayName = base.GetAttribute("DisplayName", node);
            this.LinkURL = base.GetAttribute("Source", node);
        }
    }
}
