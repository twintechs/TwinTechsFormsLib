using System;

namespace TwinTechs.Gestures
{
	public class LongPressGestureRecognizer : BaseGestureRecognizer
	{
		/// <summary>
		/// Gets or sets the minimum duration of the press.
		/// </summary>
		/// <value>The minimum duration of the press.</value>
		public double MinimumPressDuration { get; set; }

		/// <summary>
		/// Gets or sets the number of touches required.
		/// </summary>
		/// <value>The number of touches required.</value>
		public int NumberOfTouchesRequired { get; set; }

		/// <summary>
		/// Gets or sets the max distance a finger can move before failing a long press.
		/// </summary>
		/// <value>The max distance before fail.</value>
		public int MaxDistanceTolerance { get; set; }


		public LongPressGestureRecognizer ()
		{
			MinimumPressDuration = 1;
			NumberOfTouchesRequired = 1;
			MaxDistanceTolerance = 5;
		}

		public override string ToString ()
		{
			return string.Format ("[LongPressGestureRecognizer: MinimumPressDuration={0}, NumberOfTouchesRequired={1}, MaxDistanceTolerance={2}, State={2}]", MinimumPressDuration, NumberOfTouchesRequired, MaxDistanceTolerance, State);
		}
		
	}
}

