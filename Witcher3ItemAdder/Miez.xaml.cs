using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Witcher3ItemAdder
{
    /// <summary>
    /// Interaction logic for Miez.xaml
    /// </summary>
    public partial class Miez : Window, INotifyPropertyChanged
    {
        public Miez()
        {
            InitializeComponent();
            DataContext = this;
        }
        private ItemType type = ItemType.Item;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler !=null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public Array GetTypes
        {
            get { return Enum.GetValues(typeof(ItemType)); }
        }
        public ItemType Type
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

