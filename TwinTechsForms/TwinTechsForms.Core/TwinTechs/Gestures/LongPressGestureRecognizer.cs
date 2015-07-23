using System;

namespace TwinTechs.Gestures
{
	public class LongPressGestureRecognizer : BaseGestureRecognizer
	{
		//TODO
		public double MinimumPressDuration { get; set; } = 1;

		public int NumberOfTouchesRequired { get; set; } = 1;


		public LongPressGestureRecognizer ()
		{
		}
	}
}

