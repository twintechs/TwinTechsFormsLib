using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TwinTechs.Controls;
using TwinTechs.Droid.Controls;
using Android.Views;
using TwinTechs.Droid.Extensions;
using TwinTechs.Extensions;
using Android.App;
using Android;
using System.Reflection;

[assembly: ExportRenderer (typeof(PageViewContainer), typeof(PageViewContainerRenderer))]
namespace TwinTechs.Droid.Controls
{
	public class PageViewContainerRenderer : ViewRenderer<PageViewContainer,Android.Views.View>
	{
		public PageViewContainerRenderer ()
		{
			Console.WriteLine ("+++LOADED");
		}

		Page _currentPage;

		protected override void OnElementChanged (ElementChangedEventArgs<PageViewContainer> e)
		{
			base.OnElementChanged (e);
			var pageViewContainer = e.NewElement as PageViewContainer;
			if (e.NewElement != null) {
				ChangePage (e.NewElement.Content);
			} else {
				ChangePage (null);
			}

		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == "Content") {
				ChangePage (Element.Content);
			}
		}

		bool _contentNeedsLayout;

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);
			if ((changed || _contentNeedsLayout) && this.Control != null) {
				if (_currentPage != null) {
					_currentPage.Layout (new Rectangle (0, 0, Element.Width, Element.Height));
				}
				var msw = MeasureSpec.MakeMeasureSpec (r - l, MeasureSpecMode.Exactly);
				var msh = MeasureSpec.MakeMeasureSpec (b - t, MeasureSpecMode.Exactly);
				this.Control.Measure (msw, msh);
				this.Control.Layout (0, 0, r, b);
				_contentNeedsLayout = false;
			}
		}

		private int ConvertPixelsToDp (float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}


		void ChangePage (Page page)
		{

			//TODO handle current page
			if (page != null) {
				var parentPage = Element.GetParentPage ();
				page.Parent = parentPage;

				var existingRenderer = page.GetRenderer ();
				if (existingRenderer == null) {
					var renderer = RendererFactory.GetRenderer (page);
					page.SetRenderer (renderer);
					existingRenderer = page.GetRenderer ();
				}
				_contentNeedsLayout = true;
				SetNativeControl (existingRenderer.ViewGroup);
				Invalidate ();
				//TODO update the page
				_currentPage = page;
			} else {
				//TODO - update the page
				_currentPage = null;
			}

			if (_currentPage == null) {
				//have to set somethign for android not to get pissy
				var view = new Android.Views.View (this.Context);
				view.SetBackgroundColor (Element.BackgroundColor.ToAndroid ());
				SetNativeControl (view);
			}
		}

	}
}

