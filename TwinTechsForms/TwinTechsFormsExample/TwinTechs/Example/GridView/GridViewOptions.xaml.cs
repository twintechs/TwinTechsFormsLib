using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TwinTechs.Example.GridView
{
	public partial class GridViewOptions : ContentPage
	{
		public GridViewOptions ()
		{
			InitializeComponent ();
			MediaItemsGridView.ItemsSource = DataProvider.GetMediaItems ();
		}

		void OnToggleCentered (object sender, object e)
		{
			MediaItemsGridView.IsContentCentered = !MediaItemsGridView.IsContentCentered;
			CenteredMode.IsEnabled = MediaItemsGridView.IsContentCentered;
		}

		void OnToggleCenteredMode (object sender, object e)
		{
			MediaItemsGridView.CenterAsFilledRow = !MediaItemsGridView.CenterAsFilledRow;

		}

		void OnToggleHorizontal (object sender, object e)
		{
			MediaItemsGridView.IsHorizontal = !MediaItemsGridView.IsHorizontal;

		}
	}
}

