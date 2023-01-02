using BlazorApp.Components;
using BlazorApp.Factories;
using BlazorApp.Model;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Services
{
    public class DataLocalService : IDataService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DataLocalService(
            ILocalStorageService localStorage,
            HttpClient http,
            IWebHostEnvironment webHostEnvironment,
            NavigationManager navigationManager)
        {
            _localStorage = localStorage;
            _http = http;
            _webHostEnvironment = webHostEnvironment;
            _navigationManager = navigationManager;
        }

        public async Task Add(ItemModel model)
        {
            // Get the current data
            var currentData = await _localStorage.GetItemAsync<List<Item>>("data");

            // Simulate the Id
            model.Id = currentData.Max(s => s.Id) + 1;

            // Add the item to the current data
            currentData.Add(ItemFactory.Create(model));

            // Add the item to the current data
            currentData.Add(new Item
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                Name = model.Name,
                RepairWith = model.RepairWith,
                EnchantCategories = model.EnchantCategories,
                MaxDurability = model.MaxDurability,
                StackSize = model.StackSize,
                CreatedDate = DateTime.Now
            });

            
            // Save the data
            await _localStorage.SetItemAsync("data", currentData);
        }

        public async Task<int> Count()
        {
            return (await _localStorage.GetItemAsync<Item[]>("data")).Length;
        }

        public async Task<Item> GetById(int id)
        {
            // Get the current data
            var currentData = await _localStorage.GetItemAsync<List<Item>>("data");

            // Get the item int the list
            var item = currentData.FirstOrDefault(w => w.Id == id);

            // Check if item exist
            if (item == null)
            {
                throw new Exception($"Unable to found the item with ID: {id}");
            }

            return item;
        }

        public async Task<List<Item>> List(int currentPage, int pageSize)
        {
            // Load data from the local storage
            var currentData = await _localStorage.GetItemAsync<Item[]>("data");

            // Check if data exist in the local storage
            if (currentData == null)
            {
                // this code add in the local storage the fake data
                var originalData = await _http.GetFromJsonAsync<Item[]>($"{_navigationManager.BaseUri}fake-data.json");
                await _localStorage.SetItemAsync("data", originalData);
            }

            return (await _localStorage.GetItemAsync<Item[]>("data")).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task Update(int id, ItemModel model)
        {
            // Get the current data
            var currentData = await _localStorage.GetItemAsync<List<Item>>("data");

            // Get the item int the list
            var item = currentData.FirstOrDefault(w => w.Id == id);

            // Check if item exist
            if (item == null)
            {
                throw new Exception($"Unable to found the item with ID: {id}");
            }


            // Modify the content of the item
            ItemFactory.Update(item, model);

            // Modify the content of the item
            item.DisplayName = model.DisplayName;
            item.Name = model.Name;
            item.RepairWith = model.RepairWith;
            item.EnchantCategories = model.EnchantCategories;
            item.MaxDurability = model.MaxDurability;
            item.StackSize = model.StackSize;
            item.UpdatedDate = DateTime.Now;

            // Save the data
            await _localStorage.SetItemAsync("data", currentData);
        }

        public async Task Delete(int id)
        {
            // Get the current data
            var currentData = await _localStorage.GetItemAsync<List<Item>>("data");

            // Get the item int the list
            var item = currentData.FirstOrDefault(w => w.Id == id);

            // Delete item in
            currentData.Remove(item);


            // Save the data
            await _localStorage.SetItemAsync("data", currentData);
        }

        public Task<List<CraftingRecipe>> GetRecipes()
        {
            var items = new List<CraftingRecipe>
        {
            new CraftingRecipe
            {
                Give = new Item { DisplayName = "Diamond", Name = "diamond" },
                Have = new List<List<string>>
                {
                    new List<string> { "dirt", "dirt", "dirt" },
                    new List<string> { "dirt", null, "dirt" },
                    new List<string> { "dirt", "dirt", "dirt" }
                }
            }
        };

            return Task.FromResult(items);
        }
    }
}
