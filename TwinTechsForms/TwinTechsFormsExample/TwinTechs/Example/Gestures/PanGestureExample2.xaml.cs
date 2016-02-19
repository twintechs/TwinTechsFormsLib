using Xamarin.Forms;
using TwinTechs.Gestures;
using TwinTechs.Extensions;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures
{
	public partial class PanGestureExample2 : ContentPage
	{
		Rectangle _boxBounds = new Rectangle (200, 400, 200, 200);
		Rectangle _box2Bounds = new Rectangle (100, 200, 150, 150);
		BoxView Box;
		BoxView Box2;

		public PanGestureExample2 ()
		{
			InitializeComponent ();
			MainLayout.OnLayoutChildren += MainLayout_OnLayoutChildren;
			Box = new BoxView ();
			Box.Color = Color.Red;
			this.MainLayout.Children.Add (Box);
			Box.Layout (_boxBounds);
			var panRecognizer = new PanGestureRecognizer ();
			panRecognizer.IsConsumingTouchesInParallel = true;
			panRecognizer.OnAction += Gesture_OnAction;
			Box.AddGestureRecognizer (panRecognizer);

			Box2 = new BoxView ();
			Box2.Color = Color.Blue;
			this.MainLayout.Children.Add (Box2);
			Box2.Layout (_box2Bounds);
			panRecognizer = new PanGestureRecognizer ();
			panRecognizer.OnAction += Gesture_OnAction;
			panRecognizer.IsConsumingTouchesInParallel = true;
			Box2.AddGestureRecognizer (panRecognizer);
			DoBoxAnimation ();

		}

		void MainLayout_OnLayoutChildren (double x, double y, double width, double height)
		{
			OutputLabel.Layout (new Rectangle (0, 0, width, 200));
//			Box.Layout (_boxBounds);
//			Box2.Layout (_box2Bounds);
		}

		async void DoBoxAnimation ()
		{
			await Box2.LayoutTo (new Rectangle (100, 400, 150, 150), 20000);
			await Box2.LayoutTo (new Rectangle (100, 100, 150, 150), 20000);
			DoBoxAnimation ();
		}

		Rectangle _startBounds;

		void Gesture_OnAction (BaseGestureRecognizer recgonizer, GestureRecognizerState state)
		{
			string message = "";
			var panGesture = recgonizer as PanGestureRecognizer;
			Point translation = panGesture.GetTranslationInView (MainLayout);
			Point velocity = panGesture.GetVelocityInView (MainLayout);
			panGesture.SetTranslationInView (new Point (0, 0), MainLayout);

			//			MyLabel.Text = "PAN " + recognizer.LocationInView (view.ParentView);
			//			Debug.WriteLine ("PAN " + panGesture);
			//			Debug.WriteLine ("location " + panGesture.LocationInView (this));

			switch (panGesture.State) {
			case GestureRecognizerState.Began:
				message += "BEGAN ";
				message += "POS: " + recgonizer.LocationInView (MainLayout);
				break;
			case GestureRecognizerState.Changed:
				message += "CHANGED ";
				message += ", translation: " + translation;
				message += ", velocity: " + velocity;
				message += "POS: \n" + recgonizer.LocationInView (panGesture.View).PrettyPrint ();
				message += "PARENT POS: \n" + recgonizer.LocationInView (MainLayout).PrettyPrint ();
				if (recgonizer.View == Box) {
					message += ", MOVING VIEW";
					_boxBounds.X += translation.X;
					_boxBounds.Y += translation.Y;
					Box.Layout (_boxBounds);
				}
				break;
			case GestureRecognizerState.Cancelled:
			case GestureRecognizerState.Ended:
			case GestureRecognizerState.Failed:
				message += "FINISHED ";
				break;
			case GestureRecognizerState.Possible:
			default:
				break;
			}
			OutputLabel.Text = message;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			Box.RemoveAllGestureRecognizers ();
			Box2.RemoveAllGestureRecognizers ();
		}
	}
}

