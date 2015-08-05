using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;

namespace TwinTechs.Example.Gestures
{
	public partial class SwipeyListExample : ContentPage
	{
		Rectangle _boxBounds = new Rectangle (200, 400, 200, 200);
		Rectangle _box2Bounds = new Rectangle (100, 200, 150, 150);
		BoxView Box;
		BoxView Box2;

		public SwipeyListExample ()
		{
			InitializeComponent ();
			MediaItemsListView.ItemsSource = DataProvider.GetMediaItems ();
			MediaItemsListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
				if (e.SelectedItem == null)
					return; // don't do anything if we just de-selected the row
				((ListView)sender).SelectedItem = null; 
			};
		}


		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			//TODO get rid of all gestures!
		}
	}
}

