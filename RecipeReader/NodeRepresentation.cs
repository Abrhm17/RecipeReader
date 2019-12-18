using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeReader
{
    public enum ReadingRule { DEFAULT, SEQUENCE, SHALLOW, HEADER }

    public class NodeRepresentation
    {
        public string XmlName { get; set; }

        public string StartSeparator { get; set; }

        public ReadingRule Rule { get; set; }

        public NodeRepresentation[] Items { get; set; }

        public NodeRepresentation(string XmlName, string StartSeparator, NodeRepresentation[] Items)
        {
            this.XmlName = XmlName;
            this.StartSeparator = StartSeparator;
            this.Items = Items;
        }
        public NodeRepresentation(string XmlName, string StartSeparator, NodeRepresentation[] Items, ReadingRule Rule)
        {
            this.XmlName = XmlName;
            this.StartSeparator = StartSeparator;
            this.Items = Items;
            this.Rule = Rule;
        }
    }
}
