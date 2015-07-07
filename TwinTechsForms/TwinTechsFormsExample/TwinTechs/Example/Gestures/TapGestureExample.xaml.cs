using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;
using System.Text;

namespace TwinTechs.Example.Gestures
{
	public partial class TapGestureExample : ContentPage
	{
		public TapGestureExample ()
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
			var tapRecognizer = new TwinTechs.Gestures.TapGestureRecognizer ();
			tapRecognizer.OnAction += OnAction;
			Box2.AddGestureRecognizer (tapRecognizer);

			var tapRecognizerWith2Tocuhes = new TwinTechs.Gestures.TapGestureRecognizer ();
			tapRecognizerWith2Tocuhes.OnAction += OnAction;
			tapRecognizerWith2Tocuhes.NumberOfTouchesRequired = 2;
			Label2.GestureRecognizers.Add (tapRecognizerWith2Tocuhes);
			Label2.ProcessGestureRecognizers ();


		}

		void FixGesturesUsingXaml ()
		{
			Box.ProcessGestureRecognizers ();
			Label1.ProcessGestureRecognizers ();
		}

		void OnAction (BaseGestureRecognizer recognizer, GestureRecognizerState state)
		{
			var tapRecognizer = recognizer as TwinTechs.Gestures.TapGestureRecognizer;
			var view = recognizer.View;
			var message = "Tap";
			message += "TAP " + recognizer + "\n";
			message += "POS: " + recognizer.LocationInView (view);
			message += "PARENT POS: " + recognizer.LocationInView (view.ParentView);
			message += "touches: " + recognizer.NumberOfTouches + ", taps required: " + tapRecognizer.NumberOfTapsRequired;
			OutputLabel.Text = message;
		}
	}
}

