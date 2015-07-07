using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace TwinTechs.Example.GridView
{
	public partial class GridViewOptions : ContentPage
	{

		public GridViewOptions ()
		{
			InitializeComponent ();
			MediaItemsGridView.ItemsSource = DataProvider.GetMediaItems ();
			OnToggleHorizontal (null, null);
		}

		void OnItemSelected (object sender, XLabs.GridEventArgs<object> e)
		{
			var item = e.Value as MediaItem;
			DisplayAlert ("you selected an item", item.Name, "Ok");
		}


		void OnToggleAutoPad1 (object sender, object e)
		{
			MediaItemsGridView.IsContentCentered = !MediaItemsGridView.IsContentCentered;
			CenteredMode.IsEnabled = MediaItemsGridView.IsContentCentered;

			if (MediaItemsGridView.IsContentCentered) {
				var items = DataProvider.GetMediaItems ();
				var newProvider = new ObservableCollection<MediaItem> ();
				newProvider.Add (items [0]);
				newProvider.Add (items [1]);
				MediaItemsGridView.ItemsSource = newProvider;
			} else {
				MediaItemsGridView.ItemsSource = DataProvider.GetMediaItems ();
			}

		}

		void OnToggleAutoPad2 (object sender, object e)
		{
			MediaItemsGridView.IsContentCentered = !MediaItemsGridView.IsContentCentered;
			CenteredMode.IsEnabled = MediaItemsGridView.IsContentCentered;
			MediaItemsGridView.ItemsSource = DataProvider.GetMediaItems ();

			if (!MediaItemsGridView.IsContentCentered) {
				MediaItemsGridView.ContentPaddingLeft = 20;
				MediaItemsGridView.ContentPaddingRight = 40;
				MediaItemsGridView.RowSpacing = 3;
				MediaItemsGridView.ColumnSpacing = 2;
			}

		}

		void OnToggleCenteredMode (object sender, object e)
		{
			MediaItemsGridView.CenterAsFilledRow = !MediaItemsGridView.CenterAsFilledRow;

		}

		void OnToggleHorizontal (object sender, object e)
		{
			MediaItemsGridView.IsHorizontal = !MediaItemsGridView.IsHorizontal;
		}

		void OnToggleMaxPerRow (object sender, object e)
		{
			MediaItemsGridView.MaxItemsPerRow = MediaItemsGridView.MaxItemsPerRow == -1 ? 2 : -1;
		}
	}
}

