using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TwinTechs.Example.FastCells
{
	public partial class FastSimpleCellUnevenHeight : ContentPage
	{
		public FastSimpleCellUnevenHeight ()
		{
			InitializeComponent ();
			var items = DataProvider.GetMediaItems ();
			var unevenHeightItems = new List<UnevenSizedMediaItem> ();
			var random = new Random ();
			foreach (var item in items) {
				var unevenItem = new UnevenSizedMediaItem (item.Name, item.ImageName, item.UserName, item.Description, item.ImagePath, item.ThumbnailImagePath, item.Index);
				unevenItem.RowHeight = random.Next (40, 200);
				unevenHeightItems.Add (unevenItem);
			}
			MediaItemsListView.ItemsSource = unevenHeightItems;
		}

		void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MediaItem;
			DisplayAlert ("you selected an item", item.Name, "Ok");
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			AppHelper.FastCellCache.FlushAllCaches ();
		}

	}
}

