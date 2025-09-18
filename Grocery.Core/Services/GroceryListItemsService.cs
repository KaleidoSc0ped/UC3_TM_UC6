using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services;

public class GroceryListItemsService : IGroceryListItemsService
{
    private readonly IGroceryListItemsRepository _groceriesRepository;
    private readonly IProductRepository _productRepository;

    public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository,
        IProductRepository productRepository)
    {
        _groceriesRepository = groceriesRepository;
        _productRepository = productRepository;
    }

    public List<GroceryListItem> GetAll()
    {
        var groceryListItems = _groceriesRepository.GetAll();
        FillService(groceryListItems);
        return groceryListItems;
    }

    public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
    {
        var groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
        FillService(groceryListItems);
        return groceryListItems;
    }

    public GroceryListItem Add(GroceryListItem item)
    {
        return _groceriesRepository.Add(item);
    }

    public GroceryListItem? Delete(GroceryListItem item)
    {
        throw new NotImplementedException();
    }

    public GroceryListItem? Get(int id)
    {
        throw new NotImplementedException();
    }

    public GroceryListItem? Update(GroceryListItem item)
    {
        throw new NotImplementedException();
    }

    private void FillService(List<GroceryListItem> groceryListItems)
    {
        foreach (var g in groceryListItems) g.Product = _productRepository.Get(g.ProductId) ?? new Product(0, "", 0);
    }
}