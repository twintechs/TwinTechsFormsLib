using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using TwinTechs;
using TwinTechs.Example;
using TwinTechs.Ios.Controls;

namespace TwinTechsFormsExample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			AppHelper.FastCellCache = FastCellCache.Instance;

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

