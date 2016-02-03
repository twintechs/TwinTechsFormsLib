using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using TwinTechs.Controls;
using TwinTechsLib.Droid.Controls;

[assembly:ExportRenderer (typeof(EnhancedWebView), typeof(EnhancedWebViewRenderer))]
namespace TwinTechsLib.Droid.Controls
{
	/// <summary>
	/// Simple addressing the issue with the WebView not stopping videos when leaving.
	/// http://stackoverflow.com/a/3529316/48700
	/// </summary>
	public class EnhancedWebViewRenderer : WebViewRenderer
	{
		protected override void OnDetachedFromWindow ()
		{
			base.OnDetachedFromWindow ();
			Java.Lang.Class.ForName("android.webkit.WebView").GetMethod("onPause", (Java.Lang.Class[]) null).Invoke(Control, (Java.Lang.Object[]) null);
		}
	}
}
