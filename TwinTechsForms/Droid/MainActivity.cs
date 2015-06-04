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

namespace TwinTechsFormsExample.Droid
{
	[Activity (Label = "TwinTechsFormsExample.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			AppHelper.FastCellCache = FastCellCache.Instance;

			var metrics = Resources.DisplayMetrics;
			AppHelper.ScreenSize = new Xamarin.Forms.Size (ConvertPixelsToDp (metrics.WidthPixels), ConvertPixelsToDp (metrics.HeightPixels));


			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}

		private int ConvertPixelsToDp (float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}
	}
}

