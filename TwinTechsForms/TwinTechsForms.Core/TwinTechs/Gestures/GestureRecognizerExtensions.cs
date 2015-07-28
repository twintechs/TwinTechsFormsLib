using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace TwinTechs.Gestures
{
	public static class GestureRecognizerExtensions
	{
		//Set this property by whatever means you have for IOC
		public static INativeGestureRecognizerFactory Factory { get; set; }

		public static void ProcessGestureRecognizers (this View view)
		{
			foreach (var gestureRecognizer in view.GestureRecognizers) {
				var baseGestureRecognizer = gestureRecognizer as BaseGestureRecognizer;
				if (baseGestureRecognizer != null) {
					view.ProcessGestureRecognizer (baseGestureRecognizer);
				}
			}
		}

		private static void ProcessGestureRecognizer (this View view, BaseGestureRecognizer recognizer)
		{
			if (recognizer.NativeGestureRecognizer == null) {

				recognizer.View = view;

				Factory.AddNativeGestureRecognizerToRecgonizer (recognizer);
			}
		}

		public static void AddGestureRecognizer (this View view, IGestureRecognizer recognizer)
		{
			view.GestureRecognizers.Add (recognizer);
			var baseGestureRecognizer = recognizer as BaseGestureRecognizer;
			if (baseGestureRecognizer != null) {
				view.ProcessGestureRecognizer (baseGestureRecognizer);
			}
		}

		/// <summary>
		/// Removes the gesture recognizer. This is a temporary method while Xamarin don't expose any api
		/// for us to hook into their gesture recognizers mechanism.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="recognizer">Recognizer.</param>
		public static void RemoveAllGestureRecognizers (this View view)
		{
			var recognizersToRemove = view.GestureRecognizers.ToArray ();
			foreach (var recognizer in recognizersToRemove) {
				var baseGesture = recognizer as BaseGestureRecognizer;
				if (baseGesture != null) {
					Factory.RemoveRecognizer (baseGesture);
					view.GestureRecognizers.Remove (recognizer);
				}
			}
		}
	}
}

