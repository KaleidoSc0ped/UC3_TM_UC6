using System.Collections.ObjectModel;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels;

public class ProductViewModel : BaseViewModel
{
    public ProductViewModel(IProductService productService)
    {
        Products = new ObservableCollection<Product>(productService.GetAll());
    }

    public ObservableCollection<Product> Products { get; set; }
}