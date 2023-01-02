using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace BlazorApp.Pages
{
	public partial class Counter
	{
		[Inject]
		public IStringLocalizer<List> Localizer { get; set; }
	}
}
