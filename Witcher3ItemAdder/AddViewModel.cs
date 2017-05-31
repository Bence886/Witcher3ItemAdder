using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Witcher3ItemAdder
{
    public class TagNeeded :INotifyPropertyChanged
    { //tag needed osztály a tag-ek pipái miatt kellett --kardokhoz van!
        private SwordTagTypes tag; //a kard típusa
        private bool needed; //kell-e?
        public TagNeeded(SwordTagTypes tag, bool needed)
        {
            this.tag = tag;
            this.needed = needed;
        }
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public SwordTagTypes Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value; OnPropertyChanged();
            }
        }

        public bool Needed
        {
            get
            {
                return needed;
            }

            set
            {
                needed = value; OnPropertyChanged(); OnPropertyChanged("Level");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is TagNeeded)
            { //ha uyagn az a Tag enum értéke akkor egyenlőek
                return Tag == (obj as TagNeeded).tag;
            }
            return false;
        }
    }
    public class ArmorTagNeeded : INotifyPropertyChanged
    { //Az előző másolata csak --armorra!
        private ArmorTagTypes tag;
        private bool needed;
        public ArmorTagNeeded(ArmorTagTypes tag, bool needed)
        {
            this.tag = tag;
            this.needed = needed;
        }
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public ArmorTagTypes Tag
        {
            get
            {
                return tag;
            }

            set
            {
                tag = value; OnPropertyChanged();
            }
        }

        public bool Needed
        {
            get
            {
                return needed;
            }

            set
            {
                needed = value; OnPropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ArmorTagNeeded)
            {
                return Tag == (obj as ArmorTagNeeded).tag;
            }
            return false;
        }
    }

    public class AddViewModel : INotifyPropertyChanged
    {//view modell az add item ablakhoz
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        ItemType selectedItemType; //az ablak tetjén a combobox használja az item típusát adja meg
        WItems add; //kimenetként használt item változó
       
        CraftingRecipe myCraftingRecipe = new CraftingRecipe();
        Sword mySword;
        Armor myArmor;
        Item myItem;
        int selectedItemTab = 0;
        bool craftingRecipeNeeded = false;
        string searchBar = "";

        BindingList<string> allItems = new BindingList<string>();
        BindingList<string> found = new BindingList<string>();

        public AddViewModel(WItems modify)
        {//ctor
            add = modify;
            LoadAllItems();
            foreach (string akt in MainWindow.vm.Items.Select(x=>x.Name))
            {
                allItems.Add(akt);
            }
            foreach (string akt in allItems)
            {
                found.Add(akt);
            }
            AddWindow.VmDone += AddWindow_VmDone; //az ablak hívja (valami kompatibilitása miatt kellett működik és öröm.!)
        }

        private void LoadAllItems()
        {
            string[] a = new string[0];
            try
            {
                a = File.ReadAllLines("items.txt");
            }
            catch (Exception)
            {

            }
            for (int i = 0; i < a.Length; i++)
            {
                AllItems.Add(a[i]);
            }
        } 

        private void AddWindow_VmDone()
        {
            myArmor = new Armor();
            myCraftingRecipe = new CraftingRecipe();
            myItem = new Item();
            mySword = new Sword();
            if (add != null)
            { //ha van módosítandó
                SelectedItemType = add.Mytype; //kiválasztjuk a típust
                switch (SelectedItemType)
                { //a típus alapján értéket adunk a megfelelő változóknak a bejövő itemből
                    case ItemType.Item: myItem = add as Item; selectedItemTab = 2;
                    break;
                    case ItemType.Weapon: mySword = add as Sword; selectedItemTab = 0;
                    break;
                    case ItemType.Armor: myArmor = add as Armor; selectedItemTab = 1;
                    break;
                    case ItemType.Recipe: myCraftingRecipe = add as CraftingRecipe; selectedItemTab = 3;
                    break;
                }
            }
        }

        private void RecipeIncoming()
        {
            if (add is CraftingRecipe)
            {
                MyCraftingRecipe = add as CraftingRecipe;
            }
        }

        public string SearchBar
        {
            get { return searchBar; }
            set { searchBar = value; OnPropertyChanged(); }
        }
        
        public BindingList<string> AllItems
        {
            get { return allItems; }
            set { allItems = value; OnPropertyChanged(); }
        }
        public BindingList<string> Found
        {
            get { return found; }
            set { found = value; OnPropertyChanged(); }
        }
        public bool CraftingRecipeNeeded
        {
            get { return craftingRecipeNeeded; }
            set { craftingRecipeNeeded = value; OnPropertyChanged(); }
        }

        public ItemType SelectedItemType
        {
            get
            {
                return selectedItemType;
            }

            set
            {
                selectedItemType = value; OnPropertyChanged();
            }
        }
        public int SelectedItemTab
        {
            get { return selectedItemTab; }
            set { selectedItemTab = value; OnPropertyChanged(); }
        }

        public Array ItemTypes
        {
            get { return Enum.GetValues(typeof(ItemType)); }
        }

        public Array CraftsManslevel
        {
            get { return Enum.GetValues(typeof(CraftsmanLevel)); }
        }
        public Array CraftsMansType
        {
            get { return Enum.GetValues(typeof(CraftsmanType)); }
        }

        public Array SwordTypes
        {
            get { return Enum.GetValues(typeof(SwordTypes)); }
        }
        public static Array SwordBuffs
        {
            get { return Enum.GetValues(typeof(SBuff)); }
        }

        public WItems Add
        {
            get
            {
                return add;
            }

            set
            {
                add = value; OnPropertyChanged();
            }
        }

        public Array GetTagTypes
        {
            get { return Enum.GetValues(typeof(SwordTagTypes)); }
        }

        public static Array GetArmorBuffs
        {
            get { return Enum.GetValues(typeof(ABuff)); }
        }

        public Array GetSetType
        {
            get { return Enum.GetValues(typeof(SetType)); }
        }
        public Array GetArmorTagTypes
        {
            get { return Enum.GetValues(typeof(ArmorTagTypes)); }
        }

        public CraftingRecipe MyCraftingRecipe
        {
            get
            {
                return myCraftingRecipe;
            }

            set
            {
                myCraftingRecipe = value; OnPropertyChanged();
            }
        }
        public Armor MyArmor
        {
            get { return myArmor; }
            set { myArmor = value; OnPropertyChanged(); }
        }

        public Item MyItem
        {
            get { return myItem; }
            set { myItem = value; OnPropertyChanged(); }
        }

        public Sword MySword
        {
            get { return mySword; }
            set { mySword = value; OnPropertyChanged(); }
        }
    }
}