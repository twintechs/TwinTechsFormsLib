using System;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures
{
	public partial class GestureScrollListExample : ContentPage
	{
		double _currentGridY = 400;
		PanGestureRecognizer _panGesture;
		bool _didLayoutContainer;

		float _scrollPosition;

		public GestureScrollListExample ()
		{
			InitializeComponent ();
			MainLayout.OnLayoutChildren += MainLayout_OnLayoutChildren;
			MediaItemsListView.ItemsSource = DataProvider.GetMediaItems ();

			_panGesture = new PanGestureRecognizer ();
			_panGesture.OnAction += Gesture_OnAction;
			_panGesture.IsConsumingTouchesInParallel = true;
//			_panGesture.CancelsTouchesInView = true;
//			_panGesture.DelaysTouches = true;
//			_panGesture.OnGestureShouldBeginDelegate += OnGestureShouldBegin;
			MediaItemsListView.AddGestureRecognizer (_panGesture);
			MediaItemsListView.OnScroll += MediaItemsListView_OnScroll;

		}

		void MediaItemsListView_OnScroll (object sender, TwinTechs.Controls.ControlScrollEventArgs e)
		{
			_scrollPosition = e.CurrentY;
		}

		void MainLayout_OnLayoutChildren (double x, double y, double width, double height)
		{
			MediaItemsListView.Layout (new Rectangle (0, _currentGridY, Width, Height));
			Details.Layout (new Rectangle (0, 0, Width, 400));
		}

		Rectangle _startBounds;

		void Gesture_OnAction (BaseGestureRecognizer recgonizer, GestureRecognizerState state)
		{
			if (recgonizer.View != MediaItemsListView) {
				return;
			}
			var panGesture = recgonizer as PanGestureRecognizer;
			Point translation = panGesture.GetTranslationInView (MainLayout);
			Point velocity = panGesture.GetVelocityInView (MainLayout);
			panGesture.SetTranslationInView (new Point (0, 0), MainLayout);
			switch (panGesture.State) {
			case GestureRecognizerState.Began:
//				MediaItemsListView.IsEnabled = false;
				break;
			case GestureRecognizerState.Changed:
				var newY = _currentGridY + translation.Y;
				newY = Math.Max (0, newY);
				newY = Math.Min (400, newY);
				Debug.WriteLine ("newY {0} _currentGridY {1} _scrollPosition{2}", newY, _currentGridY, _scrollPosition);
				bool willMoveList = false;
				if (newY != _currentGridY) {
					willMoveList = _currentGridY > 0 || _scrollPosition <= 0 &&
					newY > 0 && newY < Height - 100;
					
					if (willMoveList) {
						_currentGridY = newY;
						var bounds = MediaItemsListView.Bounds;
						bounds.Y = _currentGridY;
						MediaItemsListView.Layout (bounds);
					}
					MediaItemsListView.IsScrollEnabled = !willMoveList;
				} else {
					MediaItemsListView.IsScrollEnabled = true;
					Debug.WriteLine ("no move " + MediaItemsListView.IsScrollEnabled);
				}
				break;
			case GestureRecognizerState.Cancelled:
			case GestureRecognizerState.Ended:
			case GestureRecognizerState.Failed:
				MediaItemsListView.IsScrollEnabled = true;
				if (_currentGridY != 0 && _currentGridY != 400) {
					_currentGridY = _currentGridY > 200 ? 400 : 0;
					MediaItemsListView.LayoutTo (new Rectangle (0, _currentGridY, Width, Height));
				}
				break;
			default:
				break;
			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			if (MediaItemsListView != null) {
				MediaItemsListView.RemoveAllGestureRecognizers ();
			}
		}


		void OnItemSelected (object sender, XLabs.GridEventArgs<object> e)
		{
			var item = e.Value as MediaItem;
			DisplayAlert ("you selected an item", item.Name, "Ok");
		}

		bool OnGestureShouldBegin (BaseGestureRecognizer gestureRecognizer)
		{
			var shouldBegin = _currentGridY > 0 || _scrollPosition < 10;
			Debug.WriteLine (">>>>>_currentGridY {0} _scrollPosition {1} shouldBegin{2}", _currentGridY, _scrollPosition, shouldBegin);
			return shouldBegin;
		}
	}
}