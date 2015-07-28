using System;
using System.Collections.Generic;

namespace TwinTechs.Gestures
{
	/// <summary>
	/// Creates gesture recognizers
	/// </summary>
	public class NativeGestureRecognizerFactory : INativeGestureRecognizerFactory
	{
		#region INativeGestureRecognizerFactory implementation

		Dictionary<Type,Type> TypeDictionary = new Dictionary<Type, Type> () {
			[typeof(SwipeGestureRecognizer) ] = typeof(NativeSwipeGestureRecognizer),
			[typeof(PanGestureRecognizer) ] = typeof(NativePanGestureRecognizer),
			[typeof(PinchGestureRecognizer) ] = typeof(NativePinchGestureRecognizer),
			[typeof(LongPressGestureRecognizer) ] = typeof(NativeLongPressGestureRecgonizer),
			[typeof(TwinTechs.Gestures.TapGestureRecognizer) ] = typeof(NativeTapPressGestureRecgonizer),
		};

		public void AddNativeGestureRecognizerToRecgonizer<T> (T recognizer) where T : BaseGestureRecognizer
		{
			if (!TypeDictionary.ContainsKey (recognizer.GetType ())) {
				throw new ArgumentException ("no native gesture recognizer for this forms recognizer " + recognizer.GetType ());
			}
			var targetType = TypeDictionary [recognizer.GetType ()];
			var nativeRecongizer = (IBaseNativeGestureRecognizerImpl)Activator.CreateInstance (targetType);
			nativeRecongizer.AddRecognizer (recognizer);
			recognizer.NativeGestureRecognizer = nativeRecongizer;
		}


		public INativeGestureRecognizerCoordinator CreateNativeGestureCoordinator ()
		{
			throw new InvalidOperationException ("iOS does not use the native gesture coordinator.");
		}


		public void RemoveRecognizer (BaseGestureRecognizer recognizer)
		{
			var nativeRecognizer = recognizer.NativeGestureRecognizer as IBaseNativeGestureRecognizerImpl;
			nativeRecognizer.RemoveRecognizer (recognizer);
		}

		#endregion
		
	}
}

