using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Example;
using System.Collections.ObjectModel;

namespace TwinTechs.Example.GridView
{
	public partial class GridViewPerformance2 : ContentPage
	{
		public GridViewPerformance2 ()
		{
			InitializeComponent ();
			var outerRandomNumberGenerator = new Random ();
			var innerRandomNumberGenererator = new Random ();
			var allItems = DataProvider.GetMediaItems ();
			var targetItems = new ObservableCollection<ObservableCollection<MediaItem>> ();
			for (int i = 0; i < 50; i++) {
				var allInnerItems = new ObservableCollection<MediaItem> (allItems);
				var targetInnerItems = new ObservableCollection<MediaItem> ();
				var numberToUse = outerRandomNumberGenerator.Next (1, 20);
				for (int y = 0; y < numberToUse; y++) {
					var index = innerRandomNumberGenererator.Next (0, allInnerItems.Count - 1);
					var item = allInnerItems [index];
					allInnerItems.RemoveAt (index);
					targetInnerItems.Add (item);
				}
				targetItems.Add (targetInnerItems);
			}
			MediaItemsGridView.ItemsSource = targetItems;

		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
			MediaItemsGridView.ItemSize = new Size (width, MediaItemsGridView.ItemSize.Height);
		}


	}
}

