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
    if (ItemExists(item)) 
    {
      Console.WriteLine($"I| Item exists!");
      return;
    }

    Items.Add(item);
    Console.WriteLine($"I| Item added! {item.ToString()}");
  }

  public void RemoveItem(Item item) 
  {
    if (!ItemExists(item)) 
    {
      Console.WriteLine($"I| Item does not exist!");
      return;
    }

    Items.Remove(item);
    Console.WriteLine($"I| Item removed! {item.ToString()}");
  }

  public void ModifyItem(Item originalItem, Item modifiedItem) 
  {
    if (!ItemExists(originalItem)) 
    {
      Console.WriteLine($"I| Item does not exist!");
    }

    Items[Items.IndexOf(originalItem)] = modifiedItem;
    Console.WriteLine($"I| Item modified! {modifiedItem.ToString()}");
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
    Item item = Items.Find(item => item.Name == name);

    if (item is null) 
    {
      Console.WriteLine("I| Item could not be found!");
      return null;
    } 

    Console.WriteLine($"Item found! {item.ToString()}");
    return clone ? new Item(item.Name, item.Quantity, item.Price) : item;
  }

  public bool ItemExists(Item item) 
  {
    return !(Items.Find(tempItem => tempItem.Name == item.Name) == new Item());
  }
}

class Program 
{
  public static Inventory inventory = new Inventory();
  public const string UPDATE_QUANTITY = "1";
  public const string UPDATE_PRICE = "2";
  public const string UPDATE_BOTH = "3";
  public const string QUIT = "4";
  public const char ADD = 'A';
  public const char DELETE = 'B';
  public const char VIEW = 'C';
  public const char UPDATE = 'D';
  public const char SEARCH = 'E';
  public const char EXIT = 'X';

  static void Main(string[] args) 
  {
    char[] USER_INPUT_OPTIONS = {ADD, DELETE, VIEW, UPDATE, SEARCH, EXIT};
    char input = 'A';
 
    while (Array.Exists(USER_INPUT_OPTIONS, OPTION => OPTION == input)) 
    {
      DisplayMenu();

      input = Char.ToUpper((char)Console.Read());
      var newLineCharacter = Console.ReadLine();

      if (input == EXIT) break;

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
        case ADD:
          AddItem();
          break;
        case DELETE:
          DeleteItem();
          break;
        case VIEW:
          inventory.PrintInventory();
          break;
        case UPDATE:
          UpdateItem();
          break;
        case SEARCH:
          SearchItem();
          break;
    }
  }

  public static void AddItem() 
  {
    while (true) 
    {
      const int NUMBER_OF_ATTRIBUTES = 3;

      string input = Input("Enter new item details (name, quantity, price): ");

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
    string name = Input("Search for Item: ");

    Item item = inventory.FindItem(name, false);

    inventory.RemoveItem(item);
  }
  
  public static void UpdateItem() 
  {
    while (true) 
    {
      inventory.PrintInventory();
      string name = Input("Enter the name of the item you would like to modify: ");
      Item item = inventory.FindItem(name, true);

      if (item is null) 
      {
        Console.WriteLine("Please try re-entering item name or searching for an item that exists.");
        continue;
      }

      DisplayUpdateItemMenu(item);

      string choice = Input("Enter your choice: ");

      if (choice == QUIT) break;

      if (choice != UPDATE_QUANTITY || choice != UPDATE_PRICE || choice != UPDATE_BOTH) continue;

      HandleUpdateItemChoice(choice, item, name);
    }
  }

  public static void SearchItem() 
  {
    string name = Input("Search for Item: ");

    Item item = inventory.FindItem(name, true);

    if (item is null) 
    {
      Console.WriteLine($"C| Item: {name} not found!");
      return;
    }

    Console.WriteLine($"C| Item Found! {item.ToString()}");
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

  public static void HandleUpdateItemChoice(string choice, Item item, string name) 
  {
    if (choice == UPDATE_QUANTITY || choice == UPDATE_BOTH) 
    {
      string quantityStr = Input("Enter new quantity: ");

      if (!int.TryParse(quantityStr, out int quantity)) 
      {
        Console.WriteLine("Invalid quantity. Please enter a valid number.");
        return;
      }

      item.Quantity = quantity;
      inventory.ModifyItem(inventory.FindItem(name, false), item);

      Console.WriteLine("C| Quantity updated successfully!");
    } 
    if (choice == UPDATE_PRICE || choice == UPDATE_BOTH) 
    {
      string priceStr = Input("Enter new price: ");

      if (!decimal.TryParse(priceStr, out decimal price)) 
      {
        Console.WriteLine("Invalid price. Please enter a valid number.");
      }

      item.Price = price;
      inventory.ModifyItem(inventory.FindItem(name, false), item);

      Console.WriteLine("C| Price updated successfully!");
    }
  }

  public static string Input(string message) 
  {
    Console.Write($"C| {message}");
    return Console.ReadLine();
  }
}