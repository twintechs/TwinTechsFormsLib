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

			FixGesturesUsingXaml ();
		}

		void FixGesturesUsingXaml ()
		{

			//the following is only necessary until Xamarin give us a hook into this I've filed a bugzilla about it
			//https://bugzilla.xamarin.com/show_bug.cgi?id=30467
			//if you don't want to write the following lines of code, then MAKE YOUR VOICE HEARD ON THAT BUG PLEASE! :)
			Label1.ProcessGestureRecognizers ();
			Label2.ProcessGestureRecognizers ();
			Label3.ProcessGestureRecognizers ();
			Label4.ProcessGestureRecognizers ();
			Label5.ProcessGestureRecognizers ();
			Label6.ProcessGestureRecognizers ();
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

