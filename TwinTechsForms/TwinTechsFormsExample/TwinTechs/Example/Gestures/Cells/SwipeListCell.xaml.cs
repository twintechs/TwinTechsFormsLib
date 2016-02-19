using System;
using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures.Cells
{

	//TODO - fastcell has a bug in it which causes the renderer not to be set on some of the top level view! need to work out why.
	public partial class SwipeListCell : ViewCell
	{
		Rectangle _contentBounds;

		public SwipeListCell ()
		{
			InitializeComponent ();
			MainLayout.OnLayoutChildren += MainLayout_OnLayoutChildren;
			//NOTE - I'm not sure how to expose a delegate method in xaml yet *blush*
			//need to fix that.

			ContentGesture.OnGestureShouldBeginDelegate = OnShouldBegin;
			ForegroundContent.ProcessGestureRecognizers ();
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			var mediaItem = BindingContext as MediaItem;
			if (mediaItem != null) {
				_contentBounds.X = 0;
				ForegroundContent.Layout (_contentBounds);
				UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
				NameLabel.Text = mediaItem.Name + " An item with a pannable content overlay";
			}
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_contentBounds.X = 0;
		}

		void MainLayout_OnLayoutChildren (double x, double y, double width, double height)
		{
			//TODO - if dragging, probably ignore this
			if (_contentBounds == Rectangle.Zero) {
				_contentBounds = new Rectangle (0, 0, width, height);
			} else if (_contentBounds.Width != width) {
				_contentBounds.Width = width;
				_contentBounds.Height = height;
			}
			BackgroundContent.Layout (new Rectangle (width / 2, 0, width / 2, height));
			ForegroundContent.Layout (_contentBounds);
		}

		Rectangle _startBounds;

		void Gesture_OnAction (BaseGestureRecognizer recgonizer, GestureRecognizerState state)
		{
			var panGesture = recgonizer as PanGestureRecognizer;
			Point translation = panGesture.GetTranslationInView (MainLayout);
			Point velocity = panGesture.GetVelocityInView (MainLayout);
			panGesture.SetTranslationInView (new Point (0, 0), MainLayout);


			Debug.WriteLine ("GESTURE STATE " + state + " trans " + translation);
			switch (panGesture.State) {
			case GestureRecognizerState.Began:
				break;
			case GestureRecognizerState.Changed:
				if (recgonizer.View == ForegroundContent) {
					_contentBounds.X += translation.X;
					_contentBounds.Y = 0;
					ForegroundContent.Layout (_contentBounds);
				}
				break;
			case GestureRecognizerState.Cancelled:
			case GestureRecognizerState.Ended:
			case GestureRecognizerState.Failed:
				_contentBounds.X = (_contentBounds.X > -(MainLayout.Width / 4)) ? 0 : -(MainLayout.Width / 2);
				ForegroundContent.LayoutTo (_contentBounds);
				break;
			case GestureRecognizerState.Possible:
			default:
				break;
			}
		}

		bool OnShouldBegin (BaseGestureRecognizer recognizer)
		{
			var panGestureRecognizer = recognizer as PanGestureRecognizer;
			var translation = panGestureRecognizer.GetTranslationInView (recognizer.View.ParentView);
			var moveX = Math.Abs (translation.X);
			var moveY = Math.Abs (translation.Y);
			return moveX > moveY;
		}
	}
}

