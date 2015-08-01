using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Gestures;
using System.Diagnostics;
using System.Text;

namespace TwinTechs.Example.Gestures
{
	public partial class MultiGestureExample : ContentPage
	{
		public MultiGestureExample ()
		{
			InitializeComponent ();

			//the following is only necessary until Xamarin give us a hook into this I've filed a bugzilla about it
			//https://bugzilla.xamarin.com/show_bug.cgi?id=30467
			//if you don't want to write the following lines of code, then MAKE YOUR VOICE HEARD ON THAT BUG PLEASE! :)
			FixGesturesUsingXaml ();
		}

		void FixGesturesUsingXaml ()
		{
			Label1.ProcessGestureRecognizers ();
			Label2.ProcessGestureRecognizers ();
		}

		void OnAction (BaseGestureRecognizer recognizer, GestureRecognizerState state)
		{
			var tapRecognizer = recognizer as TwinTechs.Gestures.LongPressGestureRecognizer;
			var view = recognizer.View;
			var message = recognizer + "\n";
			message += "POS: " + recognizer.LocationInView (view);
			message += "PARENT POS: " + recognizer.LocationInView (view.ParentView);
			message += "touches: " + recognizer.NumberOfTouches;// + ", taps required: " + tapRecognizer.NumberOfTapsRequired;
			OutputLabel.Text = message;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			Label1.RemoveAllGestureRecognizers ();
			Label2.RemoveAllGestureRecognizers ();
		}
	}
}

