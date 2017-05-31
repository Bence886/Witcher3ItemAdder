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
    public class Ingredient :INotifyPropertyChanged
    {
        string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int count = 1;

        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged(); }
        }
        public Ingredient(string name)
        {
            this.name = name;
        }
        public Ingredient(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
        public object ToXML()
        {
            return new XElement("ingredient",
                new XAttribute("quantity", count),
                new XAttribute("item_name", name));
        }

        public Ingredient GetNew()
        {
            return new Ingredient(name, count);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name ="")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
    }
}
