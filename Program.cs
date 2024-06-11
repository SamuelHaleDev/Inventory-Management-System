using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.XPath;
using Microsoft.VisualBasic;

namespace Inventory_Management_System;


class Item 
{
  public string Name { get; set; }
  public int Quantity { get; set; }
  public decimal Price { get; set; }

  public Item(string name = "", int quantity = 0, decimal price = 0m) 
  {
    Name = name;
    Quantity = quantity;
    Price = price;
  }

  public override string ToString()  
  {
    return $"Name: {Name}, Quantity {Quantity}, Price: {Price:C}";
  }
}


class Inventory 
{
  public List<Item> Items;

  public Inventory() 
  {
    Items = new List<Item>();
  }

  public void AddItem(Item item) 
  {
    bool itemExists = DoesItemExist(item);

    if (itemExists) 
    {
      Console.WriteLine($"I| Item exists!");
      return;
    }

    Items.Add(item);
    Console.WriteLine($"I| Item added! {item.ToString()}");
  }

  public void RemoveItem(Item item) 
  {
    bool itemExists = DoesItemExist(item);

    if (!itemExists) 
    {
      Console.WriteLine($"I| Item does not exist!");
      return;
    }

    Items.Remove(item);
  }

  public void ModifyItem(Item originalItem, Item modifiedItem) 
  {
    bool itemExists = DoesItemExist(originalItem);

    if (!itemExists) 
    {
      Console.WriteLine($"I| Item does not exist!");
    }

    Items[Items.IndexOf(originalItem)] = modifiedItem;
  }

  public void PrintInventory() 
  {
    Console.WriteLine("####################### Printing Inventory #######################");

    for (int i = 0; i < Items.Count(); i++) 
    {
      Console.WriteLine($"I| Item {i+1}: {Items[i].ToString()}");
    }
  }

  public Item FindItem(string name, bool clone) 
  {
    Item foundItem = Items.Find(item => item.Name == name);

    if (foundItem is null) 
    {
      Console.WriteLine("I| Item could not be found!");
    } 

    return clone ? new Item(foundItem.Name, foundItem.Quantity, foundItem.Price) : foundItem;
  }

  public bool DoesItemExist(Item item) 
  {
    return !(Items.Find(tempItem => tempItem.Name == item.Name) == new Item());
  }
}

class Program 
{
  public static Inventory inventory = new Inventory();

  static void Main(string[] args) 
  {
    char input = 'A';
    char[] USER_INPUT_OPTIONS = {'A', 'B', 'C', 'D', 'E', 'X'};
 
    while (Array.Exists(USER_INPUT_OPTIONS, OPTION => OPTION == input)) 
    {
      DisplayMenu();

      input = Char.ToUpper((char)Console.Read());
      var newLineCharacter = Console.ReadLine();

      if (input == 'X') break;

      RouteUserInput(input);
    }
  }

  static void DisplayMenu() 
  {
    Console.WriteLine("###############################");
    Console.WriteLine("Welcome to the Inventory Management System");
    Console.WriteLine("A: Add Item");
    Console.WriteLine("B: Delete Item");
    Console.WriteLine("C: View Inventory");
    Console.WriteLine("D: Update Item");
    Console.WriteLine("E: Search Item");
    Console.WriteLine("X: Quit");
    Console.WriteLine("###############################");
  }

  public static void RouteUserInput(char input) 
  {
    switch (input) 
    {
        case 'A':
          AddItem();
          break;
        case 'B':
          DeleteItem();
          break;
        case 'C':
          inventory.PrintInventory();
          break;
        case 'D':
          UpdateItem();
          break;
        case 'E':
          SearchItem();
          break;
    }
  }

  public static void AddItem() 
  {
    string input = "";

    while (true) 
    {
      const int NUMBER_OF_ATTRIBUTES = 3;
      Console.Write("C| Enter new item details (name, quantity, price): ");
      input = Console.ReadLine();
      var details = input.Split(',');

      if (details.Length != NUMBER_OF_ATTRIBUTES) 
      {
        Console.WriteLine("C| Invalid input. Please enter the details in the format: name, email, age");
        continue;
      }

      string name = details[0].Trim();
      string quantityStr = details[1].Trim();
      string priceStr = details[2].Trim();

      if (!int.TryParse(quantityStr, out int quantity) || !decimal.TryParse(priceStr, out decimal price)) 
      {
        Console.WriteLine("C| Invalid quantity or price. Please enter a valid number for quantity and price.");
        break;
      }

      inventory.AddItem(new Item(name, quantity, price));
    }
  }

  public static void DeleteItem() 
  {
    Console.WriteLine("C| Search for Item: ");
    string name = Console.ReadLine();

    Item findItem = inventory.FindItem(name, false);

    inventory.RemoveItem(findItem);
  }
  
  public static void UpdateItem() 
  {
    string name;
    Item item = new Item();
  
    while (true) 
    {
      inventory.PrintInventory();
      Console.Write("C| Enter the name of the item you would like to modify: ");
      name = Console.ReadLine();

      item = inventory.FindItem(name, true);

      if (item is null) 
      {
        Console.WriteLine("Please try re-entering item name or searching for an item that exists.");
        continue;
      }

      break;
    }

    while (true) 
    {
      DisplayUpdateItemMenu(item);
      Console.Write("Enter your choice:");
      string choice = Console.ReadLine();

      if (choice == "4") break;

      if (choice != "1" || choice != "2" || choice != "3") continue;

      RouteUserInputForUpdateOptions(choice, item, name);
    }
  }

  public static void SearchItem() 
  {
    Console.WriteLine("Search for Item: ");
    string name = Console.ReadLine();

    Item findItem = inventory.FindItem(name, true);

    if (findItem is null) 
    {
      Console.WriteLine($"C| Item: {name} not found!");
      return;
    }

    Console.WriteLine($"C| Item Found! {findItem.ToString()}");
  }

  public static void DisplayUpdateItemMenu(Item item) 
  {
    Console.WriteLine($"C| Current item details: Name({item.Name}), Quantity({item.Quantity}), Price({item.Price})");
    Console.WriteLine("What would you like to modify?");
    Console.WriteLine("1. Quantity");
    Console.WriteLine("2. Price");
    Console.WriteLine("3. Both");
    Console.WriteLine("4. Exit");
  }

  public static void RouteUserInputForUpdateOptions(string choice, Item item, string name) 
  {
    string quantityStr;
    string priceStr;

    if (choice == "1" || choice == "3") 
    {
      Console.Write("Enter new quantity: ");
      quantityStr = Console.ReadLine();

      if (!int.TryParse(quantityStr, out int quantity)) 
      {
        Console.WriteLine("Invalid quantity. Please enter a valid number.");
        return;
      }

      item.Quantity = quantity;
      inventory.ModifyItem(inventory.FindItem(name, false), item);

      Console.WriteLine("C| Quantity updated successfully!");
    } 
    if (choice == "2" || choice == "3") 
    {
      Console.Write("Enter new price: ");
      priceStr = Console.ReadLine();

      if (!decimal.TryParse(priceStr, out decimal price)) 
      {
        Console.WriteLine("Invalid price. Please enter a valid number.");
      }

      item.Price = price;
      inventory.ModifyItem(inventory.FindItem(name, false), item);

      Console.WriteLine("C| Price updated successfully!");
    }
  }
}
