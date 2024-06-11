using System.Runtime.CompilerServices;
using System.Xml.XPath;
using Microsoft.VisualBasic;

namespace Inventory_Management_System;

// Item Class
class Item {
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Item(string name = "", int quantity = 0, decimal price = 0m) {
      Name = name;
      Quantity = quantity;
      Price = price;
    }

    public override string ToString()  {
        return $"Name: {Name}, Quantity {Quantity}, Price: {Price:C}";
    }
}

// Inventory Class
class Inventory {
  public List<Item> Items;

  public Inventory() {
    Items = new List<Item>();
  }

  // Add Item
  public void AddItem(Item item) {
    bool itemExists = (Items.Find(tempItem => tempItem.Name == item.Name) == new Item());

    // Make sure Item does not exist (.Find returns default Item value if not found)
    if (!itemExists) {
      Items.Add(item);
      Console.WriteLine($"I| Item added! {item.ToString()}");
    } else {
      Console.WriteLine($"I| Item exists!");
      return; 
    }
  }

  // Remove Item
  public void RemoveItem(Item item) {
    bool itemExists = !(Items.Find(tempItem => tempItem.Name == item.Name) == new Item());

    // Make sure item exists before removing
    if (itemExists) {
      Items.Remove(item);
    } else {
      return;
    }
    
  }

  // Modify Item
  public void ModifyItem(Item originalItem, Item modifiedItem) {
    bool itemExists = !(Items.Find(tempItem => tempItem.Name == originalItem.Name) == new Item());

    // Make sure item exists before modifying
    if (itemExists) {
      Items[Items.IndexOf(originalItem)] = modifiedItem;
    } else {
      return;
    }
   
  }

  // View Inventory
  public void PrintInventory() {
    Console.WriteLine("####################### Printing Inventory #######################");
    for (int i = 0; i < Items.Count(); i++) {
      Console.WriteLine($"I| Item {i+1}: {Items[i].ToString()}");
    }
  }

  public Item FindItem(string name, bool clone) {
    Item foundItem;
    Item result;
    foundItem = Items.Find(item => item.Name == name);

    if (foundItem == null) {
      Console.WriteLine("I| Item could not be found!");
    } else {
      Console.WriteLine("I| Item found!");
    }
    if (clone) {
      result = new Item(foundItem.Name, foundItem.Quantity, foundItem.Price);
    } else {
      result = foundItem;
    }

    // return null if result is null, return a clone if its not null
    return result;
  }
}

class Program {
  public static Inventory inventory = new Inventory();

  static void Main(string[] args) {
    char input = 'A';
    char[] inputs = {'A', 'B', 'C', 'D', 'E', 'X'};
 
    while (Array.Exists(inputs, element => element == input)) {
      // Show user options and get input
      Menu();
      input = Char.ToUpper((char)Console.Read());
      var newLineCharacter = Console.ReadLine();

      if (input == 'X') break;

      // Perform operation
      switch (input) {
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
  }

  static void Menu() {
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

  public static void AddItem() {
    string input = "";
    while (true) {
      // Get item details
      Console.Write("C| Enter new item details (name, quantity, price): ");
      input = Console.ReadLine();
      var details = input.Split(',');

      if (details.Length == 3) {
        string name = details[0].Trim();
        string quantityStr = details[1].Trim();
        string priceStr = details[2].Trim();

        if (int.TryParse(quantityStr, out int quantity) && decimal.TryParse(priceStr, out decimal price)) {
          inventory.AddItem(new Item(name, quantity, price));
          break;
        } else {
          Console.WriteLine("C| Invalid quantity or price. Please enter a valid number for quantity and price.");
        }
      } else {
        Console.WriteLine("C| Invalid input. Please enter the details in the format: name, email, age");
      }
    }
  }

  public static void DeleteItem() {
    // Get name of item to search for
    Console.WriteLine("C| Search for Item: ");
    string name = Console.ReadLine();

    // Find item
    Item findItem = inventory.FindItem(name, false);

    // Delete item
    inventory.RemoveItem(findItem);
  }
  
  public static void UpdateItem() {
    bool found = false;
    string name;
    Item item = new Item();
  
    // Find item first
    inventory.PrintInventory();
    while (true) {
      Console.Write("C| Enter the name of the item you would like to modify: ");
      name = Console.ReadLine();

      // Search for item
      item = inventory.FindItem(name, true);

      // Item found, break loop
      if (item != null) {
        break;
      } else {
        Console.WriteLine("Please try re-entering item name or searching for an item that exists.");
      }
    }

    // Enter modification menu
    while (true) {
      Console.WriteLine($"C| Current item details: Name({item.Name}), Quantity({item.Quantity}), Price({item.Price})");
      Console.WriteLine("What would you like to modify?");
      Console.WriteLine("1. Quantity");
      Console.WriteLine("2. Price");
      Console.WriteLine("3. Both");
      Console.WriteLine("4. Exit");
      Console.Write("Enter your choice:");
      string choice = Console.ReadLine();

      // User wants to exit so break
      if (choice == "4") break;

      // Handle main options
      string quantityStr;
      string priceStr;
      switch (choice) {
        case "1":
          // Get quantity
          Console.Write("Enter new quantity: ");
          quantityStr = Console.ReadLine();

          // If it converts update Item, otherwise alert user
          if (int.TryParse(quantityStr, out int quantity)) {
            item.Quantity = quantity;
            inventory.ModifyItem(inventory.FindItem(name, false), item);

            Console.WriteLine("C| Quantity updated successfully!");
          } else {
            Console.WriteLine("Invalid quantity. Please enter a valid number.");
          }
          break;
        case "2":
          // Get price
          Console.Write("Enter new price: ");
          priceStr = Console.ReadLine();

          // If it converts update Item, otherwise alert user
          if (decimal.TryParse(priceStr, out decimal price)) {
            item.Price = price;
            inventory.ModifyItem(inventory.FindItem(name, false), item);

            Console.WriteLine("C| Price updated successfully!");
          } else {
            Console.WriteLine("Invalid price. Please enter a valid number.");
          }
          break;
        case "3":
          // Get quantity and price from user
          Console.Write("Enter new quantity: ");
          quantityStr = Console.ReadLine();

          Console.Write("Enter new price: ");
          priceStr = Console.ReadLine();

          // If the values convert update the item, otherwise alert user
          if (int.TryParse(quantityStr, out int quantity1) && decimal.TryParse(priceStr, out decimal price1)) {
            item.Quantity = quantity1;
            item.Price = price1;
            inventory.ModifyItem(inventory.FindItem(name, false), item);

            Console.WriteLine("Price and Quantity updated successfully!");
          } else {
            Console.WriteLine("Invalid quantity or price. Please enter a valid number.");
          }

          break;
        default:
          Console.WriteLine("Please enter a valid input.");
          break;

      }
    }
  }

  public static void SearchItem() {
    // Get name of item to search for
    Console.WriteLine("Search for Item: ");
    string name = Console.ReadLine();

    // Find item
    Item findItem = inventory.FindItem(name, true);

    // If it exists display it
    if (name != null) {
      Console.WriteLine($"C| Item Found! {findItem.ToString()}");
    } else {
      Console.WriteLine($"C| Item: {name} not found!");
    }
  }
}
