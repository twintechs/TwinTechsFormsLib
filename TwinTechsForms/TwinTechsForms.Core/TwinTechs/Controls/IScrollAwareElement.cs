using System;

namespace TwinTechs.Controls
{
	public class ControlScrollEventArgs : EventArgs
	{
		public float Delta { get; set; }

		public float CurrentY { get; set; }

		public ControlScrollEventArgs (float delta, float currentY)
		{
			this.Delta = delta;
			this.CurrentY = currentY;
		}

	}

	public interface IScrollAwareElement
	{
		event EventHandler OnStartScroll;
		event EventHandler OnStopScroll;
		event EventHandler<ControlScrollEventArgs> OnScroll;

		void RaiseOnScroll (float delta, float currentY);

		void RaiseOnStartScroll ();

		void RaiseOnStopScroll ();
	}
}

