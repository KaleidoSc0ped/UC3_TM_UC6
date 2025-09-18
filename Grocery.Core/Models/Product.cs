namespace Grocery.Core.Models;

public class Product : Model
{
    public Product(int id, string name, int stock) : base(id, name)
    {
        Stock = stock;
    }

    public int Stock { get; set; }
}