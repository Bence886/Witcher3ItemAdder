using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Witcher3ItemAdder
{
    public interface WItems :IComparable
    {
        string Name { get; set; }
        ItemType Mytype { get; set; }

         XElement ToXmlAbilities();
         XElement ToXmlItems();

        WItems GetNewItems();

        void FromXmlAbilities(XElement a);
        void FromXmlItems(XElement a);
    }
}
