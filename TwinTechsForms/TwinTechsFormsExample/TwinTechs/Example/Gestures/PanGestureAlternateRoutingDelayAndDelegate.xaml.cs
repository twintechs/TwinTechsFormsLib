using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;

using PanGestureRecognizer = TwinTechs.Gestures.PanGestureRecognizer;

namespace TwinTechs.Example.Gestures
{
	public partial class PanGestureAlternateRoutingDelayAndDelegate : ContentPage
	{
		public PanGestureAlternateRoutingDelayAndDelegate ()
		{
			InitializeComponent ();

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

		void FixGesturesUsingXaml ()
		{
			//NOTE - I'm not sure how to expose a delegate method in xaml yet *blush*
			//need to fix that.

			StackGesture.OnGestureShouldBeginDelegate = OnShouldBegin;
			ListGesture.OnGestureShouldBeginDelegate = OnShouldBegin;

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

		void OnToggleDelaysTouch (object s, EventArgs e)
		{
			ListGesture.DelaysTouches = !ListGesture.DelaysTouches;
			StackGesture.DelaysTouches = !StackGesture.DelaysTouches;
			ToggleDelaysTouchButton.Text = StackGesture.DelaysTouches ? "Delays touches : true" : "Delays touches : false";
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
			StackGesture.OnGestureShouldBeginDelegate = null;
			ListGesture.OnGestureShouldBeginDelegate = null;

			MyStack.RemoveAllGestureRecognizers ();
			MyStack2.RemoveAllGestureRecognizers ();
		}


		bool _onlyHorizontalPan = true;

		void OnToggleOnlyHorizontal (object s, EventArgs e)
		{
			_onlyHorizontalPan = !_onlyHorizontalPan;
			DelegateCancelsGestureButton.Text = _onlyHorizontalPan ? "Only horizontal pan : true" : "Only horizontal pan : false";
		}

		bool OnShouldBegin (BaseGestureRecognizer recognizer)
		{
			var panGestureRecognizer = recognizer as PanGestureRecognizer;
			var translation = panGestureRecognizer.GetTranslationInView (recognizer.View.ParentView);
			var moveX = Math.Abs (translation.X);
			var moveY = Math.Abs (translation.Y);
			return !_onlyHorizontalPan || moveX > moveY;
		}


		void OnToggleCancelTouch (object s, EventArgs e)
		{
			ListGesture.CancelsTouchesInView = !ListGesture.CancelsTouchesInView;
			StackGesture.CancelsTouchesInView = !StackGesture.CancelsTouchesInView;
			ToggleCancelTouchButton.Text = StackGesture.CancelsTouchesInView ? "Cancel touches : true" : "Cancel touches : false";
		}


	}
}

