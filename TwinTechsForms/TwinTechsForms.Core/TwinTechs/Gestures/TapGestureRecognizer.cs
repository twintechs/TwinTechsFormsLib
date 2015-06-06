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
		}

		public int NumberOfTapsRequired { get; set; } = 1;

		public int NumberOfTouchesRequired { get; set; } = 1;
	}
}


