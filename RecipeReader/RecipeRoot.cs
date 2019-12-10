using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeReader
{
   public class RecipeRoot : XmlModel
    {
        public RecipeRoot()
        {
            this.Children = new XmlModel[] { new Metadata(), new Content() };
            this.XmlDisplayName = "Recipe";
        }

    }

    public class Content : XmlModel
    {
        public Content()
        {
            this.Children = new XmlModel[] { new Ingredients(), new Method() };
            this.XmlDisplayName = "Content";

        }
    }
    public class Metadata : XmlModel
    {

    }
    public class Ingredients: XmlModel
    {

    }

    public class Method: XmlModel
    {

    }
    public class Ingredient : XmlModel
    {

    }
    public class Step: XmlModel
    {

    }
}
