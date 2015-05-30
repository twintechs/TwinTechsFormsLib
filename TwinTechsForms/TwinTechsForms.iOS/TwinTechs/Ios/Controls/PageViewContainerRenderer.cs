using System;
using Xamarin.Forms;
using TwinTechs.Ios.Controls;
using Xamarin.Forms.Platform.iOS;
using TwinTechs.Ios.Extensions;
using TwinTechs.Extensions;
using UIKit;
using TwinTechs.Controls;

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

			if (e.NewElement != null) {
				_viewControllerContainer = new ViewControllerContainer (Bounds);
				_viewControllerContainer.BackgroundColor = UIColor.Orange;
				SetNativeControl (_viewControllerContainer);

			}
		}

		void ChangePage (Page page)
		{
			if (page != null) {
				try {
					var viewController = page.CreateViewController ();
					var parentPage = Element.GetParentPage ();
					var renderer = parentPage.GetRenderer ();
					_viewControllerContainer.ParentViewController = renderer.ViewController;
					_viewControllerContainer.ViewController = viewController;
				} catch (Exception ex) {
					Console.WriteLine ("error creating page " + ex.Message);
				}
			} else {
				_viewControllerContainer = null;
			}
		}

		Page GetPageFromVisualElement (Element e)
		{
			Page returnPage = null;
			var currentElement = e;
			while (currentElement != null && returnPage == null) {
				currentElement = currentElement.Parent;
				returnPage = currentElement as Page;
			}
			return returnPage;
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

