using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;

namespace TwinTechs.Example.Gestures
{
	public partial class SwipeGestureExample : ContentPage
	{
		public SwipeGestureExample ()
		{
			InitializeComponent ();

			AddSomeGesturesUsingCode ();

			//the following is only necessary until Xamarin give us a hook into this I've filed a bugzilla about it
			//https://bugzilla.xamarin.com/show_bug.cgi?id=30467
			//if you don't want to write the following lines of code, then MAKE YOUR VOICE HEARD ON THAT BUG PLEASE! :)
			FixGesturesUsingXaml ();


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
			var swipeRecognizer = new SwipeGestureRecognizer ();
			swipeRecognizer.OnAction += OnAction;
			swipeRecognizer.Direction = SwipeGestureRecognizerDirection.Left;
			Box2.AddGestureRecognizer (swipeRecognizer);

			var swipeRecognizerWith2Tocuhes = new TwinTechs.Gestures.TapGestureRecognizer ();
			swipeRecognizerWith2Tocuhes.OnAction += OnAction;
			swipeRecognizer.Direction = SwipeGestureRecognizerDirection.Right;
			swipeRecognizerWith2Tocuhes.NumberOfTouchesRequired = 2;
			Label2.GestureRecognizers.Add (swipeRecognizerWith2Tocuhes);
			Label2.ProcessGestureRecognizers ();


		}

		void FixGesturesUsingXaml ()
		{
			//TODO write converter
			BoxXamlGesture.Direction = SwipeGestureRecognizerDirection.Up;
			LabelXamlGesture.Direction = SwipeGestureRecognizerDirection.Down;
			LabelXamlGesture.NumberOfTouchesRequired = 2;
			Box.ProcessGestureRecognizers ();
			Label1.ProcessGestureRecognizers ();
		}

		void OnAction (BaseGestureRecognizer recognizer, GestureRecognizerState state)
		{
			var swipeRecognizer = recognizer as SwipeGestureRecognizer;
			var view = recognizer.View;
			var message = "SWIPE " + recognizer + "\n";
			message += "POS: " + recognizer.LocationInView (view.ParentView);
			message += "touches: " + recognizer.NumberOfTouches + ", direction: " + swipeRecognizer.Direction;
			OutputLabel.Text = message;
		}
	}
}

