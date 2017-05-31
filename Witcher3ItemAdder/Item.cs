using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Witcher3ItemAdder
{
    public class Item : INotifyPropertyChanged, WItems
    {
        public void OnPropertyChangde([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        public XElement ToXmlAbilities()
        {
            throw new NotImplementedException();
        }

        public XElement ToXmlItems()
        {
            return new XElement("item",
                    new XAttribute("name", name),
                    new XAttribute("category", JunkItemType),
                    new XAttribute("price", price),
                    new XAttribute("stackable", stackSize),
                    new XAttribute("icon_path", iconName),
                    new XAttribute("localisation_key_name", unlocalizedName),
                    new XAttribute("localisation_key_description", unlocalizedName+"_desc"),
                    new XElement("base_abilities",
                        new XElement("a","Default Junk _Stats"),
                        new XElement("tags", "mod_junk")));
        }

        public void FromXmlAbilities(XElement a)
        {
            throw new NotImplementedException();
        }

        public void FromXmlItems(XElement a)
        {
            name = a.Attribute("name").Value;
            price = int.Parse(a.Attribute("price").Value);
            stackSize = int.Parse(a.Attribute("stackable").Value);
            unlocalizedName = a.Attribute("localisation_key_name").Value;
            iconName = a.Attribute("icon_path").Value;
            switch (a.Attribute("category").Value)
            { 
                case "alchemy_ingredient": junkItemType = JunkType.alchemy_ingredient;
                break;
                case "crafting_ingredient":
                junkItemType = JunkType.crafting_ingredient;
                break;
                case "junk":
                junkItemType = JunkType.junk;
                break;
                case "misc":
                junkItemType = JunkType.misc;
                break;
                default:
                junkItemType = JunkType.alchemy_ingredient;
                break;
            }
        }

        public Item(string n)
        {
            name = n;
        }

        public Item(string name, int price, int stack, string iname, string uname, JunkType junkItemType)
        {
            this.name = name;
            this.price = price;
            StackSize = stack;
            iconName = iname;
            unlocalizedName = uname;
            this.junkItemType = junkItemType;
        }

        public Item()
        {
        }

        string name;
        int price;
        int stackSize;
        string iconName;
        string unlocalizedName;
        JunkType junkItemType;

        public int Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value; OnPropertyChangde();
            }
        }
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value; OnPropertyChangde();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ItemType mytype = ItemType.Item;
        public ItemType Mytype
        {
            get
            {
                return mytype;
            }

            set
            {
                mytype = value; OnPropertyChangde();
            }
        }

        public int StackSize
        {
            get
            {
                return stackSize;
            }

            set
            {
                stackSize = value; OnPropertyChangde();
            }
        }

        public string IconName
        {
            get
            {
                return iconName;
            }

            set
            {
                iconName = value; OnPropertyChangde();
            }
        }

        public string UnlocalizedName
        {
            get
            {
                return unlocalizedName;
            }

            set
            {
                unlocalizedName = value; OnPropertyChangde();
            }
        }

        public JunkType JunkItemType
        {
            get
            {
                return junkItemType;
            }

            set
            {
                junkItemType = value; OnPropertyChangde();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is WItems)
            {
                return name == (obj as WItems).Name;
            }
            return false;
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

        public WItems GetNewItems()
        {
            return new Item(name, price, StackSize, iconName, unlocalizedName, JunkItemType);
        }
    }
}
