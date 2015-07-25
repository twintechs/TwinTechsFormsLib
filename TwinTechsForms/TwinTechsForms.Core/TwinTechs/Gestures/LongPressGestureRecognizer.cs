using System;

namespace TwinTechs.Gestures
{
	public class LongPressGestureRecognizer : BaseGestureRecognizer
	{
		/// <summary>
		/// Gets or sets the minimum duration of the press.
		/// </summary>
		/// <value>The minimum duration of the press.</value>
		public double MinimumPressDuration { get; set; } = 1;

		/// <summary>
		/// Gets or sets the number of touches required.
		/// </summary>
		/// <value>The number of touches required.</value>
		public int NumberOfTouchesRequired { get; set; } = 1;

		/// <summary>
		/// Gets or sets the max distance a finger can move before failing a long press.
		/// </summary>
		/// <value>The max distance before fail.</value>
		public int MaxDistanceTolerance { get; set; } = 5;


		public LongPressGestureRecognizer ()
		{
		}
	}
}

