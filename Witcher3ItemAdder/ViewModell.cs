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
    public enum SwordTypes { silversword, steelsword }
    public enum SBuff {ConfusionEffect = 0, SlowdownFrostEffect = 1, BleedingEffect = 2, PoisonEffect = 3, StaggerEffect = 4, BurningEffect=5}
    public enum SBuffChancename { desc_confusionchance_mult=0, desc_freezingchance_mult=1, desc_bleedingchance_mult=2, desc_poinsonchance_mult=3, desc_staggerchance_mult=4, desc_burningchance_mult = 5 }
    public enum ItemType { Item, Weapon, Armor, Recipe}
    public enum JunkType { alchemy_ingredient , crafting_ingredient, junk, misc}
    public enum SwordTagTypes { Quest, PlayerSteelWeapon, PlayerSilverWeapon, Weapon, sword1h, Wooden, mod_weapon, NoShow, NoDrop, SecondaryWeapon, blunt1h, PhantomWeapon, axe2h, Autogen, AutogenForceLevel, TypeAxe, mod_secondary, NoUse, AutogenUseLevelRange, mod_valuable, mod_origin_nml, mod_origin_nilfgaard, mod_origin_novigraad, mod_origin_skellige, mod_origin_nonhuman, TypeClub, mod_legendary, Upgradeable, EP1, mod_origin_ofir }
    public enum ArmorTagTypes { Armor, MediumArmor, mod_armor, LightArmor, Autogen, HeavyArmor, mod_origin_nilfgaard, mod_origin_skellige, mod_legendary, Quest, EP1, Ofir, mod_origin_ofir, DoNotEnhance, AutogenUseLevelRange, AutogenForceLevel }
    public enum SetType { armor, pants, boots, gloves}
    public enum CraftsmanType { Smith, Armorer , Crafter }
    public enum CraftsmanLevel { Master, Journeyman, Grand_Master}
    public enum ABuff { bonus_money, critical_hit_chance, critical_hit_damage_bonus, spell_power_yrden, spell_power_quen, spell_power_axii, staminaRegen_armor_mod, vitality, elemental_resistance_perc, rending_resistance_perc , bludgeoning_resistance_perc , piercing_resistance_perc , slashing_resistance_perc } //még lehet kell hozzá+
    public class ViewModell : INotifyPropertyChanged
    {
        public ViewModell()
        {
            items = new BindingList<WItems>();
            LoadShops();
            LoadContainer();
        }
        private void LoadShops()
        {
            string[] a= new string[0];
            try
            {
                a =  File.ReadAllLines("merchants.txt");
            }
            catch (Exception)
            {
                
            }
            for (int i = 0; i < a.Length; i++)
            {
                shops.Add(new Merchant(a[i]));
            }
        }

        private void LoadContainer()
        {

            string[] a = new string[0];
            try
            {
                a = File.ReadAllLines("containers.txt");
            }
            catch (Exception)
            {

            }
            for (int i = 0; i < a.Length; i++)
            {
                containers.Add(new Merchant(a[i]));
            }
        }

        BindingList<WItems> items;
        BindingList<String> files = new BindingList<string>();
        BindingList<Merchant> shops = new BindingList<Merchant>();
        BindingList<WItems> selectedShopItems = new BindingList<WItems>();
        Merchant selectedShop;
        WItems selectedShopItem;
        string workingDir;
        string selectedFile;
        string fileName="File name";

        BindingList<Merchant> containers = new BindingList<Merchant>();

        public BindingList<WItems> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value; OnPropertyChanged();
            }
        }

        WItems selectedItem;

        public WItems SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                selectedItem = value; OnPropertyChanged();
            }
        }

        public string WorkingDir
        {
            get
            {
                return workingDir;
            }

            set
            {
                workingDir = value; OnPropertyChanged();
            }
        }

        public BindingList<string> Files
        {
            get
            {
                return files;
            }

            set
            {
                files = value; OnPropertyChanged();
            }
        }

        public string SelectedFile
        {
            get
            {
                return selectedFile;
            }

            set
            {
                selectedFile = value; OnPropertyChanged();
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value; OnPropertyChanged();
            }
        }

        public static Array SwordBuffNames
        {
            get { return Enum.GetValues(typeof(SBuffChancename)); }
        }

        public BindingList<Merchant> Shops
        {
            get
            {
                return shops;
            }

            set
            {
                shops = value; OnPropertyChanged();
            }
        }

        public BindingList<WItems> SelectedShopNoItems
        {
            get
            {
                return selectedShopItems;
            }

            set
            {
                selectedShopItems = value; OnPropertyChanged();
            }
        }

        public Merchant SelectedShop
        {
            get
            {
                return selectedShop;
            }

            set
            {
                selectedShop = value; OnPropertyChanged();
            }
        }

        public WItems SelectedShopItem
        {
            get
            {
                return selectedShopItem;
            }

            set
            {
                selectedShopItem = value; OnPropertyChanged();
            }
        }

        public BindingList<Merchant> Containers
        {
            get
            {
                return containers;
            }

            set
            {
                containers = value; OnPropertyChanged();
            }
        }        

        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
