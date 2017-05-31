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
    public class Merchant :INotifyPropertyChanged
    {
        string name;
        private BindingList<WItems> shop = new BindingList<WItems>();

        public Merchant(string v)
        {
            this.name = v;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name ="")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        public BindingList<WItems> Shop
        {
            get
            {
                return shop;
            }

            set
            {
                shop = value; OnPropertyChanged();
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

        public XElement ToXmlContainer()
        {
            return new XElement("loot",
                    new XAttribute("name", name),
                    new XAttribute("player_level_min", "0"),
                    new XAttribute("player_level_max", "0"),
                    new XAttribute("quantity_min", "130"),
                    new XAttribute("quantity_max", "185"),
                    new XAttribute("respawn_time", "0"), Items().Select(x => x));
        }

        public XElement ToXmlShop()
        {
            return new XElement("loot", 
                    new XAttribute("name", name),
                    new XAttribute("player_level_min", "0"),
                    new XAttribute("player_level_max", "0"),
                    new XAttribute("quantity_min", "130"),
                    new XAttribute("quantity_max", "185"),
                    new XAttribute("chance", "1"), Items().Select(x=>x));
        }

        private List<XElement> Items()
        {
            List<XElement> a = new List<XElement>();
            foreach (WItems akt in Shop)
            {
                a.Add(new XElement("loot_entry",
                        new XAttribute("name", akt.Name),
                        new XAttribute("player_level_min", "0"),
                        new XAttribute("player_level_max", "0"),
                        new XAttribute("quantity_min", "1"),
                        new XAttribute("quantity_max", "1"),
                        new XAttribute("chance", "1")));
            }

            return a;
        }
    }
}
