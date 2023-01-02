using BlazorApp.Components;
using BlazorApp.Model;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
    public partial class Index
    {
        public List<Cake> Cakes { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRenderAsync(firstRender);

            LoadCakes();
            StateHasChanged();

            if (firstRender)
            {
                Items = await DataService.List(0, await DataService.Count());
                Recipes = await DataService.GetRecipes();
            }
        }

        public void LoadCakes()
        {


            Cakes = new List<Cake>
        {
            // items hidden for display purpose
            new Cake
            {
                Id = 1,
                Name = "Red Velvet",
                Cost = 60
            },
            new Cake
            {
                Id = 2,
                Name = "NoWay",
                Cost= 60
            },
        };
        }

        [Inject]
        public IDataService DataService { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        private List<CraftingRecipe> Recipes { get; set; } = new List<CraftingRecipe>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //Items = await DataService.List(0, await DataService.Count());
            //Recipes = await DataService.GetRecipes();
        }
    }
}
