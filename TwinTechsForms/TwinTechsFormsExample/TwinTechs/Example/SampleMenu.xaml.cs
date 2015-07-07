using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Example.FastCells;
using TwinTechs.Example.GridView;
using TwinTechs.Example.Gestures;
using TwinTechs.Example.PageInPage;

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

		#region Xlabs GridView improvements


		void OnGridViewPerformance (object sender, object args)
		{
			Navigation.PushAsync (new GridViewPerformance ());
		}

		void OnGridViewPerformance2 (object sender, object args)
		{
			Navigation.PushAsync (new GridViewPerformance2 ());
		}

		void OnGridViewPerformance3 (object sender, object args)
		{
			Navigation.PushAsync (new GridViewXamlPerformance ());
		}

		void OnGridViewProperties (object sender, object args)
		{
			Navigation.PushAsync (new GridViewOptions ());
		}

		#endregion

		#region gestures

		void OnTapGesture (object sender, object args)
		{
			Navigation.PushAsync (new TapGestureExample ());
		}


		void OnPageScrollerExample (object sender, object args)
		{
			Navigation.PushAsync (new ViewScrollerExample ());

		}

		void OnSwipeGesture (object sender, object args)
		{
			Navigation.PushAsync (new SwipeGestureExample ());

		}

		void OnLongPressGesture (object sender, object args)
		{
			Navigation.PushAsync (new GridViewOptions ());

		}

		void OnPanGesture (object sender, object args)
		{
			Navigation.PushAsync (new PanGestureExample ());

		}

		void OnPinchGesture (object sender, object args)
		{
			Navigation.PushAsync (new GridViewOptions ());

		}

		void OnMixedGestures (object sender, object args)
		{
			Navigation.PushAsync (new GridViewOptions ());

		}

		#endregion

		#region Page in page

		void OnPageInPageSimple (object sender, object args)
		{
			var pageInPage = new PageInPageSample ();
			Navigation.PushAsync (pageInPage);
			
		}

		#endregion
	}
}

