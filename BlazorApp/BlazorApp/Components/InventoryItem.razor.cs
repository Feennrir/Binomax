using System.Diagnostics;
using BlazorApp.Model;
using BlazorApp.Pages;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components
{
	public partial class InventoryItem
	{
		[Parameter]
		public Item Item { get; set; }

		[Parameter]
		public bool NoDrop { get; set; }

		[Parameter]
		public int Index { get; set; }

		[Parameter]
		public bool listItem { get; set; }

		[CascadingParameter]
		public Inventory Parent { get; set; }


		internal void OnDragEnter()
		{
			if (NoDrop)
			{
				return;
			}

			Parent.Actions.Add(new InventoryAction { Action = "Drag Enter", Item = this.Item, Index = this.Index });
		}

		internal void OnDragLeave()
		{
			if (NoDrop)
			{
				return;
			}

			Parent.Actions.Add(new InventoryAction { Action = "Drag Leave", Item = this.Item, Index = this.Index });
		}

		internal void OnDrop()
		{
			if (NoDrop)
			{
				return;
			}
			if (Parent.CurrentDragItem == null)
			{
				return;
			}

			if (this.Item == null)
			{
				this.Item = new Item()
				{
					Id = Parent.CurrentDragItem.Id,
					DisplayName = Parent.CurrentDragItem.DisplayName,
					Name = Parent.CurrentDragItem.Name,
					StackSize = Parent.CurrentDragItem.StackSize,
					MaxDurability = Parent.CurrentDragItem.MaxDurability,
					EnchantCategories = Parent.CurrentDragItem.EnchantCategories,
					RepairWith = Parent.CurrentDragItem.RepairWith,
					CreatedDate = Parent.CurrentDragItem.CreatedDate,
					UpdatedDate = Parent.CurrentDragItem.UpdatedDate,
					ImageBase64 = Parent.CurrentDragItem.ImageBase64,
					Quantity = Parent.CurrentDragItem.Quantity,
					InInventory = true,
				};
				Parent.InventoryItems[this.Index] = this.Item;
			}
			else
			{
				if (this.Item.Quantity + Parent.CurrentDragItem.Quantity <= this.Item.StackSize && this.Item.Name == Parent.CurrentDragItem.Name)
				{
					this.Item.Quantity += Parent.CurrentDragItem.Quantity;
				}
				else if (this.Item.Name == Parent.CurrentDragItem.Name)
				{
					this.Item = new Item()
					{
						Id = Parent.CurrentDragItem.Id,
						DisplayName = Parent.CurrentDragItem.DisplayName,
						Name = Parent.CurrentDragItem.Name,
						StackSize = Parent.CurrentDragItem.StackSize,
						MaxDurability = Parent.CurrentDragItem.MaxDurability,
						EnchantCategories = Parent.CurrentDragItem.EnchantCategories,
						RepairWith = Parent.CurrentDragItem.RepairWith,
						CreatedDate = Parent.CurrentDragItem.CreatedDate,
						UpdatedDate = Parent.CurrentDragItem.UpdatedDate,
						ImageBase64 = Parent.CurrentDragItem.ImageBase64,
						Quantity = this.Item.Quantity + Parent.CurrentDragItem.Quantity - this.Item.StackSize,
						InInventory = true,
					};
					if (Parent.CurrentDragItem.InInventory)
					{
						if (Parent.InventoryItems[this.Index].Quantity + Parent.CurrentDragItem.Quantity > Parent.CurrentDragItem.StackSize)
						{
							Item itemp = Parent.InventoryItems[this.Index];
							itemp.Quantity = Parent.InventoryItems[this.Index].Quantity + Parent.CurrentDragItem.Quantity - Parent.CurrentDragItem.StackSize;
							Parent.InventoryItems[this.Index] = Parent.CurrentDragItem;
							Parent.InventoryItems[this.Index].Quantity = Parent.InventoryItems[this.Index].StackSize;
							Parent.InventoryItems[Parent.CurrentDragIndex] = itemp;

						}
						else
						{
							//Swap the place of the dragged item and the item that was dropped on it in the inventory list without changing the quantity
							Item itemp = Parent.InventoryItems[this.Index];
							Parent.InventoryItems[this.Index] = Parent.CurrentDragItem;
							Parent.InventoryItems[Parent.CurrentDragIndex] = itemp;
						}
					}
					else
					{
						bool boule = false;
                        for (int i = 0; i < Parent.InventoryItems.Count; i++)
                        {
							if (Parent.InventoryItems[i] != null)
							{
								if (Parent.InventoryItems[i].Name == this.Item.Name && Parent.InventoryItems[i].Quantity < Parent.InventoryItems[i].StackSize)
								{
									Parent.InventoryItems[i].Quantity = Parent.InventoryItems[i].Quantity + this.Item.Quantity;
									boule = true;
									break;
								}
							}
                        }
                        if (!boule)
						{
							for (int i = 0; i < Parent.InventoryItems.Count; i++)
							{
								if (Parent.InventoryItems[i] == null)
								{
									Parent.InventoryItems[i] = this.Item;
									break;
								}
							}
						}
					}


				}
				else
				{
					this.Item = new Item()
					{
						Id = Parent.CurrentDragItem.Id,
						DisplayName = Parent.CurrentDragItem.DisplayName,
						Name = Parent.CurrentDragItem.Name,
						StackSize = Parent.CurrentDragItem.StackSize,
						MaxDurability = Parent.CurrentDragItem.MaxDurability,
						EnchantCategories = Parent.CurrentDragItem.EnchantCategories,
						RepairWith = Parent.CurrentDragItem.RepairWith,
						CreatedDate = Parent.CurrentDragItem.CreatedDate,
						UpdatedDate = Parent.CurrentDragItem.UpdatedDate,
						ImageBase64 = Parent.CurrentDragItem.ImageBase64,
						Quantity = Parent.CurrentDragItem.Quantity,
						InInventory = true,
					};
					if (Parent.CurrentDragItem.InInventory)
					{
						//Swap the place of the dragged item and the item that was dropped on it in the inventory list
						Item itemp = Parent.InventoryItems[this.Index];
						Parent.InventoryItems[this.Index] = Parent.CurrentDragItem;
						Parent.InventoryItems[Parent.CurrentDragIndex] = itemp;
					}
					else
					{
						bool boule = false;
						for (int i = 0; i < Parent.InventoryItems.Count; i++)
						{
							if (Parent.InventoryItems[i] != null)
							{
								if (Parent.InventoryItems[i].Name == this.Item.Name && Parent.InventoryItems[i].Quantity < Parent.InventoryItems[i].StackSize)
								{
									Parent.InventoryItems[i].Quantity = Parent.InventoryItems[i].Quantity + this.Item.Quantity;
									boule = true;
									break;
								}
							}
						}
						if (!boule)
						{
							for (int i = 0; i < Parent.InventoryItems.Count; i++)
							{
								if (Parent.InventoryItems[i] == null)
								{
									Parent.InventoryItems[i] = this.Item;
									break;
								}
							}
						}
					}

				}
			}

			StateHasChanged();

			//this.Item = Parent.CurrentDragItem;

			Parent.Actions.Add(new InventoryAction { Action = "Drop", Item = this.Item, Index = this.Index });

			//Parent.CheckQuantity(this.Index);
		}

		private void OnDragStart()
		{
			Parent.CurrentDragIndex = this.Index;
			Parent.CurrentDragItem = this.Item;

			Parent.Actions.Add(new InventoryAction { Action = "Drag Start", Item = this.Item, Index = this.Index });

			if (this.Item == null)
			{
				return;
			}

			if (Parent.CurrentDragItem.InInventory)
			{
				Parent.InventoryItems[this.Index] = null;
			}


		}

		private void OnDragEnd()
		{
			Parent.Actions.Add(new InventoryAction { Action = "Drag End", Item = this.Item, Index = this.Index });
		}
	}

}
