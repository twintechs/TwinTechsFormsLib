using System;

using Xamarin.Forms;

namespace TwinTechs.Gestures
{
	public interface ITapGestureRecognizer : INativeGestureRecognizer
	{
	}

	public class TapGestureRecognizer  : BaseGestureRecognizer
	{
		public TapGestureRecognizer ()
		{
			NumberOfTapsRequired = 1;
			NumberOfTouchesRequired = 1;
		}

		public int NumberOfTapsRequired { get; set; }

		public int NumberOfTouchesRequired { get; set; }

		public override string ToString ()
		{
			return string.Format ("[TapGestureRecognizer: NumberOfTapsRequired={0}, NumberOfTouchesRequired={1}, State={2}]", NumberOfTapsRequired, NumberOfTouchesRequired, State);
		}
		
	}
}


