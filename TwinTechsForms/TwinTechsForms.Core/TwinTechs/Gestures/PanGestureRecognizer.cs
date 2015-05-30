using System;
using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public interface INativePanGestureRecognizer : INativeGestureRecognizer
	{
		Point GetVelocityInView (VisualElement view);

		Point GetTranslationInView (VisualElement view);
	}

	public class PanGestureRecognizer : BaseGestureRecognizer
	{
		public int MinimumNumberOfTouches { get; set; } = 1;

		public int MaximumNumberOfTouches { get; set; } = 1;


		public Point GetVelocityInView (VisualElement view)
		{
			return (NativeGestureRecognizer as INativePanGestureRecognizer).GetVelocityInView (view);
		}

		public Point GetTranslationInView (VisualElement view)
		{
			return (NativeGestureRecognizer as INativePanGestureRecognizer).GetTranslationInView (view);
		}

		public PanGestureRecognizer ()
		{
		}
	}
}

