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
    public class SwordEffect:INotifyPropertyChanged
    {
        public SwordEffect(SBuff type)
        {
            this.type = type;
            foreach (SBuffChancename akt in ViewModell.SwordBuffNames)
            {
                SBCN.Add(akt);
            }
        }

        public static List<SBuffChancename> SBCN = new List<SBuffChancename>();
        SBuff type;
        float chance;

        public SBuff Type
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

        public XElement ToXMLName()
        {
            return new XElement(type.ToString(), new XAttribute("is_ability", "true"));
        }

        internal object ToXMLChance()
        {
            return new XElement(SBCN.Where(x=>(int)x==(int)type).First().ToString(),
                    new XAttribute("type", "add"),
                    new XAttribute("min", chance),
                    new XAttribute("max", chance));
        }

        public override bool Equals(object obj)
        {
            if (obj is SwordEffect)
            {
                return type == (obj as SwordEffect).type;
            }
            return false;
        }
    }
}