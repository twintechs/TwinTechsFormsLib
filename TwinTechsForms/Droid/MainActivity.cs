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
using TwinTechs.Gestures;
using System.Threading.Tasks;
using System.Collections.Generic;
using TwinTechs.Controls;
using TwinTechs.Droid.Controls;

namespace TwinTechsFormsExample.Droid
{
	[Activity (Label = "TwinTechsFormsExample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		DummyIncludes _dummyIncludes;
		GestureTouchDispatcher _gestureTouchDispatcher;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			_gestureTouchDispatcher = new GestureTouchDispatcher (this);
			AppHelper.FastCellCache = FastCellCache.Instance;

			var metrics = Resources.DisplayMetrics;
			AppHelper.ScreenSize = new Xamarin.Forms.Size (ConvertPixelsToDp (metrics.WidthPixels), ConvertPixelsToDp (metrics.HeightPixels));
			GestureRecognizerExtensions.Factory = new NativeGestureRecognizerFactory ();

			global::Xamarin.Forms.Forms.Init (this, bundle);

			ViewEffectExtensions.ViewExtensionProvider = new ViewMaskExtensionProvider ();
			LoadApplication (new App ());
		}

		private int ConvertPixelsToDp (float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}

		public override bool DispatchTouchEvent (MotionEvent ev)
		{
			var didConsumeTouch = _gestureTouchDispatcher.DispatchTouchEvent (ev);
			//TODO - consider not passing this along?
			var isHandledByNormalRouting = base.DispatchTouchEvent (ev);
			return isHandledByNormalRouting;
		}
	}
}

