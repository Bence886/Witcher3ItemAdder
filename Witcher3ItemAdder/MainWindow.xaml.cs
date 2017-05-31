using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace Witcher3ItemAdder
{
    public partial class MainWindow : Window
    {
        public static ViewModell vm;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new ViewModell();
            DataContext = vm;
        }

        private void RemoveBTN(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedItem!=null)
            {
                vm.Items.Remove(vm.SelectedItem);
                if (vm.Items.Count > 0)
                {
                    vm.SelectedItem = vm.Items.First();
                }
            }
        }

        private void AddBTN(object sender, RoutedEventArgs e)
        {
            AddWindow AW = new AddWindow(null);
            AW.ShowDialog();
        }

        private void ModifyBTN(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vm.SelectedItem != null)
                {
                    System.Windows.MessageBox.Show("You MUST submit even if you haven't changed anything!\n", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    AddWindow AW = new AddWindow(vm.SelectedItem);
                    vm.Items.Remove(vm.SelectedItem);
                    AW.ShowDialog();
                } else
                {
                    System.Windows.MessageBox.Show("No Item Selected!");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void AddDirBTN(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog FD = new FolderBrowserDialog();

            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vm.WorkingDir = FD.SelectedPath;
                ProcessDirectory(vm.WorkingDir);
            }
        }

        private void ProcessDirectory(string targetDirectory)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            //rekurzív mappakeresés
            try
            {
                string[] fileEntries = Directory.GetFiles(targetDirectory);
                foreach (string fileName in fileEntries)
                    ProcessFile(fileName);

                // fájlok a mappában
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void ProcessFile(string fileName)
        {
            if (fileName.Contains(".xml"))
            { //csak a .docx eket keresi
                vm.Files.Add(fileName);
            }
        }

        Random rnd = new Random();
        private void DuplicateBTN(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedItem!= null)
            {
                switch (vm.SelectedItem.Mytype)
                {
                    case ItemType.Item: Item item = (vm.SelectedItem as Item); vm.Items.Add(new Item(item.Name+rnd.Next(0, 1000)));
                    break;
                    case ItemType.Weapon:Sword sword = (vm.SelectedItem as Sword); vm.Items.Add(new Sword(sword.Name+rnd.Next(0, 1000), sword.Damage, sword.CritChance, sword.Price, sword.SwordType, sword.Bufflist, sword.IconName, sword.EquipTemplate,sword.UnlocalizedName, sword.SlotNum, sword.Tags, sword.Weight, sword.Quality));
                    break;
                    case ItemType.Armor:
                    Armor armor = (vm.SelectedItem as Armor); vm.Items.Add(new Armor(armor.ArmorProp, armor.Name + rnd.Next(0, 1000), armor.Price, armor.Piece, armor.SlotNum, armor.EquipTemplate, armor.UnlocalizedName, armor.IconName, armor.ArmorBuffs, armor.Tags, armor.Weight, armor.Quality));
                    break;
                    default:
                    break;
                }
            }
        }

        private void OpenBTN(object sender, RoutedEventArgs e)
        {
            ItemType ez = ItemType.Item;
            if (vm.SelectedFile!=null)
            {
                Miez mi = new Miez();
                if (mi.ShowDialog() == true || mi.ShowDialog() == false)
                {
                    ez = mi.Type;
                }
                try
                {
                    XMLToListConverter(XDocument.Load(vm.SelectedFile), ez);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            } else
            {
                System.Windows.MessageBox.Show("No File Selected!");
            }
        }

        private void XMLToListConverter(XDocument selectedFile, ItemType a)
        {
            foreach (XElement akt in selectedFile.Root.Elements("definitions").Elements("abilities").Elements("ability"))
            {
                switch (a)
                {
                    case ItemType.Item: //vm.Items.Add(new Item());
                        //vm.Items.Last().FromXmlAbilities(akt);
                    break;
                    case ItemType.Weapon: vm.Items.Add(new Sword());
                        vm.Items.Last().FromXmlAbilities(akt);
                    break;
                    case ItemType.Armor: vm.Items.Add(new Armor());
                        vm.Items.Last().FromXmlAbilities(akt);
                    break;
                    default:
                    break;
                }
            }
            foreach (XElement akt in selectedFile.Root.Elements("definitions").Elements("items").Elements("item"))
            {
                try
                {
                    switch (a)
                    {
                        case ItemType.Item:
                        vm.Items.Add(new Item());
                        vm.Items.Last().FromXmlItems(akt);
                        break;
                        case ItemType.Weapon:
                        vm.Items.Where(x => x.Name == akt.Attribute("name").Value).First().FromXmlItems(akt);
                        break;
                        case ItemType.Armor:
                        vm.Items.Where(x => x.Name == akt.Attribute("name").Value).First().FromXmlItems(akt);
                        break;
                        default:
                        break;
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            }
        }

        private void SaveBTN(object sender, RoutedEventArgs e)
        {
            if (vm.Items.Where(x=>x.Mytype==ItemType.Weapon).Count()>0)
            {
                SwordsSave();
            }
            if (vm.Items.Where(x => x.Mytype == ItemType.Armor).Count() > 0)
            {
                ArmorSave();
            }
            if (vm.Items.Where(x => x.Mytype == ItemType.Item).Count() > 0)
            {
                ItemsSave();
            }
            if (vm.Shops.Where(x=>x.Shop.Count()>0).Count()>0)
            {
                ShopsSave();
            }
            if (vm.Containers.Where(x=>x.Shop.Count()>0).Count()>0)
            {
                LootSave();
            }
            if (false) //not implemented yet!
            {
                MonsterLootSave();
            }
            if (vm.Items.Where(x => x.Mytype == ItemType.Recipe).Count() > 0)
            {
                RecipesSave();
            }
        }

        private void RecipesSave()
        {
            XDocument armors = new XDocument(
                new XElement("redxml",
                    new XElement("definitions",
                        new XElement("items",
                            vm.Items.Where(X => X.Mytype == ItemType.Recipe).Select(x => x.ToXmlItems()))),
						new XElement("custom",
							new XElement("crafting_schematics",
								vm.Items.Where(X => X.Mytype == ItemType.Recipe).Select(x => x.ToXmlAbilities())))));

            armors.Declaration = new XDeclaration("1.0", "UTF-16", "true");
            try
            {
                armors.Save(vm.WorkingDir + "\\" + vm.FileName + "_crafting_recipes.xml");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No Filename or path selected!\nOr no authorization.");
            }
        }

        private void ArmorSave()
        {
            XDocument armors = new XDocument(new
                XElement("redxml",
                    new XElement("definitions",
                        new XElement("abilities",
                            vm.Items.Where(X => X is Armor).Select(x => x.ToXmlAbilities())),
                        new XElement("items",
                            vm.Items.Where(X => X is Armor).Select(x => x.ToXmlItems())))));

            armors.Declaration = new XDeclaration("1.0", "UTF-16", "true");
            try
            {
                armors.Save(vm.WorkingDir + "\\" + vm.FileName + "_armors.xml");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No Filename or path selected!\nOr no authorization.");
            }
        }

        private void MonsterLootSave()
        {
            System.Windows.MessageBox.Show("Monster Loot not implemented!");
        }

        private void LootSave()
        {
            XDocument container = new XDocument(new XElement("redxml",
                  new XElement("definitions",
                      new XElement("loot_definitions", vm.Containers.Where(x => x.Shop.Count > 0).Select(x => x.ToXmlContainer())))));
            container.Declaration = new XDeclaration("1.0", "UTF-16", "true");
            try
            {
                container.Save(vm.WorkingDir + "\\" + vm.FileName + "_containers.xml");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No Filename or path selected!\nOr no authorization.");
            }
            System.Windows.MessageBox.Show("Files created at: \n " + vm.WorkingDir + "\\");
        }

        private void SwordsSave()
        {
            XDocument weapons = new XDocument(new
                XElement("redxml",
                    new XElement("definitions",
                        new XElement("abilities",
                            vm.Items.Where(X => X is Sword).Select(x => x.ToXmlAbilities())),
                        new XElement("items",
                            vm.Items.Where(X => X is Sword).Select(x => x.ToXmlItems())))));

            weapons.Declaration = new XDeclaration("1.0", "UTF-16", "true");
            try
            {
                weapons.Save(vm.WorkingDir + "\\" + vm.FileName + "_swords.xml");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No Filename or path selected!\nOr no authorization.");
            }
        }

        private void ItemsSave()
        {
            XDocument items = new XDocument(new
               XElement("redxml",
                   new XElement("definitions",
                       new XElement("items",
                           vm.Items.Where(X => X is Item).Select(x => x.ToXmlItems())))));

            items.Declaration = new XDeclaration("1.0", "UTF-16", "true");
            try
            {
                items.Save(vm.WorkingDir + "\\" + vm.FileName + "_items.xml");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No Filename or path selected!\nOr no authorization.");
            }
        }

        private void ShopsSave()
        {
            XDocument shop = new XDocument(new XElement("redxml",
                   new XElement("definitions",
                       new XElement("loot_definitions", vm.Shops.Where(x => x.Shop.Count > 0).Select(x => x.ToXmlShop())))));
            shop.Declaration = new XDeclaration("1.0", "UTF-16", "true");
            try
            {
                shop.Save(vm.WorkingDir + "\\" + vm.FileName + "_shop.xml");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No Filename or path selected!\nOr no authorization.");
            }
            System.Windows.MessageBox.Show("Files created at: \n " + vm.WorkingDir + "\\");
        }

        private void NewShopSelected(object sender, SelectionChangedEventArgs e)
        {
            NewNoShop();
        }

        private void NewNoShop()
        {
            if (vm.SelectedShop != null)
            {
                vm.SelectedShopNoItems.Clear();
                vm.SelectedShopNoItems = new BindingList<WItems>(vm.Items.Except(vm.SelectedShop.Shop).ToList());
            }
        }

        private void NewNoContainer()
        {
            if (vm.SelectedShop != null)
            {
                vm.SelectedShopNoItems.Clear();
                vm.SelectedShopNoItems = new BindingList<WItems>(vm.Items.Except(vm.SelectedShop.Shop).ToList());
            }
        }

        private void AddToShop(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedShop!=null && vm.SelectedItem!=null)
            {
                vm.SelectedShop.Shop.Add(vm.SelectedItem);
                NewNoShop();
            }
        }

        private void RemoveFromShop(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedShop != null && vm.SelectedShopItem != null)
            {
                vm.SelectedShop.Shop.Remove(vm.SelectedShopItem);
                NewNoShop();
            }
        }

        private void NewContainerSelected(object sender, SelectionChangedEventArgs e)
        {
            NewNoContainer();
        }

        private void AddToContainer(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedShop != null && vm.SelectedItem != null)
            {
                vm.SelectedShop.Shop.Add(vm.SelectedItem);
                NewNoContainer();
            }
        }

        private void RemoveFromContainer(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedShop != null && vm.SelectedShopItem != null)
            {
                vm.SelectedShop.Shop.Remove(vm.SelectedShopItem);
                NewNoContainer();
            }
        }

        public static void SubmitButtonPressed(WItems add)
        {
            vm.Items.Add(add);
        }
    }
}