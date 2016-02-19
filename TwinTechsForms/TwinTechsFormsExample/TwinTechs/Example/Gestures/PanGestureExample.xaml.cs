using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures
{
	public partial class PanGestureExample : ContentPage
	{
		PanGestureRecognizer _stackPanRecognizer;

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
			_stackPanRecognizer = new PanGestureRecognizer ();
			_stackPanRecognizer.OnAction += OnAction;
			MyStack.AddGestureRecognizer (_stackPanRecognizer);
		}

		void FixGesturesUsingXaml ()
		{
			MyStack.ProcessGestureRecognizers ();
			MyStack2.ProcessGestureRecognizers ();
		}

		void OnAction (BaseGestureRecognizer recognizer, GestureRecognizerState state)
		{
			var panRecognizer = recognizer as PanGestureRecognizer;
			var view = recognizer.View;
			var message = "PAN " + recognizer + "\n";
			message += "ParentPOS: " + recognizer.LocationInView (view.ParentView);
			message += "POS: " + recognizer.LocationInView (view);
			var translation = panRecognizer.GetTranslationInView (view.ParentView);
			var velocity = panRecognizer.GetVelocityInView (view.ParentView);
			message += ", translation: " + translation;
			var bounds = new Rectangle (view.X + velocity.X, view.Y + velocity.Y, view.Width, view.Height);
			message += ", vb: " + bounds;
			OutputLabel.Text = message;
		}

		void OnToggleCancelTouch (object s, EventArgs e)
		{
			ListGesture.CancelsTouchesInView = !ListGesture.CancelsTouchesInView;
			_stackPanRecognizer.CancelsTouchesInView = !_stackPanRecognizer.CancelsTouchesInView;
			ToggleCancelTouchButton.Text = _stackPanRecognizer.CancelsTouchesInView ? "Cancel touches : true" : "Cancel touches : false";
		}

		int _clickedCount = 0;

		void OnClickedButtonInPanStack (object sender, EventArgs ev)
		{
			_clickedCount++;
			OutputLabel.Text = "Clicked button " + _clickedCount;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			MyStack.RemoveAllGestureRecognizers ();
			MyStack2.RemoveAllGestureRecognizers ();
		}

	}
}

