using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RecipeReader
{
    public abstract class XmlModel
    {
        public string XmlDisplayName;

        public XmlModel[] Children { get; set; }


        //public XmlNode BuildXml(XmlNode parentNode, string text)
        //{
        //    //    if(Children == null || Children.Length == 0)
        //    //    {
        //    //        return parentNode;
        //    //    }
        //    //    int index = 0;
        //    //    while (index < Children.Length)
        //    //    {
        //    //        text = text.Split(Children[index].XmlDisplayName)[1];
        //    //       var inner text = text.Split(Children[index].XmlDisplayName)[1];

        //    //        index++;
        //    //    }

        //    //    return null;
        //    //}
        //}
    }
}
