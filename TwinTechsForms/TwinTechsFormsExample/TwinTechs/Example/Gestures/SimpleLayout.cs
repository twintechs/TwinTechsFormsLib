using System;
using Xamarin.Forms;

namespace TwinTechs.Example.Gestures
{
	public delegate void LayoutChildrenDelegate (double x, double y, double width, double height);

	//utility class to allow composing of layout behaviour
	public class SimpleLayout : AbsoluteLayout
	{
		public event LayoutChildrenDelegate OnLayoutChildren;

		public bool IsHandlingLayoutManually = false;
		Rectangle oldSize;

		public SimpleLayout ()
		{
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (!IsHandlingLayoutManually) {
				base.LayoutChildren (x, y, width, height);
			}
			//TODO optimize this
//			var newSize = new Rectangle (x, y, width, height);
//			if (oldSize != newSize) {
//				oldSize = newSize;
			if (OnLayoutChildren != null) {
				OnLayoutChildren (x, y, width, height);
			}
//			}

		}
	}
}

