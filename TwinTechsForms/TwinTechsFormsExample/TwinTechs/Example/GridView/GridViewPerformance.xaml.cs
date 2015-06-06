using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Example;
using System.Collections.ObjectModel;

namespace TwinTechs.Example.GridView
{
	public partial class GridViewPerformance : ContentPage
	{
		public GridViewPerformance ()
		{
			InitializeComponent ();
			if (AppHelper.ScreenSize.Width > 320 && AppHelper.ScreenSize.Height > 320) {
				MediaItemsGridView.ItemWidth = 300;
				MediaItemsGridView.ItemHeight = 320;
			}
			MediaItemsGridView.ItemsSource = DataProvider.GetMediaItems ();
		}

		void OnItemSelected (object sender, XLabs.GridEventArgs<object> e)
		{
			var item = e.Value as MediaItem;
			DisplayAlert ("you selected an item", item.Name, "Ok");
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
			MediaItemsGridView.Layout (new Rectangle (0, 0, width, height));
		}
	}
}

