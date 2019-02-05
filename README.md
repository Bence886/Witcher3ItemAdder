# Witcher3ItemAdder

A special XML editor for adding and editing different items for the Witcher3 game.

Description:
-------------
With this program you can easily add swords armors and other items to your mod, and you can even add
those items to in game merchants or containers.
It has an easy to use self explaining GUI, which allows for quick and easy work.
This program generates .xml definition files.


Latest Updates:
-------------
1. New better GUI.
2. Option to add Crafting Schematics, to items.
3. Lots of optimization and bug fixes.
4. Item lvl calculator (Thanks to SkacikPL)

How it works:
-------------

1. Select the working directory you want your file to be saved.
2. Write the name of your file(_swords.xml, _items.xml, _shop.xml e.g. will be added for the output files)
3. Go to the Items tab, click Add item fill the form as you desire.
4. You can choose if you need crafting recipe or not.  
    4,1. Fill the other form on the right.  
    4,2. You can choose crafting ingredients by clicking on them, you can search in the list.  
    4,3. The selected items will appear in another list by clicking on them you can remove them fom there.  
    4,4. By editing the number near the"selected items" you determine the number of that item needed in the crafting.  
5. When you click submit the item and the recipe (if checked) will  be added and the Add window will stay open sou you can create the next item.
5. Go to  the Shop/Containers tab, select the shop/container you want your item to be in, select your item click Add.
6. Go back to the first (Tools)tab click save.
7. You'r done the files shouldbe at the directory you selected.

8. You can open a file byselecting a folder, the program will list all the xml files in that directory and it's sub directories. Select your file click open
   select the type of your file. All the items from the file will be added to the list at the Items tab. (The program can only open files created by this program, other files may be corrupted during the process.)

Known issues:
-------------

1. File opening is not tested for every case.
2. Not all the merchants and containers are tested.
3. Adding junk, misc, alchemy,crafting items is not implemented.
4. Item lvl calculator may be incorrect for swords.

Addition:
-------------
WitcherItemSearcher
This is a short program witch lists out Items, Merchants, Locations from game XML-s.
You can modify the paths to get it working.


> **Note:** Please contact me if you have any problem with the program.
