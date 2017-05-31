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
    public class ArmorEffect: INotifyPropertyChanged
    {
        public ArmorEffect(ABuff type)
        {
            this.type = type;
        }

        ABuff type;
        float chance;

        public ABuff Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value; OnPropertyChanged();
            }
        }

        public float Chance
        {
            get
            {
                return chance;
            }

            set
            {
                chance = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        public object ToXMLNameChance()
        {
            return new XElement(type.ToString() ,
                    new XAttribute("type", "add"),
                    new XAttribute("min", chance),
                    new XAttribute("max", chance));
        }
        public override bool Equals(object obj)
        {
            if (obj is ArmorEffect)
            {
                return type == (obj as ArmorEffect).type;
            }
            return false;
        }
    }
}
