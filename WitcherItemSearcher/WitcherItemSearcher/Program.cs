using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WitcherItemSearcher
{
    class Program
    {
        static StreamWriter sw = new StreamWriter("items.txt");
        static string items;
        static List<string> Files = new List<string>();

        static void Main(string[] args)
        {
            ProcessDirectory(@"D:\Projects\Witcher3\gameplay\items");

            foreach (string akt in Files)
            {
                ProcessXFile(akt);
            }
            sw.Write(items);
            Console.WriteLine("Tis Done!");
            Console.ReadKey();
        }

        private static void ProcessXFile(string akt)
        {
            try
            {
                XDocument a = XDocument.Load(akt);

                foreach (XElement temp in a.Root.Elements("definitions").Elements("items").Elements("item"))
                {
                    items += temp.Attribute("name").Value + '\n';
                }
            }
            catch (Exception)
            {
            }
        }

        private static void ProcessDirectory(string targetDirectory)
        {
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
            }
        }

        private static void ProcessFile(string fileName)
        {
            if (fileName.Contains(".xml"))
            { //csak a .xml eket keresi
                Files.Add(fileName);
            }
        }
    }
}
