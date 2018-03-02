using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SystemService.IAccountService
{
    public abstract class XMLData
    {
        public string GetAttribute(string attr, System.Xml.XmlNode node)
        {
            string text = string.Empty;
            if (node.Attributes[attr] != null)
            {
                text = node.Attributes[attr].Value;
            }
            string result;
            if (string.IsNullOrEmpty(text))
            {
                result = string.Empty;
            }
            else
            {
                result = text;
            }
            return result;
        }
        public abstract void FromXmlData(XmlNode node);
    }
}
