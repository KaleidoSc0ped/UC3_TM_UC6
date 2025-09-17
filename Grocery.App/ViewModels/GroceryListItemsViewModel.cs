using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(GroceryList), nameof(GroceryList))]
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;
        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        [ObservableProperty]
        GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        public GroceryListItemsViewModel(IGroceryListItemsService groceryListItemsService, IProductService productService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;
            Load(groceryList.Id);
        }

        private void Load(int id)
        {
            MyGroceryListItems.Clear();
            foreach (var item in _groceryListItemsService.GetAllOnGroceryListId(id)) MyGroceryListItems.Add(item);
            GetAvailableProducts();
        }

        private void GetAvailableProducts()
        {
            // Maakt de lijst met beschikbare producten leeg.
            AvailableProducts.Clear();

            // Loopt door alle producten waarvan de voorraad groter is dan 0.
            foreach (var product in _productService.GetAll().Where(p => p.Stock > 0))
            {
                // Controleerd of het product nog niet op de boodschappenlijst staat.
                if (!MyGroceryListItems.Any(gli => gli.ProductId == product.Id))
                {
                    // Voegt het product toe aan de lijst met beschikbare producten.
                    AvailableProducts.Add(product);
                }
            }
        }

        partial void OnGroceryListChanged(GroceryList value) => Load(value.Id);

        [RelayCommand]
        public async Task ChangeColor()
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), GroceryList } };
            await Shell.Current.GoToAsync($"{nameof(ChangeColorView)}?Name={GroceryList.Name}", true, paramater);
        }
        [RelayCommand]
        public void AddProduct(Product product)
        {
            // Controleerd of het product geldig is (niet null en Id > 0).
            if (product == null || product.Id <= 0) return;

            // Maakt een nieuw GroceryListItem aan (Id 0 = nieuw item) en koppelt het aan huidige GroceryList en Product.
            GroceryListItem groceryListItem = new GroceryListItem(0, GroceryList.Id, product.Id, 1);

            // Voegt het nieuwe item toe aan de dataset via de service.
            _groceryListItemsService.Add(groceryListItem);

            // Voegt het nieuwe product toe aan de lokale collectie.
            MyGroceryListItems.Add(groceryListItem);

            // Verminderd de voorraad van het product met 1 en werk dit bij via de ProductService.
            _productService.Update(new Product(product.Id, product.Name, product.Stock - 1));

            // Verwijderd het product uit de lijst met beschikbare producten.
            AvailableProducts.Remove(product);

            // Herlaad de gegevens om alles up-to-date te krijgen...
            OnGroceryListChanged(GroceryList);
        }
    }
}
