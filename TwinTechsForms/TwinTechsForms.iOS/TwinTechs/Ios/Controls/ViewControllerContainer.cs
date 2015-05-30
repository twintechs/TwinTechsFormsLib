using System;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;

namespace TwinTechs.Ios.Controls
{
	public class ViewControllerContainer : UIView
	{
		public ViewControllerContainer (CGRect frame) : base (frame)
		{
			BackgroundColor = Color.Transparent.ToUIColor ();
		}

		public UIViewController ParentViewController {
			get;
			set;
		}

		#region properties

		UIViewController _viewController;

		public UIViewController ViewController {
			get{ return _viewController; }
			set {
				if (_viewController != null) {
					RemoveCurrentViewController ();
				}
				_viewController = value;

				if (_viewController != null) {
					if (ParentViewController == null) {
						throw new Exception ("No Parent View controller was found");
					}
				}
				AddViewController ();
			}
		}

		void AddViewController ()
		{
//			var testView = new UIView (new CGRect (0, 0, 200, 200)) {
//				BackgroundColor = UIColor.Blue,
//			};
			AddSubview (_viewController.View);
//			AddSubview (testView);
			ParentViewController.AddChildViewController (_viewController);
			_viewController.View.Frame = Bounds;
			_viewController.View.Tag = 2;

			_viewController.DidMoveToParentViewController (ParentViewController);

		}

		#endregion

		#region lifecycle

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			//hack to fix sizing of children when changing orientation
			if (ViewController?.View.Subviews.Length > 0) {
				foreach (UIView view in ViewController.View.Subviews) {
					view.Frame = Bounds;
				}
			}
//			if (ViewController != null) {
//				ViewController.View.Frame = Bounds;
//			}
		}

		#endregion

		#region private impl

		void RemoveCurrentViewController ()
		{
			if (ViewController != null) {
				ViewController.WillMoveToParentViewController (null);
				ViewController.View.RemoveFromSuperview ();
				ViewController.RemoveFromParentViewController ();
			}
		}

		#endregion
	}
}

