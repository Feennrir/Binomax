using BlazorApp.Modals;
using BlazorApp.Model;
using BlazorApp.Services;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Pages
{
    public partial class List
    {
        [Inject]
        public IStringLocalizer<List> Localizer { get; set; }

        private List<Item> items;

        private int totalItem;

        [Inject]
        public IDataService DataService { get; set; }

        [Inject]
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        private async Task OnReadData(DataGridReadDataEventArgs<Item> e)
        {
            if (e.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (!e.CancellationToken.IsCancellationRequested)
            {
                items = await DataService.List(e.Page, e.PageSize);
                totalItem = await DataService.Count();
            }
        }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }
        private async void OnDeleteAsync(int id)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(Item.Id), id);

            var modal = Modal.Show<DeleteConfirmation>("Delete Confirmation", parameters);
            var result = await modal.Result;

            if (result.Cancelled)
            {
                return;
            }

            await DataService.Delete(id);

            // Reload the page
            NavigationManager.NavigateTo("list", true);
        }
    }
}
