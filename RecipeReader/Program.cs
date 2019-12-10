using System;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace RecipeReader
{
    public enum ReadingRule { DEFAULT, SEQUENCE, SHALLOW }

    class Program
    {
       static XmlDocument doc = new XmlDocument();

        static void Main(string[] args)
        {
            BuildXml();


        }
        public static void BuildXml()
        {

            XmlNode rootNode = doc.CreateElement("Recipe");
            var allText = File.ReadAllText(@"C:\Users\abrhm\source\repos\RecipeReader\RecipeReader\1_LemonCake.txt");
            NodeRepresentation[] ingredients = { new NodeRepresentation("Ingredient", @"\n", null) };
            NodeRepresentation[] steps = { new NodeRepresentation("Step", @"\d\.", null) };

            NodeRepresentation[] categories = { new NodeRepresentation("Ingredients", "Ingredients:", ingredients), new NodeRepresentation("Method", "Method:", steps) };
            BuildNode(doc, rootNode, categories, allText);

            var first = allText.Split("]");
             
            var test = GetMiddleString("FALALALALALALALADAH", "FA", "DAH");
            var ing = GetMiddleString(allText, "Ingredients:", "Method:");
            var ingred = Regex.Split(ing, @"\n");
            var metadata = first[0];
            var number = metadata.Split(".")[0]; 
            var afterNumber = metadata.Split(".")[1];
            var afterBracket = afterNumber.Split("[");
            var date = afterBracket[1];
            var title = afterBracket[0].Trim();
            var second = first[1].Split("Ingredients:");
            var leader = second[0];
            var third = second[1].Split("Method:");
            var stepz = Regex.Split(third[1], @"\d\.");
            var recipes = Regex.Split(third[0], @"\n");

            //    if(Children == null || Children.Length == 0)
            //    {
            //        return parentNode;
            //    }
            //    int index = 0;
            //    while (index < Children.Length)
            //    {
            //        text = text.Split(Children[index].XmlDisplayName)[1];
            //       var inner text = text.Split(Children[index].XmlDisplayName)[1];

            //        index++;
            //    }

            //    return null;
            //}
          //   XmlDocument doc = new XmlDocument();
          //  XmlNode rootNode = doc.CreateElement("Recipe");
          //  XmlModel recipeRoot = new RecipeRoot();
          //  recipeRoot.BuildXml(rootNode, allText);
          
        }

        public static void BuildNode(XmlDocument doc, XmlNode node, NodeRepresentation[] descendents, string text)
        {

            for(int i = 0; i < descendents.Length; i++)
            {

                if ( (descendents[i].Items != null && descendents[i].Rule == ReadingRule.DEFAULT) || descendents[i].Rule == ReadingRule.SEQUENCE )
                {
                    BuildNodeFromSequence(text, i, descendents, node);
                }
                if ((descendents[i].Items == null && descendents[i].Rule == ReadingRule.DEFAULT) || descendents[i].Rule == ReadingRule.SHALLOW)
                {
                    BuildShallowNode(text, descendents[i], node);
                    return;
                }
     
                // rootNode.AppendChild(BuildXml(categories[i].Items, nodeText));
            }

        }

        private static void BuildNodeFromSequence(string text, int index, NodeRepresentation[] categories, XmlNode parentNode)
        {
            var endSeparator = string.Empty;
            if (index < categories.Length - 1)
            {
                endSeparator = categories[index + 1].StartSeparator;
            }
            var nodeText = GetMiddleString(text, categories[index].StartSeparator, endSeparator);
            XmlNode newNode = doc.CreateElement(categories[index].XmlName);

            BuildNode(doc, newNode, categories[index].Items, nodeText);
            parentNode.AppendChild(newNode);
        }

        private static void BuildShallowNode(string nodeText, NodeRepresentation shallowNodeRepresentation, XmlNode parentNode)
        {
            var words = Regex.Split(nodeText, shallowNodeRepresentation.StartSeparator);
            foreach (var word in words)
            {
                XmlNode shallowNode = doc.CreateElement(shallowNodeRepresentation.XmlName);
                if (string.IsNullOrWhiteSpace(word))
                {
                    continue;
                }
                shallowNode.InnerText = word.Trim();
                parentNode.AppendChild(shallowNode);
            }
        }

        public static string GetMiddleString(string firstString, string separator1, string separator2)
        {
            int pFrom = firstString.IndexOf(separator1) + separator1.Length;
            int pTo = firstString.LastIndexOf(separator2);

          return firstString.Substring(pFrom, pTo - pFrom);
        }
    }

    public class NodeRepresentation
    {
        public string XmlName { get; set; }

        public string StartSeparator { get; set; }

        public ReadingRule Rule {get; set;}

        public NodeRepresentation[] Items { get; set; }

        public NodeRepresentation(string XmlName, string StartSeparator, NodeRepresentation[] Items)
        {
            this.XmlName = XmlName;
            this.StartSeparator = StartSeparator;
            this.Items = Items;
        }
    }
}
