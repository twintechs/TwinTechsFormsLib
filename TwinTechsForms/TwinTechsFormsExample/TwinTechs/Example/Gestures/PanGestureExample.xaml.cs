using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;

namespace TwinTechs.Example.Gestures
{
	public partial class PanGestureExample : ContentPage
	{
		public PanGestureExample ()
		{
			InitializeComponent ();

			AddSomeGesturesUsingCode ();

			//the following is only necessary until Xamarin give us a hook into this I've filed a bugzilla about it
			//https://bugzilla.xamarin.com/show_bug.cgi?id=30467
			//if you don't want to write the following lines of code, then MAKE YOUR VOICE HEARD ON THAT BUG PLEASE! :)
			FixGesturesUsingXaml ();
			Box2.Layout (new Rectangle (100, 100, 50, 50));

			var items = new List<string> ();
			for (int i = 0; i < 200; i++) {
				items.Add ("a test item " + i);
			}
			MockList.ItemsSource = items;

		}

		void AddSomeGesturesUsingCode ()
		{
			//2 options: 
			//1. use the standard xamarin api. e.g. view.GestureRecgonizers.Add(yourgesturerecognizer)
			//    and then call view.ProcessGestures();
			//    this has the benefit that when Xamarin add an api hook we can remove the view.ProcessGestures call and
			//    it will all be good.
			//2. use the extension method view.AddGestureRecognizer(yourgesturerecognizer)
			//    this is easier to use; and does everything under the hood; but it's a bit more obtrusive.
			//   in all cases, until Xamarin do more to open up the api, you must use the view extension method 
			//   removeGestureRecognizer
			// comment on https://bugzilla.xamarin.com/show_bug.cgi?id=30467 to get Xamarin to expand
			// IGestureRecognizer with some add/remove hooks
			var panRecognizer = new PanGestureRecognizer ();
			panRecognizer.OnAction += OnAction;
			var Box3 = new BoxView () {
				Color = Color.Olive,
			};

			MainLayout.Children.Add (Box3);
			Box3.Layout (new Rectangle (100, 400, 100, 100));
//			Box3.AddGestureRecognizer (panRecognizer);
		}

		void FixGesturesUsingXaml ()
		{
			Box.ProcessGestureRecognizers ();
			MyStack.ProcessGestureRecognizers ();
			MyStack2.ProcessGestureRecognizers ();
		}

		Rectangle _startBounds;

		void OnAction (BaseGestureRecognizer recognizer, GestureRecognizerState state)
		{
			var panRecognizer = recognizer as PanGestureRecognizer;
			var view = recognizer.View;
			if (state == GestureRecognizerState.Began) {
				_startBounds = recognizer.View.Bounds;
//				Debug.WriteLine ("START " + _startBounds); 
			} else if (state == GestureRecognizerState.Changed) {
				var message = "PAN " + recognizer + "\n";
				message += "ParentPOS: " + recognizer.LocationInView (view.ParentView);
				message += "POS: " + recognizer.LocationInView (view);
				var translation = panRecognizer.GetTranslationInView (view.ParentView);
				var velocity = panRecognizer.GetVelocityInView (view.ParentView);
//			message += "touches: " + recognizer.NumberOfTouches + ", velocity: " + velocity;
				message += ", translation: " + translation;
				var bounds = new Rectangle (view.X + velocity.X, view.Y + velocity.Y, view.Width, view.Height);
				message += ", vb: " + bounds;
				_startBounds.X += translation.X;
				_startBounds.Y += translation.Y;
//				Debug.WriteLine ("MOVE " + bounds);
				Device.BeginInvokeOnMainThread (() => {
					MyStack.Layout (bounds);
				});
				OutputLabel.Text = message;
			}
		}

		void OnToggleCancelTouch (object s, EventArgs e)
		{
			ListGesture.CancelsTouchesInView = !ListGesture.CancelsTouchesInView;
			StackGesture.CancelsTouchesInView = !StackGesture.CancelsTouchesInView;
		}

		int _clickedCount = 0;

		void OnClickedButtonInPanStack (object sender, EventArgs ev)
		{
			_clickedCount++;
			OutputLabel.Text = "Clicked button " + _clickedCount;
		}
	}
}

