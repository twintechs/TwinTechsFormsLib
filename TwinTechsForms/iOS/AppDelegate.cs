using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using TwinTechs;
using TwinTechs.Example;
using TwinTechs.Ios.Controls;
using TwinTechs.Controls;
using System.Drawing;
using TwinTechs.Gestures;
using TwinTechs.iOS;

namespace TwinTechsFormsExample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			SvgImageRenderer.Init ();
			AppHelper.FastCellCache = FastCellCache.Instance;
			AppHelper.ScreenSize = new Xamarin.Forms.Size (UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);
			GestureRecognizerExtensions.Factory = new NativeGestureRecognizerFactory ();

			ViewEffectExtensions.ViewExtensionProvider = new ViewMaskExtensionProvider ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

