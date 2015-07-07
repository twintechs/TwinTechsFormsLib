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

			var panRecognizerWith2Tocuhes = new TwinTechs.Gestures.PanGestureRecognizer ();
			panRecognizerWith2Tocuhes.OnAction += OnAction;
			panRecognizerWith2Tocuhes.MinimumNumberOfTouches = 2;
			Label2.GestureRecognizers.Add (panRecognizerWith2Tocuhes);
			Label2.ProcessGestureRecognizers ();


		}

		void FixGesturesUsingXaml ()
		{
			Box.ProcessGestureRecognizers ();
			Label1.ProcessGestureRecognizers ();
		}

		Rectangle _startBounds;

		void OnAction (BaseGestureRecognizer recognizer, GestureRecognizerState state)
		{
			var panRecognizer = recognizer as PanGestureRecognizer;
			var view = recognizer.View;
			if (state == GestureRecognizerState.Began) {
				_startBounds = recognizer.View.Bounds;
				Debug.WriteLine ("START " + _startBounds); 
			}
			if (state == GestureRecognizerState.Changed) {
				var message = "PAN " + recognizer + "\n";
				message += "POS: " + recognizer.LocationInView (view.ParentView);
				var translation = panRecognizer.GetTranslationInView (view.ParentView);
//			message += "touches: " + recognizer.NumberOfTouches + ", velocity: " + velocity;
				message += ", translation: " + translation;
				var bounds = new Rectangle (view.X + translation.X, view.Y + translation.Y, view.Width, view.Height);
				message += ", vb: " + bounds;
				_startBounds.X += translation.X;
				_startBounds.Y += translation.Y;
				Debug.WriteLine ("MOVE " + bounds);
				Device.BeginInvokeOnMainThread (() => {
					Label2.Layout (bounds);
				});
				OutputLabel.Text = message;
			}
		}

		void OnMoveClicked (object s, EventArgs e)
		{
			Box2.Layout (new Rectangle (Box2.X + 50, 100, 50, 50));
		}
	}
}

