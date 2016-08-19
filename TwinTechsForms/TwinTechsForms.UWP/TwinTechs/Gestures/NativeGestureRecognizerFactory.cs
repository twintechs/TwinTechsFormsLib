using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using TwinTechs.Gestures;
using TwinTechsLib.UWP.TwinTechs.UWP.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using TapGestureRecognizer = TwinTechs.Gestures.TapGestureRecognizer;

namespace TwinTechsLib.UWP.TwinTechs.Gestures
{
    public class NativeGestureRecognizerFactory : INativeGestureRecognizerFactory
    {
        public NativeGestureRecognizerFactory()
        {
        }

        Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type>() {
            { typeof(TapGestureRecognizer) , typeof(NativeTapGestureRecognizer) },
            //{ typeof(SwipeGestureRecognizer) , typeof(NativeSwipeGestureRecognizer) },
            //{ typeof(PanGestureRecognizer) ,typeof(NativePanGestureRecognizer) },
            //{ typeof(PinchGestureRecognizer), typeof(NativePinchGestureRecgonizer) },
            //{ typeof(LongPressGestureRecognizer) , typeof(NativeLongPressGestureRecognizer) },
        };


        #region INativeGestureRecognizerFactory implementation
        
        public void AddNativeGestureRecognizerToRecgonizer<T>(T recognizer) where T : BaseGestureRecognizer
        {
            if (!this.TypeDictionary.ContainsKey(recognizer.GetType()))
            {
                throw new ArgumentException("no native gesture recognizer for this forms recognizer " + recognizer.GetType());
            }
            var targetType = this.TypeDictionary[recognizer.GetType()];
            var nativeRecongizer = (BaseNativeGestureRecognizer)Activator.CreateInstance(targetType);
            nativeRecongizer.Recognizer = recognizer;
            EventHandler handler = null;
            handler = (s, e) =>
             {
                 recognizer.View.SizeChanged -= handler;
                 var r = Platform.GetRenderer(recognizer.View);
                 var nel = r.GetNativeControl();
                 nativeRecongizer.NativeControl = nel;
                 nel.PointerPressed += (es, ee) =>
                 {
                     nativeRecongizer.PointerPressed(ee);
                 };
                 nel.PointerMoved += (es, ee) =>
                 {
                     nativeRecongizer.PointerMoved(ee);
                 };
                 nel.PointerReleased += (es, ee) =>
                 {
                     nativeRecongizer.PointerReleased(ee);
                 };
             };
            recognizer.View.SizeChanged += handler;

            recognizer.NativeGestureRecognizer = nativeRecongizer;
        }


        public void RemoveRecognizer(BaseGestureRecognizer recognizer)
        {
            var nativeRecognizer = recognizer.NativeGestureRecognizer as BaseNativeGestureRecognizer;
            nativeRecognizer.RemoveRecognizer(recognizer);
        }

        #endregion


    }
}
