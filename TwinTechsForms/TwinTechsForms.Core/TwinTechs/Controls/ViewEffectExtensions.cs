using System;

namespace TwinTechs.Controls
{
	public enum ViewMaskerType
	{
		None = 0,
		Circle = 1,
		Triangle = 2,
		Square = 3
	}

	public interface IViewEffectExtensionProvider
	{
		void ApplyMaskToView (Xamarin.Forms.View view, ViewMaskerType maskType);

		void ToggleViewShadow (Xamarin.Forms.View view, bool isOn);
	}

	public static class ViewEffectExtensions
	{
		public static IViewEffectExtensionProvider ViewExtensionProvider { get; set; }

		public static void ApplyMask (this Xamarin.Forms.View view, ViewMaskerType maskType)
		{
			ViewExtensionProvider.ApplyMaskToView (view, maskType);
		}

		public static void ToggleShadow (this Xamarin.Forms.View view, bool isOn)
		{
			ViewExtensionProvider.ToggleViewShadow (view, isOn);
		}
	}
}

