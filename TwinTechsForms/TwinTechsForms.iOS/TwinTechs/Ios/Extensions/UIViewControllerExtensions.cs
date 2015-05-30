using System;
using UIKit;

namespace TwinTechs.Ios.Extensions
{
	public static class UIViewControllerExtensions
	{
		public static UIViewController GetViewController (this UIView view)
		{
			var responder = (UIResponder)view;
			while (responder != null && !(responder is UIViewController)) {
				responder = responder.NextResponder;
			}

			return (UIViewController)responder;
		}
	}
}

