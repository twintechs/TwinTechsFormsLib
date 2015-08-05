using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;
using TwinTechs.Example;

namespace TwinTechs.Example.Gestures
{
	public partial class YoutubeStyleContentPage : ContentPage
	{
		public double ParentHeight { get; set; }


		public YoutubeStyleContentPage ()
		{
			InitializeComponent ();
			MinimumHeightRequest = 100;
			MinimumWidthRequest = 160;
			MediaItemsListView.ItemsSource = DataProvider.GetMediaItems ();
		}


		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			var complete = Math.Min (1, height / (ParentHeight));
			MediaItemsListView.Opacity = complete;
			TitleLabel.Opacity = complete;
			MainLayout.RowDefinitions [0].Height = 40 * complete;
			MainLayout.RowDefinitions [1].Height = MinimumHeightRequest + (160 * complete);
//			Debug.WriteLine ("complete {0} height {1} ph{2}", complete, height, ParentHeight);
			base.LayoutChildren (x, y, width, height);
			MediaItemsListView.Layout (new Rectangle (0, MediaItemsListView.Bounds.Y, width, MediaItemsListView.Bounds.Height));
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			base.OnSizeAllocated (width, height);

		}

		public View VideoPlayerView { get { return VideoPlayer; } }

		public MediaItem Item { get; set; }

	}
}
