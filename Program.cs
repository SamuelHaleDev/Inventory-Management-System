namespace Inventory_Management_System;

// Item Class
class Item {
    private string name;
    private int quantity;
    private float price;

    public Item(string n, int q, float p) {
      name = n;
      quantity = q;
      price = p;
    }

    public string GetName() {
      return name;
    }

    public int GetQuantity() {
      return quantity;
    }

    public float GetPrice() {
      return price;
    }

    public void SetName(string n) {
      name = n;
    }

    public void SetQuantity(int q) {
      quantity = q;
    }

    public void SetPrice(float p) {
      price = p;
    }
}

// Inventory Class


class Program {
  static void Main(string[] args) {
    
  }
}
