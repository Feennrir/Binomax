using BlazorApp.Components;
using BlazorApp.Model;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Pages
{
	public partial class InventoryPage
	{
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			base.OnAfterRenderAsync(firstRender);
			StateHasChanged();

			if (firstRender)
			{
				Items = await DataService.List(0, await DataService.Count());
			}
		}

		[Inject]
		public IDataService DataService { get; set; }
		public List<Item> Items { get; set; } = new List<Item>();



		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
		}
	}
}
