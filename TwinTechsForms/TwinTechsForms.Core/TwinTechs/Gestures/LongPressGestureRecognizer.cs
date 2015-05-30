using System;

namespace TwinTechs.Gestures
{
	public class LongPressGestureRecognizer : BaseGestureRecognizer
	{
		public double MinimumPressDuration { get; set; } = 1;

		public LongPressGestureRecognizer ()
		{
		}
	}
}

