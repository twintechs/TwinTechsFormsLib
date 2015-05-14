using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TwinTechs.Example
{
	public partial class ViewCellComplexSamplePage : ContentPage
	{
		public ViewCellComplexSamplePage ()
		{
			InitializeComponent ();
			MediaItemsListView.ItemsSource = DataProvider.GetMediaItems ();
		}


		void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MediaItem;
			DisplayAlert ("you selected an item", item.Name, "Ok");
		}
	}
}

