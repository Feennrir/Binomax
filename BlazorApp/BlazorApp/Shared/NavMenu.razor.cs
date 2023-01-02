using BlazorApp.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Shared
{
	public partial class NavMenu
	{
		[Inject]
		public IStringLocalizer<List> Localizer { get; set; }
	}
}
