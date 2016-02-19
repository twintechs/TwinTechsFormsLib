using Xamarin.Forms;
using TwinTechs.Gestures;
using TwinTechs.Extensions;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures
{
	public partial class PanGestureExample3 : ContentPage
	{
		Rectangle _outerLayoutBounds = new Rectangle (200, 400, 200, 250);
		Rectangle _box2Bounds = new Rectangle (100, 200, 150, 150);
		BoxView Box;
		BoxView Box2;
		StackLayout OuterView;

		public PanGestureExample3 ()
		{
			InitializeComponent ();
			MainLayout.OnLayoutChildren += MainLayout_OnLayoutChildren;

			OuterView = new StackLayout {
				Padding = new Thickness (50),
				BackgroundColor = Color.Yellow
			};
			this.MainLayout.Children.Add (OuterView);
			OuterView.Layout (_outerLayoutBounds);

			Box = new BoxView {
				Color = Color.Red,
				WidthRequest = 150,
				HeightRequest = 150,
			};
			OuterView.Children.Add (Box);
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
					_outerLayoutBounds.X += translation.X;
					_outerLayoutBounds.Y += translation.Y;
					OuterView.Layout (_outerLayoutBounds);
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

