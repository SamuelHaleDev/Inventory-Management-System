namespace Inventory_Management_System;

// Item Class
class Item {
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Item(string name, int quantity, decimal price) {
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

}

class Program {
  static void Main(string[] args) {
    
  }
}
