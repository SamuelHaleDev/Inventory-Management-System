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
    bool itemExists = !(Items.Find(tempItem => tempItem.Name == item.Name) == new Item());

    // Make sure Item does not exist (.Find returns default Item value if not found)
    if (itemExists) {
      Items.Add(item);
    } else {
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

  public Item FindItem(string name) {
    Item result = new Item();
    result = Items.Find(item => item.Name == name);

    if (result == new Item()) {
      Console.WriteLine("I| Item could not be found!");
    } else {
      Console.WriteLine("I| Item found!");
    }

    return result;
  }
}

class Program {
  static void Main(string[] args) {
   
  }
}
