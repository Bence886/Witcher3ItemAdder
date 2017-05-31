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
    public class Sword :INotifyPropertyChanged, WItems
    {
        public Sword()
        {
            foreach (SwordTagTypes akt in Enum.GetValues(typeof(SwordTagTypes)))
            {
                tags.Add(new TagNeeded(akt, false));
            }
            foreach (SBuff akt in AddViewModel.SwordBuffs)
            {
                bufflist.Add(new SwordEffect(akt));
            }
        }

        public Sword(string name, int damage, float crit, int price, SwordTypes type, BindingList<SwordEffect> bl, string icon, string equiptamplate, string uname, int slots, BindingList<TagNeeded> tag, float weight, int quality)
        {
            this.Name = name;
            this.damage = damage;
            this.critChance = crit;
            this.Price = price;
            this.swordType = type;
            bufflist = bl;
            iconName = icon;
            this.equipTemplate = equiptamplate;
            unlocalizedName = uname;
            slotNum = slots;
            tags = tag;
            this.weight = weight;
            this.Quality = quality;

            if (bufflist.Count==0)
            {
                foreach (SBuff akt in AddViewModel.SwordBuffs)
                {
                    bufflist.Add(new SwordEffect(akt));
                }
            }
        }

        string name;
        int price;
        SwordTypes swordType = SwordTypes.silversword;
        int damage;
        float critChance;
        BindingList<SwordEffect> bufflist = new BindingList<SwordEffect>();
        string iconName;
        string equipTemplate;
        string unlocalizedName; //optional
        int slotNum;
        float weight;
        int quality;
        BindingList<TagNeeded> tags = new BindingList<TagNeeded>();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChangde([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        public XElement ToXmlAbilities()
        {
            return new XElement("ability",
                    new XAttribute("name", name + "Ability _Stats"),
                new XElement("quality",
                    new XAttribute("type", "add"),
                    new XAttribute("min", quality)),
                new XElement("weight",
                    new XAttribute("type", "base"),
                    new XAttribute("min", Weight)),
                DamageType(),
                AntiDamageType(),
                new XElement("buff_apply_chance",
                    new XAttribute("type", "add"),
                    new XAttribute("min", "1")),
                new XElement("critical_hit_chance",
                    new XAttribute("type", "add"),
                    new XAttribute("min", critChance)),
                bufflist.Where(x=>x.Chance>0).Select(x=>x.ToXMLName()),
                bufflist.Where(x => x.Chance > 0).Select(x=>x.ToXMLChance()));
        }

        private object DamageType()
        {
            switch (SwordType)
            {
                case SwordTypes.silversword: return new XElement("SilverDamage", new XAttribute("type", "base"), new XAttribute("min", damage));
                case SwordTypes.steelsword: return new XElement("SlashingDamage", new XAttribute("type", "base"), new XAttribute("min", damage));
            }
            return null;
        }

        private XElement AntiDamageType()
        {
            switch (SwordType)
            {
                case SwordTypes.silversword: return new XElement("SlashingDamage", new XAttribute("type", "base"), new XAttribute("min", "1"));
                case SwordTypes.steelsword: return new XElement("SilverDamage", new XAttribute("type", "base"), new XAttribute("min", "1"));
            }return null;
        }

        public XElement ToXmlItems()
        {
            switch (SwordType)
            {
                case SwordTypes.silversword: return SilverXML();
                case SwordTypes.steelsword: return SteelXML();
            }
            return null;
        }

        private XElement SilverXML()
        {
            return new XElement("item",
                    new XAttribute("name", name),
                    new XAttribute("category", SwordType),
                    new XAttribute("price", price),
                    new XAttribute("initial_durability", "100"),
                    new XAttribute("max_durability", "100"),
                    new XAttribute("enhancement_slots", slotNum),
                    new XAttribute("stackable", "1"),
                    new XAttribute("grid_size", "2"),
                    new XAttribute("equip_template", equipTemplate),
                    new XAttribute("equip_slot", SwordType.ToString().Substring(0, SwordType.ToString().IndexOf('w') - 1) + "_sword_back_slot"),
                    new XAttribute("hold_slot", "r_weapon"),
                    new XAttribute("weapon", "true"),
                    new XAttribute("lethal", "true"),
                    new XAttribute("ability_mode", "OnHold"),
                    new XAttribute("hand", "right"),
                    new XAttribute("sound_identification", "long steel"),
                    new XAttribute("draw_event", "DrawWeapon"),
                    new XAttribute("draw_act", "draw_steel_sword_back_act"),
                    new XAttribute("draw_deact", "draw_steel_sword_back_deact"),
                    new XAttribute("holster_event", "HolsterWeapon"),
                    new XAttribute("holster_act", "holster_steel_sword_back_act"),
                    new XAttribute("holster_deact", "holster_steel_sword_back_deact"),
                    new XAttribute("localisation_key_name", unlocalizedName),
                    new XAttribute("localisation_key_description", unlocalizedName + "_desc"),
                    new XAttribute("icon_path",IconName),
                new XElement("tags", Tags.Where(x=>x.Needed).Select(x => x.Tag + ", ")),
                new XElement("base_abilities",
                    new XElement("a", name + "Ability _Stats")),
                new XElement("recycling_parts"),
                new XElement("anim_actions",
                    new XElement("action",
                        new XAttribute("name", "draw"),
                        new XAttribute("event", "DrawWeapon"),
                        new XAttribute("act", "draw_steel_sword_back_act"),
                        new XAttribute("deact", "draw_steel_sword_back_deact")),
                   new XElement("action",
                        new XAttribute("name", "holster"),
                        new XAttribute("event", "HolsterWeapon"),
                        new XAttribute("act", "holster_steel_sword_back_act"),
                        new XAttribute("deact", "holster_steel_sword_back_deact")),
                    new XElement("action",
                        new XAttribute("name", "attack"),
                        new XAttribute("event", "attack_silver_sword_back"),
                        new XAttribute("act", "attack_silver_sword_back_act"),
                        new XAttribute("deact", "attack_silver_sword_back_deact"))),
                new XElement("anim_switches",
                    new XElement("anim_switch",
                        new XAttribute("category", "steelsword"),
                        new XAttribute("equip_slot", "silver_sword_back_slot"),
                        new XAttribute("event", "silver_to_steel"),
                        new XAttribute("switch_act", "silver_to_steel_act"),
                        new XAttribute("switch_deact", "silver_to_steel_deact")),
                    new XElement("anim_switch",
                        new XAttribute("category", "steelsword"),
                        new XAttribute("equip_slot", "steel_sword_back_slot"),
                        new XAttribute("event", "steel_to_silver"),
                        new XAttribute("switch_act", "steel_to_silver_act"),
                        new XAttribute("switch_deact", "steel_to_silver_deact")),
                    new XElement("anim_switch",
                        new XAttribute("category", "steelsword"),
                        new XAttribute("equip_slot", "steel_sword_back_slot"),
                        new XAttribute("event", "steel_to_silver"),
                        new XAttribute("switch_act", "steel_to_silver_act"),
                        new XAttribute("switch_deact", "steel_to_silver_deact"))));
        }

        private XElement SteelXML()
        {
            return new XElement("item",
                    new XAttribute("name", name),
                    new XAttribute("category", SwordType),
                    new XAttribute("price", price),
                    new XAttribute("initial_durability", "100"),
                    new XAttribute("max_durability", "100"),
                    new XAttribute("enhancement_slots", slotNum),
                    new XAttribute("stackable", "1"),
                    new XAttribute("grid_size", "2"),
                    new XAttribute("equip_template", equipTemplate),
                    new XAttribute("equip_slot", SwordType.ToString().Substring(0, SwordType.ToString().IndexOf('w') - 1) + "_sword_back_slot"),
                    new XAttribute("hold_slot", "r_weapon"),
                    new XAttribute("weapon", "true"),
                    new XAttribute("lethal", "true"),
                    new XAttribute("ability_mode", "OnHold"),
                    new XAttribute("hand", "right"),
                    new XAttribute("sound_identification", "long steel"),
                    new XAttribute("draw_event", "DrawWeapon"),
                    new XAttribute("draw_act", "draw_steel_sword_back_act"),
                    new XAttribute("draw_deact", "draw_steel_sword_back_deact"),
                    new XAttribute("holster_event", "HolsterWeapon"),
                    new XAttribute("holster_act", "holster_steel_sword_back_act"),
                    new XAttribute("holster_deact", "holster_steel_sword_back_deact"),
                    new XAttribute("localisation_key_name", unlocalizedName),
                    new XAttribute("localisation_key_description", unlocalizedName + "_desc"),
                    new XAttribute("icon_path",IconName),
                new XElement("tags", Tags.Where(x=>x.Needed).Select(x=>x.Tag + ", ")),
                new XElement("base_abilities",
                    new XElement("a", name + "Ability _Stats")),
                new XElement("recycling_parts"),
                new XElement("anim_actions",
                    new XElement("action",
                        new XAttribute("name", "draw"),
                        new XAttribute("event", "DrawWeapon"),
                        new XAttribute("act", "draw_steel_sword_back_act"),
                        new XAttribute("deact", "draw_steel_sword_back_deact")),
                   new XElement("action",
                        new XAttribute("name", "holster"),
                        new XAttribute("event", "HolsterWeapon"),
                        new XAttribute("act", "holster_steel_sword_back_act"),
                        new XAttribute("deact", "holster_steel_sword_back_deact")),
                    new XElement("action",
                        new XAttribute("name", "attack"),
                        new XAttribute("event", "attack_steel_sword_back"),
                        new XAttribute("act", "attack_steel_sword_back_act"),
                        new XAttribute("deact", "attack_steel_sword_back_deact"))),
                new XElement("anim_switches",
                    new XElement("anim_switch",
                        new XAttribute("category", "silversword"),
                        new XAttribute("equip_slot", "silver_sword_back_slot"),
                        new XAttribute("event", "silver_to_steel"),
                        new XAttribute("switch_act", "silver_to_steel_act"),
                        new XAttribute("switch_deact", "silver_to_steel_deact"))));
        }

        public void FromXmlAbilities(XElement a)
        {
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
                damage = int.Parse(a.Element("SlashingDamage").Attribute("min").Value);
            }
            catch (Exception)
            {
            }
            try
            {
                critChance = float.Parse(a.Element("critical_hit_chance").Attribute("min").Value);
            }
            catch (Exception)
            {
            }
            bufflist = new BindingList<SwordEffect>();
            foreach (SBuff akt in ViewModell.SwordBuffNames)
            {
                try
                {
                    SwordEffect temp = new SwordEffect(akt);
                    temp.Chance = float.Parse(a.Element(SwordEffect.SBCN.Where(x => (int)x == (int)akt).First().ToString()).Attribute("max").Value.Replace('.', ','));
                    bufflist.Add(temp);
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
                    case "steelsword":
                    SwordType = SwordTypes.steelsword;
                    break;
                    case "silversword":
                    SwordType = SwordTypes.silversword;
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
            catch (Exception)
            {
            }
        }

        public int Level
        {
            get
            {
                int lvl = 1;

                switch (swordType)
                {
                    case SwordTypes.silversword: int dmg =(int)(damage - 1); lvl = (int)((1 + dmg - 90) / 10);
                        break;
                    case SwordTypes.steelsword: lvl = (int)((1 + (damage - 25) / 5) - 1);
                        break;
                }

                if (tags.Where(x => x.Tag.ToString().Contains("EP1") && x.Needed).Count() > 0)  lvl -= 1; //hots
                if (quality == 4) lvl -= 1; //relic
                if (quality == 5) lvl -= 2; //wgear

                if (lvl < 1) lvl = 1;
                if (lvl > 70)  lvl = 70;

                return lvl;
            }
        }

        public SwordTypes SwordType
        {
            get
            {
                return swordType;
            }

            set
            {
                swordType = value; OnPropertyChangde(); OnPropertyChangde("Level");
            }
        }

        public int Damage
        {
            get
            {
                return damage;
            }

            set
            {
                damage = value; OnPropertyChangde(); OnPropertyChangde("Level");
            }
        }

        public float CritChance
        {
            get
            {
                return critChance;
            }

            set
            {
                critChance = value; OnPropertyChangde();
            }
        }
        
        public BindingList<SwordEffect> Bufflist
        {
            get
            {
                return bufflist;
            }

            set
            {
                bufflist = value; OnPropertyChangde();
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

        public string EquipTemplate
        {
            get
            {
                return equipTemplate;
            }

            set
            {
                equipTemplate = value; OnPropertyChangde();
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
                slotNum = value; OnPropertyChangde();
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

        ItemType mytype = ItemType.Weapon;

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

        public BindingList<TagNeeded> Tags
        {
            get
            {
                return tags;
            }

            set
            {
                tags = value; OnPropertyChangde(); OnPropertyChangde("Level");
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
                weight = value; OnPropertyChangde();
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
                quality = value; OnPropertyChangde(); OnPropertyChangde("Level");
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
            return new Sword(name, damage, critChance, price, swordType, bufflist, iconName, EquipTemplate, unlocalizedName, slotNum, tags, weight, quality);
        }
    }
}
