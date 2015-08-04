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
	public class PageViewContainerRenderer : ViewRenderer<PageViewContainer,ViewControllerContainer>
	{
		public PageViewContainerRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<PageViewContainer> e)
		{
			base.OnElementChanged (e);

			if (Control != null) {
				Control.ViewController = null;
			}

			if (e.NewElement != null) {
				var viewControllerContainer = new ViewControllerContainer (Bounds);
				SetNativeControl (viewControllerContainer);
			}


		}

		Page _initializedPage;

		void ChangePage (Page page)
		{
			if (page != null) {
				page.Parent = Element.GetParentPage ();
				var pageRenderer = page.GetRenderer ();
				UIViewController viewController = null;
				if (pageRenderer != null && pageRenderer.ViewController != null) {
					viewController = pageRenderer.ViewController;
				} else {
					viewController = page.CreateViewController ();
				}
				var parentPage = Element.GetParentPage ();
				var renderer = parentPage.GetRenderer ();
				Control.ParentViewController = renderer.ViewController;
				Control.ViewController = viewController;
				_initializedPage = page;
			} else {
				if (Control != null) {
					Control.ViewController = null;
				}
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var page = Element != null ? Element.Content : null;
			if (page != null) {
				page.Layout (new Rectangle (0, 0, Bounds.Width, Bounds.Height));
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == "Content" || e.PropertyName == "Renderer") {
				Device.BeginInvokeOnMainThread (() => ChangePage (Element != null ? Element.Content : null));
			}
		}

	}
}

