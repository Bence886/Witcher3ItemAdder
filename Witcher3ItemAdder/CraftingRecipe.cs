using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Witcher3ItemAdder
{
    public class CraftingRecipe : WItems, INotifyPropertyChanged
    {
        ItemType mytype = ItemType.Recipe;
        CraftsmanLevel cl = CraftsmanLevel.Journeyman;
        CraftsmanType ct = CraftsmanType.Crafter;
        string name = "";
        int price = 0;
        int craftPrice = 0;
        string unlocalizedname = "";
        string iconpath = "";
        string craftedItemname = "";
        BindingList<Ingredient> ingredients = new BindingList<Ingredient>();

        public CraftingRecipe()
        {

        }

        public CraftingRecipe(CraftsmanLevel cl, CraftsmanType ct, string name, int price, int craftingPrice, string uname, string iname, string ciname, List<Ingredient> ingredients)
        {
            this.cl = cl;
            this.ct = ct;
            this.name = name;
            this.price = price;
            this.CraftPrice = craftingPrice;
            this.unlocalizedname = uname;
            this.iconpath = iname;
            this.craftedItemname = ciname;
            foreach (Ingredient akt in ingredients)
            {
                this.ingredients.Add(akt);
            }
        }

        public XElement ToXmlItems()
        {
            return new XElement("item",
                new XAttribute("name", Name),
                new XAttribute("category", "crafting_schematic"),
                new XAttribute("price", price),
                new XAttribute("weight", "0.1"),
                new XAttribute("stackable", "100"),
                new XAttribute("localisation_key_name", unlocalizedname),
                new XAttribute("localisation_key_description", unlocalizedname + "_desc"),
                new XAttribute("icon_path", iconpath),
                new XElement("tags", "ReadableItem, mod_crafting"));
        }

        public XElement ToXmlAbilities()
        {
            return new XElement("schematic",
                new XAttribute("name_name", Name),
                new XAttribute("craftedItem_name", craftedItemname),
                new XAttribute("craftsmanLevel_name", cl.ToString().Replace('_', ' ')),
                new XAttribute("craftsmanType_name", ct),
                new XAttribute("price", craftPrice),
                new XElement("ingredients",
                    ingredients.Select(x => x.ToXML())));
        }

        public void FromXmlAbilities(XElement a)
        {
            throw new NotImplementedException();
        }

        public void FromXmlItems(XElement a)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            if (obj is WItems)
            {
                WItems a = (obj as WItems);
                return name.CompareTo(a.Name);
            }
            return 0;
        }

        public CraftsmanType Ct
        {
            get { return ct; }
            set { ct = value; OnPropertyChanged(); }
        }
        public CraftsmanLevel Cl
        {
            get { return cl; }
            set { cl = value; OnPropertyChanged(); }
        }
        public BindingList<Ingredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; OnPropertyChanged(); }
        }
        public int Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); }
        }
        public int CraftPrice
        {
            get { return craftPrice; }
            set { craftPrice = value; OnPropertyChanged(); }
        }
        public string Unlocalizedname
        {
            get { return unlocalizedname; }
            set { unlocalizedname = value; OnPropertyChanged(); }
        }

        public string Iconpath
        {
            get { return iconpath; }
            set { iconpath = value; OnPropertyChanged(); }
        }
        public string CraftedItemname
        {
            get { return craftedItemname; }
            set { craftedItemname = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value; OnPropertyChanged();
            }
        }

        public ItemType Mytype
        {
            get
            {
                return mytype;
            }

            set
            {
                mytype = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler hander = PropertyChanged;
            if (hander != null) hander(this, new PropertyChangedEventArgs(name));
        }

        public WItems GetNewItems()
        {
            return new CraftingRecipe(cl, ct, name, price, CraftPrice, unlocalizedname, iconpath, craftedItemname, ingredients.ToList());
        }
    }
}