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
			MediaItemsGridView.ItemsSource = DataProvider.GetMediaItems ();
		}

		void OnItemSelected (object sender, XLabs.EventArgs<object> e)
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

