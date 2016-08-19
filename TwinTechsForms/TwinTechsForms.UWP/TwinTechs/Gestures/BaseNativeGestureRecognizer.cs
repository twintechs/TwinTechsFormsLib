using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using TwinTechs.Gestures;
using Xamarin.Forms;

namespace TwinTechsLib.UWP.TwinTechs.Gestures
{
    public abstract class BaseNativeGestureRecognizer : INativeGestureRecognizer
    {
        internal BaseGestureRecognizer Recognizer { get; set; }
        public int NumberOfTouches { get; set; }

        public void UpdateCancelsTouchesInView(bool _cancelsTouchesInView)
        {
           
        }

        protected virtual bool IsGestureCotinuous { get { return false; } }
        protected Xamarin.Forms.Point FirstTouchPoint { get; set; }
        public FrameworkElement NativeControl { get; set; }
        protected int PointerId = -1;
        GestureRecognizerState _state;
        bool _gestureDidBegin;

        public GestureRecognizerState State
        {
            get
            {
                return _state;
            }
            protected set
            {
                var oldState = _state;
                _state = value;
                if (oldState == GestureRecognizerState.Possible && value == GestureRecognizerState.Began)
                {
                    if (Recognizer.OnGestureShouldBeginDelegate != null && !Recognizer.OnGestureShouldBeginDelegate(Recognizer))
                    {
                        _state = GestureRecognizerState.Failed;
                    }
                }

                if (_state == GestureRecognizerState.Cancelled || _state == GestureRecognizerState.Ended || _state == GestureRecognizerState.Failed)
                {
                    PointerId = -1;
                    this.NumberOfTouches = 0;
                }

                //we track if the gesture had begun at some point in processing this gesture, so we can elect which continuous events to send
                if (_state == GestureRecognizerState.Began)
                {
                    _gestureDidBegin = true;
                }

                if (_state == GestureRecognizerState.Recognized || (IsGestureCotinuous && _gestureDidBegin))
                {
                    SendGestureEvent();
                }

                if (GetIsFinishedState(_state))
                {
                    _gestureDidBegin = false;
                }
            }
        }

        private void SendGestureEvent()
        {
            if (Recognizer != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Recognizer.SendAction();
                });
            }
        }

        bool GetIsFinishedState(GestureRecognizerState state)
        {
            return state == GestureRecognizerState.Ended || state == GestureRecognizerState.Cancelled || state == GestureRecognizerState.Recognized ||
            state == GestureRecognizerState.Failed;
        }

        public void UpdateDelaysTouches(bool _delaysTouches)
        {
           
        }

        public Point LocationOfTouch(int touchIndex, VisualElement view)
        {
            return this.FirstTouchPoint;
        }

        public Point LocationInView(VisualElement view)
        {
            return this.FirstTouchPoint;
        }

        public void RemoveRecognizer(BaseGestureRecognizer recognizer)
        {           
            recognizer.NativeGestureRecognizer = null;
        }

        public abstract void PointerPressed(PointerRoutedEventArgs e);
        public abstract void PointerMoved(PointerRoutedEventArgs e);
        public abstract void PointerReleased(PointerRoutedEventArgs e);
    }
}
