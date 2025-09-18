using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels;

public partial class GroceryListViewModel : BaseViewModel
{
    private readonly IGroceryListService _groceryListService;

    public GroceryListViewModel(IGroceryListService groceryListService)
    {
        Title = "Boodschappenlijst";
        _groceryListService = groceryListService;
        GroceryLists = new ObservableCollection<GroceryList>(_groceryListService.GetAll());
    }

    public ObservableCollection<GroceryList> GroceryLists { get; set; }

    [RelayCommand]
    public async Task SelectGroceryList(GroceryList groceryList)
    {
        Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
        await Shell.Current.GoToAsync($"{nameof(GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
    }

    public override void OnAppearing()
    {
        base.OnAppearing();
        GroceryLists = new ObservableCollection<GroceryList>(_groceryListService.GetAll());
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        GroceryLists.Clear();
    }
}
