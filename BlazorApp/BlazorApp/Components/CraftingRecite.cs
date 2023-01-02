using BlazorApp.Model;

namespace BlazorApp.Components
{
    public class CraftingRecipe
    {
        public Item Give { get; set; }
        public List<List<string>> Have { get; set; }
    }
}
