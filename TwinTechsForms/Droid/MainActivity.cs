using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using TwinTechs.Example;
using TwinTechs.Droid.Controls;
using TwinTechs;
using Android.Util;
using System.Runtime.InteropServices;
using Xamarin.Forms.Platform.Android;
using TwinTechs.Gestures;

namespace TwinTechsFormsExample.Droid
{
	[Activity (Label = "TwinTechsFormsExample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		DummyIncludes _dummyIncludes;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			AppHelper.FastCellCache = FastCellCache.Instance;

			var metrics = Resources.DisplayMetrics;
			AppHelper.ScreenSize = new Xamarin.Forms.Size (ConvertPixelsToDp (metrics.WidthPixels), ConvertPixelsToDp (metrics.HeightPixels));
			GestureRecognizerExtensions.Factory = new NativeGestureRecognizerFactory ();

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		private int ConvertPixelsToDp (float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}
		//
		//		public override bool DispatchTouchEvent (MotionEvent ev)
		//		{
		//			//find if there's a view container with a gesture, which is currently on the screen.
		//			foreach (var recognizer in BaseNativeGestureRecognizer.GroupRecognizers) {
		//				var nativeRecognizer = recognizer.NativeGestureRecognizer as BaseNativeGestureRecognizer;
		//				if (nativeRecognizer.ConsumesActivityTouch (ev)) {
		//					return true;
		//				}
		//			}
		//			return base.DispatchTouchEvent (ev);
		//		}
	}
}

