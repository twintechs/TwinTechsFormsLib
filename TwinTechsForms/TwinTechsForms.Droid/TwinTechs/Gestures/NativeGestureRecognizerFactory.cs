using System;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	public class NativeGestureRecognizerFactory : INativeGestureRecognizerFactory
	{
		public NativeGestureRecognizerFactory ()
		{
		}

		Dictionary<Type,Type> TypeDictionary = new Dictionary<Type, Type> () {
			[typeof(TapGestureRecognizer) ] = typeof(NativeTapGestureRecognizer),
			[typeof(SwipeGestureRecognizer) ] = typeof(NativeSwipeGestureRecognizer),
			[typeof(PanGestureRecognizer) ] = typeof(NativePanGestureRecognizer),
			[typeof(PinchGestureRecognizer) ] = typeof(NativePinchGestureRecgonizer),
			[typeof(LongPressGestureRecognizer) ] = typeof(NativeLongPressGestureRecognizer),
		};


		#region INativeGestureRecognizerFactory implementation

		public void AddNativeGestureRecognizerToRecgonizer<T> (T recognizer) where T : BaseGestureRecognizer
		{
			if (!TypeDictionary.ContainsKey (recognizer.GetType ())) {
				throw new ArgumentException ("no native gesture recognizer for this forms recognizer " + recognizer.GetType ());
			}

			var targetType = TypeDictionary [recognizer.GetType ()];
			var nativeRecongizer = (BaseNativeGestureRecognizer)Activator.CreateInstance (targetType);
			nativeRecongizer.Recognizer = recognizer;
			recognizer.NativeGestureRecognizer = nativeRecongizer;

			if (recognizer.NativeGestureCoordinator == null) {
				recognizer.NativeGestureCoordinator = new NativeGestureCoordinator (recognizer.View);
			}

			var coordinator = recognizer.NativeGestureCoordinator as NativeGestureCoordinator;
			if (coordinator == null) {
				throw new InvalidOperationException ("the recognizer's native gesture coordinator is null, or an invalid type");
			}
			coordinator.AddRecognizer (nativeRecongizer);
		}

		public void RemoveRecognizer (BaseGestureRecognizer recognizer)
		{
			if (recognizer.NativeGestureRecognizer != null) {
				
				var coordinator = recognizer.NativeGestureCoordinator as NativeGestureCoordinator;
				if (coordinator == null) {
					throw new InvalidOperationException ("the recognizer's native gesture coordinator is null, or an invalid type");
				}
				coordinator.RemoveRecognizer ((BaseNativeGestureRecognizer)recognizer.NativeGestureRecognizer);
				if (!coordinator.HasRecognizers) {
					coordinator.Destroy ();
					recognizer.NativeGestureCoordinator = null;
				}
			}
		}

		#endregion


	}
}

