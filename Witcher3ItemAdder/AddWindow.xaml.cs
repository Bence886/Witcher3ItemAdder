    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Witcher3ItemAdder
{
    public partial class AddWindow : Window
    {   //az item add ablak önnön maga!
        public delegate void VMCreated(); //valami event előzőleg magyarázva
        public static event VMCreated VmDone; //hozzávaló cucc
        private void OnVmDone()
        {
            if (VmDone != null) VmDone();
        }

        public AddWindow(WItems modify)
        {
            InitializeComponent();
            this.modify = modify; //átadjuk a modify értékét (lehet null is!)
            Loaded += AddWindow_Loaded; //loaded mert na
        }
        public AddViewModel vm; //view modell az ablakhoz
        Grid g; //az ablak grid-je ;)
        WItems modify; //a módosításra hívott item (lehet null is)
        private void AddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new AddViewModel(modify); //a vm létrehozása a modify al
            vm.Add = modify;
            DataContext = vm;
            g = Content as Grid; 
            OnVmDone();
        }

        private void Number_PreviewKeyDown(object sender, KeyEventArgs e)
        { //számotnyomjálcsakb+
            e.Handled = !((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Delete || e.Key==Key.Back || e.Key == Key.Decimal || (e.Key>=Key.NumPad0 && Key.E <= Key.NumPad9) || e.Key==Key.Left || e.Key == Key.Right || e.Key == Key.Enter || e.Key == Key.Tab);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SubmitButtonPressed(object sender, RoutedEventArgs e)
        {
            switch (vm.SelectedItemType)
            {
                case ItemType.Item: MainWindow.SubmitButtonPressed(vm.MyItem.GetNewItems());
                    break;
                case ItemType.Weapon: MainWindow.SubmitButtonPressed(vm.MySword.GetNewItems());
                    break;
                case ItemType.Armor: MainWindow.SubmitButtonPressed(vm.MyArmor.GetNewItems());
                    break;
            }
            if (vm.CraftingRecipeNeeded)
            {
                MainWindow.SubmitButtonPressed(vm.MyCraftingRecipe.GetNewItems());
            }
            vm.AllItems.Add(MainWindow.vm.Items.Select(x=>x.Name).Last());
            vm.Found.Add(MainWindow.vm.Items.Select(x => x.Name).Last());
        }

        private void NewTabSelected(object sender, SelectionChangedEventArgs e)
        {
            if (vm!=null)
            {
                switch (((sender as TabControl).SelectedItem as TabItem).Tag.ToString())
            {
                case "Sword": vm.SelectedItemType = ItemType.Weapon; vm.MyCraftingRecipe.CraftedItemname = vm.MySword.Name; vm.CraftingRecipeNeeded = false;
                    break;
                case "Armor": vm.SelectedItemType = ItemType.Armor; vm.MyCraftingRecipe.CraftedItemname = vm.MyArmor.Name; vm.CraftingRecipeNeeded = false;
                    break;
                case "Item": vm.SelectedItemType = ItemType.Item; vm.MyCraftingRecipe.CraftedItemname = vm.MyItem.Name; vm.CraftingRecipeNeeded = false;
                    break;
                case "Recipe": vm.SelectedItemType = ItemType.Recipe; vm.CraftingRecipeNeeded = true;
                    break;
            }
            }
        }
        private void ClearButtonClicked(object sender, RoutedEventArgs e)
        {
            vm.MyArmor = new Armor();
            vm.MyItem = new Item();
            vm.MySword = new Sword();
            vm.MyCraftingRecipe = new CraftingRecipe();
        }

        private void NameChangedEvent(object sender, TextChangedEventArgs e)
        {
            vm.MyCraftingRecipe.CraftedItemname = (sender as TextBox).Text;
        }

        private void SearchbarChanged(object sender, TextChangedEventArgs e)
        {
            var a = vm.AllItems.Where(x=>x.Contains((sender as TextBox).Text));
            vm.Found.Clear();
            foreach (var akt in a)
            {
                vm.Found.Add(akt);
            }
        }

        private void NewItemClicked(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem != null)
            {
                 vm.MyCraftingRecipe.Ingredients.Add(new Ingredient((sender as ListBox).SelectedItem.ToString()));
                vm.AllItems.Remove((sender as ListBox).SelectedItem.ToString());
                vm.Found.Remove((sender as ListBox).SelectedItem.ToString());
            }
        }

        private void OldItemClicked(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem != null)
            {
                vm.AllItems.Add(((sender as ListBox).SelectedItem as Ingredient).Name);
                vm.Found.Add(((sender as ListBox).SelectedItem as Ingredient).Name);
                vm.MyCraftingRecipe.Ingredients.Remove((Ingredient)(sender as ListBox).SelectedItem);
            }
        }
    }
}