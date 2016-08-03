using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Controls;

namespace TwinTechs.Example.FastCells
{
	public partial class FastCellComplexSVGSamplePage : ContentPage
	{
		public FastCellComplexSVGSamplePage ()
		{
			InitializeComponent ();
			MediaItemsListView.ItemsSource = DataProvider.GetMediaItems ();
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

