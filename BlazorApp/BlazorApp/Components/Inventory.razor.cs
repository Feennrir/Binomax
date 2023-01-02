using BlazorApp.Model;
using BlazorApp.Services;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BlazorApp.Pages;
using BlazorApp.Factories;
using static System.Net.WebRequestMethods;
using Blazored.LocalStorage;
using Blazored.Modal.Services;

namespace BlazorApp.Components
{
	public partial class Inventory
	{
		private Item _inventoryResult;

		[Inject]
		public IStringLocalizer<List> Localizer { get; set; }

		[Inject]
		public ILocalStorageService LocalStorage { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Inject]
		public HttpClient Http { get; set; }

		public Inventory()
		{
			Actions = new ObservableCollection<InventoryAction>();
			Actions.CollectionChanged += OnActionsCollectionChanged;
			this.InventoryItems = new List<Item> { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
		}

		public ObservableCollection<InventoryAction> Actions { get; set; }
		public Item CurrentDragItem { get; set; }

		public int CurrentDragIndex { get; set; }

		[Parameter]
		public List<Item> Items { get; set; }

		public List<Item> InventoryItems { get; set; }

		public Item InventoryResult
		{
			get => this._inventoryResult;
			set
			{
				if (this._inventoryResult == value)
				{
					return;
				}

				this._inventoryResult = value;
				this.StateHasChanged();
			}
		}

		[Parameter]
		public List<CraftingRecipe> Recipes { get; set; }

		[Inject]
		internal IJSRuntime JavaScriptRuntime { get; set; }

		private void OnActionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			JavaScriptRuntime.InvokeVoidAsync("Crafting.AddActions", e.NewItems);
			EnregistrementInventory();
		}

		private async void EnregistrementInventory()
        {
			List<FakeItem> originalData = new List<FakeItem>();
			for (int i = 0; i < InventoryItems.Count; i++)
			{
				if (InventoryItems[i] != null)
				{
					originalData.Add(new FakeItem() { Index = i, Name = InventoryItems[i].Name, Quantity = InventoryItems[i].Quantity });
				}
			}
			await LocalStorage.SetItemAsync("inv", originalData);
		}



		/// Liste Datagrid

		private List<Item> items;

		private int totalItem;

		string SearchValue;
		
		private List<Item> SearchItems;

		private List<Item> DataSource;


		[Inject]
		public IDataService DataService { get; set; }

		[Inject]
		public IWebHostEnvironment WebHostEnvironment { get; set; }



		[CascadingParameter]
		public IModalService Modal { get; set; }

		private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
		{
			if (e.CancellationToken.IsCancellationRequested)
			{
				return;
			}
			if (!String.IsNullOrEmpty(SearchValue))
			{
				SearchItems = DataSource.FindAll(e => e.Name.StartsWith(SearchValue));
				items = SearchItems.Skip((e.Page - 1) * e.PageSize).Take(e.PageSize).ToList();
				totalItem = SearchItems.Count;
			}
			else
			{
				DataSource = await DataService.List(e.Page, 336);
				items = await DataService.List(e.Page, e.PageSize);
				totalItem = await DataService.Count();
			}
		}
		private async void handleSubmit()
		{
			await OnReadData(new DataGridReadDataEventArgs<Item>(DataGridReadDataMode.Paging, null, null, 1, 10, 0, 0, new CancellationToken()));
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			// Do not treat this action if is not the first render
			if (!firstRender)
			{
				return;
			}

			var currentData = await LocalStorage.GetItemAsync<FakeItem[]>("inv");

			// Check if data exist in the local storage
			if (currentData != null)
			{
				for (int i = 0; i < currentData.Length; i++)
				{
					InventoryItems[currentData[i].Index] = new Item()
					{
						Name = currentData[i].Name,
						Quantity = currentData[i].Quantity,
						InInventory = true,
					};
					DataSource = await DataService.List(0, 336);
					var fileContent = await System.IO.File.ReadAllBytesAsync($"{WebHostEnvironment.WebRootPath}/images/default.png");
					ItemModel itemModel = ItemFactory.ToModel(DataSource[IndexOf(currentData[i].Name)], fileContent);
					ItemFactory.Update(InventoryItems[currentData[i].Index], itemModel);
				}
			}
		}

        private int IndexOf(string name)
        {	
            for(int i = 0; i < DataSource.Count ; i++)
            {
				if(DataSource[i].Name == name)
                {
					return i;
                }
            };
			return 0;
        }

        //Getter les items de l'inventaire quand on lance l'application
        //setter quand on deplace un item (quand on appelle stateHasChanged)
        //Quand on set on fait un foreach sur les items et on serialise les fauxItems (Nom, index, nombre)
        //Quand on get on fait un foreach sur les items serialiser et on recup les vrai items pour les foutre dans la liste.
    }
}
