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
			[typeof(LongPressGestureRecognizer) ] = typeof(NativeTapGestureRecognizer),
		};


		#region INativeGestureRecognizerFactory implementation

		public INativeGestureRecognizer CreateNativeGestureRecognizer<T> (T recognizer)
		{
			if (!TypeDictionary.ContainsKey (recognizer.GetType ())) {
				throw new ArgumentException ("no native gesture recognizer for this forms recognizer " + recognizer.GetType ());
			}
			var targetType = TypeDictionary [recognizer.GetType ()];
			return (INativeGestureRecognizer)Activator.CreateInstance (targetType);
		}

		#endregion


	}
}

