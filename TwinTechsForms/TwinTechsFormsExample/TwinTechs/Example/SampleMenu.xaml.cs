using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Example.FastCells;
using TwinTechs.Example.GridView;
using TwinTechs.Example.Gestures;
using TwinTechs.Example.PageInPage;
using TwinTechs.Example.CircleView;

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

		void OnFastCellUnevenHeights (object sender, object args)
		{
			Navigation.PushAsync (new FastSimpleCellUnevenHeight ());
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
			Navigation.PushAsync (new LongPressGestureExample ());

		}

		void OnPanGesture (object sender, object args)
		{
			Navigation.PushAsync (new PanGestureExample ());

		}

		void OnPanGestureAlternate (object sender, object args)
		{
			Navigation.PushAsync (new PanGestureAlternateRouting ());

		}

		void OnPanGestureAlternateDelegate (object sender, object args)
		{
			Navigation.PushAsync (new PanGestureAlternateRoutingDelayAndDelegate ());

		}

		void OnPan2Gesture (object sender, object args)
		{
			Navigation.PushAsync (new PanGestureExample2 ());

		}


		void OnPan3Gesture (object sender, object args)
		{
			Navigation.PushAsync (new PanGestureExample3 ());

		}

		void OnPinchGesture (object sender, object args)
		{
			Navigation.PushAsync (new GridViewOptions ());

		}

		void OnMixedGestures (object sender, object args)
		{
			Navigation.PushAsync (new MultiGestureExample ());

		}


		void OnGesturesInChildViews (object sender, object args)
		{
			Navigation.PushAsync (new GesturesInChildViewsExample ());

		}

		void OnCellGestures (object sender, object args)
		{
			Navigation.PushAsync (new SwipeyListExample ());

		}


		void OnYoutubeStyleGestures (object sender, object args)
		{
			Navigation.PushAsync (new GestureYoutubeLikeExample ());

		}

		void OnDragListGestures (object sender, object args)
		{
			Navigation.PushAsync (new GestureScrollListExample ());

		}



		#endregion

		#region Page in page

		void OnPageInPageSimple (object sender, object args)
		{
			var pageInPage = new PageInPageSample ();
			Navigation.PushAsync (pageInPage);
			
		}

		void OnEmbeddedNavigationPage (object sender, object args)
		{
			var navigationPageInPage = new NavigationPageInPage ();
			Navigation.PushAsync (navigationPageInPage);
			
		}

		#endregion

		#region composition examples

		void OnCircleView (object sender, object args)
		{
			Navigation.PushAsync (new CircleViewSamplePage ());
			
		}

		#endregion

		#region SvgImage examples

		async void OnSvgImage (object sender, EventArgs args)
		{
			await Navigation.PushAsync (new SvgImageSample.SvgImageSamplePage ());
		}

		#endregion
	}
}

