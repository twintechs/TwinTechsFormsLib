using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Reflection;

namespace TwinTechs.Droid.Extensions
{
	public static class ViewExtensions
	{
		private static readonly Type _platformType = Type.GetType ("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android", true);
		private static BindableProperty _rendererProperty;

		public static BindableProperty RendererProperty {
			get {
				_rendererProperty = (BindableProperty)_platformType.GetField ("RendererProperty", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
					.GetValue (null);

				return _rendererProperty;
			}
		}

		public static IVisualElementRenderer GetRenderer (this BindableObject bindableObject)
		{
			return (IVisualElementRenderer)bindableObject.GetValue (RendererProperty);
		}

		public static Android.Views.View GetNativeView (this BindableObject bindableObject)
		{
			return bindableObject.GetRenderer ().ViewGroup.RootView;
		}
	}
}