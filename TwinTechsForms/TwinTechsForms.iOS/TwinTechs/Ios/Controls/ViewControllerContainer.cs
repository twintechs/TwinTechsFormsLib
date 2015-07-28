using System;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using System.Diagnostics;

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
					
					AddViewController ();
				}
			}
		}

		void AddViewController ()
		{
			if (ParentViewController == null) {
				throw new Exception ("No Parent View controller was found");
			}

			Debug.WriteLine ("vc.v is " + _viewController.View);

			ParentViewController.AddChildViewController (_viewController);
			AddSubview (_viewController.View);

			_viewController.View.Frame = Bounds;
			_viewController.DidMoveToParentViewController (ParentViewController);

		}

		#endregion

		#region lifecycle

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			//hack to fix sizing of children when changing orientation
			if (ViewController != null && ViewController.View.Subviews.Length > 0) {
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

