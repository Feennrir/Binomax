namespace BlazorApp.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public int StackSize { get; set; }
        public int MaxDurability { get; set; }
        public List<string> EnchantCategories { get; set; }
        public List<string> RepairWith { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ImageBase64 { get; set; }
		public int Quantity { get; set; }
		public bool InInventory { get; set; }
	}
}
