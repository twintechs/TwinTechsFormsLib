using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;


[assembly: 
	InternalsVisibleTo ("TwinTechsLib.iOS"),
	InternalsVisibleTo ("TwinTechsLib.Droid")]
namespace TwinTechs.Gestures
{
	public enum SwipeGestureRecognizerDirection
	{
		Right = 1,
		Left = 2,
		Up = 4,
		Down = 8
	}

	public class SwipeGestureRecognizer : BaseGestureRecognizer
	{

		public int NumberOfTouchesRequired { get; set; } = 1;

		public SwipeGestureRecognizerDirection Direction { get; set; }


		public SwipeGestureRecognizer ()
		{
			
		}

	}
}

