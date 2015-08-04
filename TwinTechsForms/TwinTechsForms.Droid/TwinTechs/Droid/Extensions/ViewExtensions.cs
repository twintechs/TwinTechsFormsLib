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
			var value = bindableObject.GetValue (RendererProperty);
			return (IVisualElementRenderer)bindableObject.GetValue (RendererProperty);
		}

		public static Android.Views.View GetNativeView (this BindableObject bindableObject)
		{
			var renderer = bindableObject.GetRenderer ();
			var viewGroup = renderer.ViewGroup;
			var rootView = viewGroup.RootView;

			//TODO - look at why I returned the root view not the viewgoup - I think this is amistake I checked in but now has implications for many things
			return rootView;
		}
	}
}