using System;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace RecipeReader
{
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
            NodeRepresentation header =  new NodeRepresentation("Header", @"\d\.", null, ReadingRule.HEADER);

            NodeRepresentation[] ingredients = { new NodeRepresentation("Ingredient", @"\n", null) };
            NodeRepresentation[] steps = { new NodeRepresentation("Step", @"\d\.", null) };

            NodeRepresentation[] categories = { header, new NodeRepresentation("Ingredients", "Ingredients:", ingredients), new NodeRepresentation("Method", "Method:", steps) };
            BuildNode(rootNode, categories, allText);
            doc.AppendChild(rootNode);
            doc.Save(@"C:\Users\abrhm\source\repos\RecipeReader\RecipeReader\EncodedRecipe.xml");
        }

        public static void BuildNode(XmlNode node, NodeRepresentation[] descendents, string text)
        {

            for(int i = 0; i < descendents.Length; i++)
            {
                if (descendents[i].Rule == ReadingRule.HEADER)
                {
                    BuildHeaderNode(text, descendents[i], node);
                    continue;
                }
                if ( (descendents[i].Items != null && descendents[i].Rule == ReadingRule.DEFAULT) || descendents[i].Rule == ReadingRule.SEQUENCE )
                {
                    BuildNodeFromSequence(text, i, descendents, node);
                }
                if ((descendents[i].Items == null && descendents[i].Rule == ReadingRule.DEFAULT) || descendents[i].Rule == ReadingRule.SHALLOW)
                {
                    BuildShallowNode(text, descendents[i], node);
            
                }
     
            }

        }

        private static void BuildHeaderNode(string text, NodeRepresentation nodeRep, XmlNode parentNode)
        {
            var pattern = @"(\d)\.(.+)\[(.+)\].+";

            var match = Regex.Match(text, pattern);
            var recipeNumber = match.Groups[1].Value;
            var recipeTitle = match.Groups[2].Value.Trim();
            var datePosted = match.Groups[3].Value;

            XmlNode newNode = doc.CreateElement(nodeRep.XmlName);
            XmlNode numberNode = doc.CreateElement("RecipeNumber");
            numberNode.InnerText = recipeNumber;
            XmlNode titleNode = doc.CreateElement("Title");
            titleNode.InnerText = recipeTitle;
            XmlNode dateNode = doc.CreateElement("Date");
            dateNode.InnerText = datePosted;
            newNode.AppendChild(numberNode);
            newNode.AppendChild(titleNode);
            newNode.AppendChild(dateNode);
            parentNode.AppendChild(newNode);
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

            BuildNode(newNode, categories[index].Items, nodeText);
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

        private static string GetMiddleString(string firstString, string separator1, string separator2)
        {
            int pFrom = firstString.IndexOf(separator1) + separator1.Length;
            int pTo = firstString.LastIndexOf(separator2);

          return firstString.Substring(pFrom, pTo - pFrom);
        }
    }

}
