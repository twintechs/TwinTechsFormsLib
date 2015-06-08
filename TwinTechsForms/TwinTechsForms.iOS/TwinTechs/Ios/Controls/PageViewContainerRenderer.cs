using System;
using Xamarin.Forms;
using TwinTechs.Ios.Controls;
using Xamarin.Forms.Platform.iOS;
using TwinTechs.Ios.Extensions;
using TwinTechs.Extensions;
using UIKit;
using TwinTechs.Controls;
using System.Diagnostics;

[assembly: ExportRenderer (typeof(PageViewContainer), typeof(PageViewContainerRenderer))]
namespace TwinTechs.Ios.Controls
{
	public class PageViewContainerRenderer : ViewRenderer<PageViewContainer,UIView>
	{
		public PageViewContainerRenderer ()
		{
		}

		ViewControllerContainer _viewControllerContainer;

		protected override void OnElementChanged (ElementChangedEventArgs<PageViewContainer> e)
		{
			base.OnElementChanged (e);
			var pageViewContainer = e.NewElement as PageViewContainer;

			if (_viewControllerContainer != null) {
				_viewControllerContainer.ViewController = null;
				_viewControllerContainer = null;
			}

			if (e.NewElement != null) {
				_viewControllerContainer = new ViewControllerContainer (Bounds);
				SetNativeControl (_viewControllerContainer);
			}
			 

		}

		Page _initializedPage;

		void ChangePage (Page page)
		{
			if (page != null) {
				var pageRenderer = page.GetRenderer ();
				var viewController = pageRenderer?.ViewController != null ? pageRenderer?.ViewController : page.CreateViewController ();
				var parentPage = Element.GetParentPage ();
				var renderer = parentPage.GetRenderer ();

				if (_viewControllerContainer == null) {
					_viewControllerContainer = new ViewControllerContainer (Bounds);
					SetNeedsLayout ();
					SetNativeControl (_viewControllerContainer);
				}

				_viewControllerContainer.ParentViewController = renderer.ViewController;
				_viewControllerContainer.ViewController = viewController;
				_initializedPage = page;
			} else {
				_viewControllerContainer = null;
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var page = Element?.Content;
			if (page != null) {
				page.Layout (new Rectangle (0, 0, Bounds.Width, Bounds.Height));
			}
			_viewControllerContainer.Frame = Bounds;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == "Content" || e.PropertyName == "Renderer") {
				Device.BeginInvokeOnMainThread (() => ChangePage (Element?.Content));
			}
		}

	}
}

