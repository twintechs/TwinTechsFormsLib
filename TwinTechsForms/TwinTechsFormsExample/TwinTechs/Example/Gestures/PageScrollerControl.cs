using System;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;
using System.Linq;
using System.Collections.ObjectModel;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures
{
	public class PageScrollerControl : AbsoluteLayout
	{
		public event EventHandler OnPageChanged;

		StackLayout _contentView;

		//		public IList<View>Pages {
		//			get{ return _contentView.Children; }
		//		}

		public ObservableCollection<VisualElement> Pages { get; private set; }

		int _currentPageIndex = 0;

		public int NumberOfPages { get { return Pages.Count (); } }

		bool _isAnimating;

		public int CurrentPageIndex {
			get { return _currentPageIndex; }
			set {
				_currentPageIndex = value; 
				AnimateToPageIndex (_currentPageIndex);

			}
		}

		async void AnimateToPageIndex (object index, bool animated = true)
		{
			var bounds = GetBoundsWithX (-_currentPageIndex * Width);
			_isAnimating = true;
			if (animated) {
				await _contentView.LayoutTo (bounds);
			} else {
				_contentView.Layout (bounds);
			}
			_isAnimating = false;
		}

		public PageScrollerControl () : base ()
		{
			Pages = new ObservableCollection<VisualElement> ();
			_contentView = new StackLayout () {Orientation = StackOrientation.Horizontal,
				Spacing = 0
			};
			Children.Add (_contentView);
			var gesture2 = new PanGestureRecognizer ();
			gesture2.OnAction += Gesture_OnAction;
			gesture2.IsConsumingTouchesInParallel = true;
			gesture2.CancelsTouchesInView = true;
			this.AddGestureRecognizer (gesture2);
			IsClippedToBounds = true;
			Pages.CollectionChanged += Pages_CollectionChanged;
		}

		void Pages_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			foreach (var child in e.NewItems) {
				if (!_contentView.Children.Contains (child)) {
					_contentView.Children.Add ((View)child);
				}
			}
		}

		double _contentWidth;

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (Pages != null) {
				foreach (var child in Pages) {
					child.WidthRequest = width;
				}
			}
			base.LayoutChildren (x, y, width, height);
			_contentWidth = NumberOfPages * width;
			_contentView.Layout (GetBoundsWithX (-_currentPageIndex * Width));
//			if (!_isAnimating && !_sizeChanged) {
//			}

		}

		void Gesture_OnAction (BaseGestureRecognizer recgonizer, GestureRecognizerState state)
		{
			var panGesture = recgonizer as PanGestureRecognizer;
			Point translation = panGesture.GetTranslationInView (this);
			panGesture.SetTranslationInView (new Point (0, 0), this);

			//			MyLabel.Text = "PAN " + recognizer.LocationInView (view.ParentView);
//			Debug.WriteLine ("PAN " + panGesture);
//			Debug.WriteLine ("location " + panGesture.LocationInView (this));

			switch (panGesture.State) {
			case GestureRecognizerState.Began:
				break;
			case GestureRecognizerState.Changed:
				double newPosition = _contentView.X + translation.X;
				var endWidth = _contentWidth;
				Debug.WriteLine ("new pos " + newPosition + " endWidth " + endWidth);
				if (newPosition >= ((-endWidth) - (Width / 2)) && (newPosition <= (Width / 2))) {
					_contentView.Layout (GetBoundsWithX (newPosition));
				}
				break;
			case GestureRecognizerState.Cancelled:
			case GestureRecognizerState.Ended:
			case GestureRecognizerState.Failed:
				//scroll to page location, set index
				double currentPage = (-_contentView.X) / Width;
				Boolean isBehindHalfScreen = (currentPage - (int)currentPage) > 0.5;

				if (isBehindHalfScreen && (currentPage + 1) < NumberOfPages) {
					CurrentPageIndex = (int)(currentPage + 1);
				} else {
					CurrentPageIndex = (int)currentPage;
				}

				AnimateToPageIndex (CurrentPageIndex);
				break;
			case GestureRecognizerState.Possible:
			default:
				break;
			}
		}

		Rectangle GetBoundsWithX (double origin)
		{
			return new Rectangle (origin, 0, _contentWidth, Height);
		}

		public void Cleanup ()
		{
			this.RemoveAllGestureRecognizers ();
		}
	}

}


