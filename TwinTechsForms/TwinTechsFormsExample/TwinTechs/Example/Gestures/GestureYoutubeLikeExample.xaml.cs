using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;

namespace TwinTechs.Example.Gestures
{
	public partial class GestureYoutubeLikeExample : ContentPage
	{
		Rectangle _contentBounds = new Rectangle (100, 200, 150, 150);
		YoutubeStyleContentPage _contentPage;

		PanGestureRecognizer _panGesture;
		bool _didLayoutContainer;

		public GestureYoutubeLikeExample ()
		{
			InitializeComponent ();
			MainLayout.OnLayoutChildren += MainLayout_OnLayoutChildren;
			MediaItemsListView.ItemsSource = DataProvider.GetMediaItems ();
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
			if (_contentPage != null) {
				_contentPage.ParentHeight = height;
			}
		}

		void MainLayout_OnLayoutChildren (double x, double y, double width, double height)
		{
			MediaItemsListView.Layout (new Rectangle (0, 0, Width, Height));
			if (!_didLayoutContainer) {
				_contentBounds.Y = height - 100;
				_contentBounds.X = width - 160;
				_contentBounds.Width = 160;
				_contentBounds.Height = 100;
				_didLayoutContainer = true;
			}
			PageContainer.Layout (_contentBounds);
		}

		Rectangle _startBounds;

		void Gesture_OnAction (BaseGestureRecognizer recgonizer, GestureRecognizerState state)
		{
			var panGesture = recgonizer as PanGestureRecognizer;
			Point translation = panGesture.GetTranslationInView (MainLayout);
			Point velocity = panGesture.GetVelocityInView (MainLayout);
			panGesture.SetTranslationInView (new Point (0, 0), MainLayout);
			switch (panGesture.State) {
			case GestureRecognizerState.Began:
				break;
			case GestureRecognizerState.Changed:
				if (recgonizer.View == _contentPage.VideoPlayerView) {
					_contentBounds.Y += translation.Y;
					var complete = Math.Min (1, (Height - _contentBounds.Y) / Height);
					var inverseCompletion = 1 - complete;
					_contentBounds.X = (Width - 160) * inverseCompletion;
					_contentBounds.Width = (160) + ((Width - 160) * complete);
					_contentBounds.Height = Math.Max (100, Height * complete);
					Debug.WriteLine ("bounds {0} comp {1} inComp {2}", _contentBounds, complete, inverseCompletion);
					PageContainer.Layout (_contentBounds);
				}
				break;
			case GestureRecognizerState.Cancelled:
			case GestureRecognizerState.Ended:
			case GestureRecognizerState.Failed:
				var isShowing = _contentBounds.Y < 200;
				ToggleShowing (isShowing, true);
				break;
			default:
				break;
			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
		}

		void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			
			ToggleShowing (true, true);
		}



		void ToggleShowing (bool isShowing, bool animated)
		{
			_contentBounds.Y = isShowing ? 0 : Height - 100;
			_contentBounds.X = isShowing ? 0 : Width - 160;
			_contentBounds.Width = isShowing ? Width : 160;
			_contentBounds.Height = isShowing ? Height : 100;
			if (_contentPage == null) {
				_contentPage = new YoutubeStyleContentPage ();
				_contentPage.ParentHeight = Height;
				PageContainer.Content = _contentPage;
				_panGesture = new PanGestureRecognizer ();
				_panGesture.OnAction += Gesture_OnAction;
				_panGesture.IsConsumingTouchesInParallel = true;
				_contentPage.VideoPlayerView.AddGestureRecognizer (_panGesture);
			}
			if (MediaItemsListView.SelectedItem != null) {
				_contentPage.Item = MediaItemsListView.SelectedItem as MediaItem;

			}
			if (animated) {
				PageContainer.LayoutTo (_contentBounds);
			} else {
				PageContainer.Layout (_contentBounds);
			}

		}
	}
}

