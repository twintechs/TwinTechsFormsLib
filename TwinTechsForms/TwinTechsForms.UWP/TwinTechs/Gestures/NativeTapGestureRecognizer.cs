using System;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using TwinTechs.Gestures;

namespace TwinTechsLib.UWP.TwinTechs.Gestures
{
    public class NativeTapGestureRecognizer : BaseNativeGestureRecognizer
    {
        public NativeTapGestureRecognizer()
        {
        }

        DispatcherTimer _multiTapTimer;

        int _currentTapCount;
        DateTime _startTime;

        TapGestureRecognizer TapGestureRecognizer { get { return this.Recognizer as TapGestureRecognizer; } }

        void OnDown(PointerRoutedEventArgs e)
        {
            //TODO - should really be possible until all taps/fingers are satisfied.
            if (TapGestureRecognizer.NumberOfTouchesRequired > 1)
            {
                if (this.NumberOfTouches != this.TapGestureRecognizer.NumberOfTouchesRequired)
                    return;
            }
            else
                this.State = (this.NumberOfTouches == this.TapGestureRecognizer.NumberOfTouchesRequired)
                    ? GestureRecognizerState.Began
                    : GestureRecognizerState.Failed;

            this._currentTapCount = 0;
            //TODO track all pointers that are down.
            this.PointerId = (int)e.Pointer.PointerId;
            this.FirstTouchPoint = new Xamarin.Forms.Point(e.GetCurrentPoint(this.NativeControl).Position.X, e.GetCurrentPoint(this.NativeControl).Position.Y);
            if (this.TapGestureRecognizer.NumberOfTapsRequired > 1 && this.State == GestureRecognizerState.Began)
            {
                this.ResetMultiTapTimer(true);
            }
        }


        void OnUp(PointerRoutedEventArgs e)
        {
            var tooLongBetweenTouches = (DateTime.Now - this._startTime).Milliseconds > 400;
            var wrongNumberOfTouches = this.NumberOfTouches < (this.Recognizer as TapGestureRecognizer).NumberOfTouchesRequired;
            if (tooLongBetweenTouches || wrongNumberOfTouches)
            {
                this.State = GestureRecognizerState.Failed;
                return;
            }
            this._currentTapCount++;

            var requiredTaps = (this.Recognizer as TapGestureRecognizer).NumberOfTapsRequired;
            if (requiredTaps == 1)
            {
                this.State = GestureRecognizerState.Recognized;
                this.NumberOfTouches = 0;
            }
            else
            {

                if (this._currentTapCount == requiredTaps)
                {
                    this.State = GestureRecognizerState.Recognized;
                    this.ResetMultiTapTimer(false);
                    this._currentTapCount = 0;
                    this.NumberOfTouches = 0;
                }
            }

            return;
        }

        private void _multiTapTimer_Tick(object sender, object e)
        {
            this._currentTapCount = 0;
            this.NumberOfTouches = 1;
            this.ResetMultiTapTimer(false);
        }

        void ResetMultiTapTimer(bool isActive)
        {
            if (this._multiTapTimer != null)
            {
                this._multiTapTimer.Tick -= this._multiTapTimer_Tick;
                this._multiTapTimer.Stop();
            }
            if (isActive)
            {
                this.State = GestureRecognizerState.Possible;
                this._multiTapTimer = new DispatcherTimer();
                this._multiTapTimer.Interval = TimeSpan.FromMilliseconds(300 * (this.TapGestureRecognizer.NumberOfTapsRequired - 1));
                this._multiTapTimer.Tick += this._multiTapTimer_Tick;
                this._multiTapTimer.Start();
            }
            else
            {
                this._currentTapCount = 0;
                if (this.State == GestureRecognizerState.Possible)
                {
                    this.State = GestureRecognizerState.Failed;
                }
            }

        }


        public override void PointerPressed(PointerRoutedEventArgs e)
        {
            this._startTime = DateTime.Now;
            this.NumberOfTouches++;
            if (this.PointerId == -1)
            {
                this.OnDown(e);
                if (this.State == GestureRecognizerState.Began)
                {
                    e.Handled = true;
                }
            }
        }

        public override void PointerMoved(PointerRoutedEventArgs e)
        {
        }

        public override void PointerReleased(PointerRoutedEventArgs e)
        {
            this.OnUp(e);
        }
    }
}

