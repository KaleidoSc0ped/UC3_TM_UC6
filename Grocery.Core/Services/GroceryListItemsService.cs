using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
    {
        public class GroceryListItemsService : IGroceryListItemsService
        {
            private readonly IGroceryListItemsRepository _groceriesRepository;
            private readonly IProductRepository _productRepository;

            public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
            {
                _groceriesRepository = groceriesRepository;
                _productRepository = productRepository;
            }

            public List<GroceryListItem> GetAll()
            {
                List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
                FillService(groceryListItems);
                return groceryListItems;
            }

            public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
            {
                List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll()
                    .Where(g => g.GroceryListId == groceryListId)
                    .ToList();

                FillService(groceryListItems);
                return groceryListItems;
            }

            public GroceryListItem Add(GroceryListItem item)
            {
                return _groceriesRepository.Add(item);
            }

        public GroceryListItem AddProduct(int groceryListId, Product product)
        {
            var newItem = new GroceryListItem(
                id: 0,                   
                groceryListId: groceryListId,
                productId: product.Id,
                amount: 1          
            );

            return _groceriesRepository.Add(newItem);
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
            public List<Product> GetAvailableProducts(int groceryListId)
            {
               
                var allProducts = _productRepository.GetAll();

                var groceryListItems = _groceriesRepository.GetAll()
                    .Where(g => g.GroceryListId == groceryListId)
                    .ToList();

                var productIdsOnList = groceryListItems.Select(g => g.ProductId).ToHashSet();

                var availableProducts = allProducts
                    .Where(p => p.Stock > 0 && !productIdsOnList.Contains(p.Id))
                    .ToList();

                return availableProducts;
            }

            private void FillService(List<GroceryListItem> groceryListItems)
            {
                foreach (GroceryListItem g in groceryListItems)
                {
                    g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
                }
            }
        }
    }
