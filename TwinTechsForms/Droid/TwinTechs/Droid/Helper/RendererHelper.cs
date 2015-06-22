using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Reflection;
using TwinTechs.Droid.Extensions;

namespace TwinTechs.Droid.Helper
{
	public static class RendererHelper
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

		public static void SetRenderer (BindableObject bindableObject, IVisualElementRenderer renderer)
		{
			var value = bindableObject.GetValue (RendererProperty);
			bindableObject.SetValue (RendererProperty, renderer);
		}

		public static IVisualElementRenderer GetRenderer (BindableObject bindableObject)
		{
			var value = bindableObject.GetValue (RendererProperty);
			return (IVisualElementRenderer)bindableObject.GetValue (RendererProperty);
		}

		public static Android.Views.View GetNativeView (BindableObject bindableObject)
		{
			var renderer = bindableObject.GetRenderer ();
			var viewGroup = renderer.ViewGroup;
			var rootView = viewGroup.RootView;
			return rootView;
		}

		/// <summary>
		/// Gets the or create renderer.
		/// </summary>
		/// <returns>The or create renderer.</returns>
		/// <param name="source">Source.</param>
		public static IVisualElementRenderer GetOrCreateRenderer (VisualElement source)
		{
			var renderer = RendererHelper.GetRenderer (source);
			if (renderer == null) {
				renderer = RendererFactory.GetRenderer (source);
				RendererHelper.SetRenderer (source, renderer);
				renderer = RendererHelper.GetRenderer (source);
			}
			return renderer;
		}
	}
}

