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
    public class Armor : WItems, INotifyPropertyChanged
    {

        string name;
        ItemType mytype = ItemType.Armor;
        int armor;
        SetType piece = SetType.armor;
        int price;
        int slotNum;
        string equipTemplate;
        string unlocalizedName;
        string iconName;
        List<ArmorEffect> armorBuffs = new List<ArmorEffect>();
        List<ArmorTagNeeded> tags = new List<ArmorTagNeeded>();
        float weight;
        int quality;

        public Armor(int armor, string name, int price, SetType piece, int slotnum, string et, string uname, string iname, List<ArmorEffect> armorB, List<ArmorTagNeeded> tags, float we, int qual)
        {
            this.name = name;
            this.price = price;
            this.piece = piece;
            slotNum = slotnum;
            equipTemplate = et;
            unlocalizedName = uname;
            iconName = iname;
            armorBuffs = armorB;
            this.armor = armor;
            this.Tags = tags;
            Weight = we;
            Quality = qual;
            if (armorBuffs.Count == 0)
            {
                foreach (ABuff akt in AddViewModel.GetArmorBuffs)
                {
                    armorBuffs.Add(new ArmorEffect(akt));
                }
            }
        }

        public Armor()
        {
            foreach (ArmorTagTypes akt in Enum.GetValues(typeof(ArmorTagTypes)))
            {
                tags.Add(new ArmorTagNeeded(akt, false));
            }
            foreach (ABuff akt in Enum.GetValues(typeof(ABuff)))
            {
                armorBuffs.Add(new ArmorEffect(akt));
            }
        }

        public int ArmorLVL
        {
            get
            {
                int lvl=0;
                switch (piece)
                {
                    case SetType.armor: lvl=  (int)((1 + (armor - 25) / 5) - 1);
                        break;
                    case SetType.pants: lvl= (int)((1 + (armor - 5) / 2) - 1);
                        break;
                    case SetType.boots: lvl= (int)((1 + (armor - 5) / 2) - 1);
                        break;
                    case SetType.gloves: lvl= (int)((1 + (armor - 1) / 2) - 1);
                        break;
                }

                if (tags.Where(x => x.Tag.ToString().Contains("EP1") && x.Needed).Count() > 0) 
                    lvl -= 1; //hots
                if (quality==4)
                    lvl -= 1; //relic
                if (quality==5)
                    lvl -= 2; //wgear
                if (lvl < 1) lvl = 1;
                if (lvl > 70) lvl = 70;

                return lvl;
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

        public SetType Piece
        {
            get
            {
                return piece;
            }

            set
            {
                piece = value; OnPropertyChanged(); OnPropertyChanged("ArmorLVL");
            }
        }

        public int Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value; OnPropertyChanged();
            }
        }

        public int SlotNum
        {
            get
            {
                return slotNum;
            }

            set
            {
                slotNum = value; OnPropertyChanged();
            }
        }

        public string EquipTemplate
        {
            get
            {
                return equipTemplate;
            }

            set
            {
                equipTemplate = value; OnPropertyChanged();
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
                unlocalizedName = value; OnPropertyChanged();
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
                iconName = value; OnPropertyChanged();
            }
        }

        public List<ArmorEffect> ArmorBuffs
        {
            get
            {
                return armorBuffs;
            }

            set
            {
                armorBuffs = value; OnPropertyChanged();
            }
        }

        public int ArmorProp
        {
            get
            {
                return armor;
            }

            set
            {
                armor = value; OnPropertyChanged(); OnPropertyChanged("ArmorLVL");
            }
        }

        public List<ArmorTagNeeded> Tags
        {
            get
            {
                return tags;
            }

            set
            {
                tags = value; OnPropertyChanged(); OnPropertyChanged("ArmorLVL");
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }

            set
            {
                weight = value; OnPropertyChanged();
            }
        }

        public int Quality
        {
            get
            {
                return quality;
            }

            set
            {
                quality = value; OnPropertyChanged(); OnPropertyChanged("ArmorLVL");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name ="")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler!=null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
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

        public void FromXmlAbilities(XElement a)
        {
            try
            {
                weight = float.Parse(a.Attribute("weight").Value.Replace('.', ','));
            }
            catch (Exception)
            {
                weight = 1;
            }
            try
            {
                quality = int.Parse(a.Attribute("quality").Value);
            }
            catch (Exception)
            {
                quality = 1;
            }
            try
            {
                name = a.Attribute("name").Value.ToString().Substring(0, a.Attribute("name").Value.Length - 14);
            }
            catch (Exception)
            {
                name = "NameNotReadable";
            }
            try
            {
                armor = int.Parse(a.Element("armor").Attribute("min").Value);
            }
            catch (Exception)
            {
            }
            armorBuffs = new List<ArmorEffect>();
            foreach (ABuff akt in Enum.GetValues(typeof(ABuff)))
            {
                try
                {
                    ArmorEffect temp = new ArmorEffect(akt);
                    temp.Chance = float.Parse(a.Element("akt").Attribute("min").Value.Replace('.', ','));
                    armorBuffs.Add(temp);
                }
                catch (Exception)
                {
                }
            }
        }

        public void FromXmlItems(XElement a)
        {
            try
            {
                switch (a.Attribute("category").Value)
                {
                    case "armor": piece = SetType.armor; break;
                    case "boots": piece = SetType.boots; break;
                    case "gloves": piece = SetType.gloves; break;
                    case "pants": piece = SetType.pants; break;
                    default: piece = SetType.armor;
                        break;
                }
                price = int.Parse(a.Attribute("price").Value);
                slotNum = int.Parse(a.Attribute("enhancement_slots").Value);
                equipTemplate = a.Attribute("equip_template").Value;
                unlocalizedName = a.Attribute("localisation_key_name").Value;
                iconName = a.Attribute("icon_path").Value;
                string[] tmp = a.Element("tags").Value.Split(' ');
                throw new NotImplementedException(); //tageket beolvasni!
            }
            catch(Exception){}
        }

        public XElement ToXmlAbilities()
        {
            return new XElement("ability",
                new XAttribute("name", name + "Ability _Stats"),
                new XElement("weight",
                    new XAttribute("type", "base"),
                    new XAttribute("min", weight)),
                new XElement("quality",
                    new XAttribute("type", "add"),
                    new XAttribute("min", quality)),
                new XElement("armor",
                    new XAttribute("type", "base"),
                    new XAttribute("min", armor)),
                ArmorBuffs.Where(x => x.Chance > 0).Select(x=>x.ToXMLNameChance()));
        }

        public XElement ToXmlItems()
        {
            return new XElement("item",
                new XAttribute("name", name),
                new XAttribute("category", Piece),
                new XAttribute("price", price),
                new XAttribute("initial_durability", "100"),
                new XAttribute("max_durability", "100"),
                new XAttribute("enhancement_slots", slotNum),
                new XAttribute("stackable", "1"),
                new XAttribute("grid_size", "2"),
                new XAttribute("ability_mode", "OnMount"),
                new XAttribute("equip_template", equipTemplate),
                new XAttribute("localisation_key_name", unlocalizedName),
                new XAttribute("localisation_key_description", unlocalizedName + "_desc"),
                new XAttribute("icon_path", iconName),
                new XElement("tags", Tags.Where(x=>x.Needed).Select(x => x.Tag + ", ")),
                new XElement("base_abilities",
                    new XElement("a", name + "Ability _Stats")),
                new XElement("recycling_parts"));
        }

        public WItems GetNewItems()
        {
            return new Armor(armor, name, price, piece, slotNum, EquipTemplate, unlocalizedName, iconName, ArmorBuffs, tags, weight, quality);
        }
    }
}
