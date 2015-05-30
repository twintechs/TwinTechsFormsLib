using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Example.FastCells;

namespace TwinTechs.Example
{
	public partial class SampleMenu : ContentPage
	{
		public SampleMenu ()
		{
			InitializeComponent ();
		}

		#region fast cell samples

		void OnDefaultSimpleCell (object sender, object args)
		{
			Navigation.PushAsync (new ViewCellSimple ());
		}

		void OnFastSimpleCell (object sender, object args)
		{
			Navigation.PushAsync (new FastCellSamplePage ());
		}

		void OnFastSimpleCellLots (object sender, object args)
		{
			Navigation.PushAsync (new FastSimpleCellLots ());
		}

		void OnDefaultComplexCell (object sender, object args)
		{
			Navigation.PushAsync (new ViewCellComplexSamplePage ());
		}

		void OnFastComplexCell (object sender, object args)
		{
			Navigation.PushAsync (new FastCellComplexSamplePage ());
		}

		#endregion
	}
}

