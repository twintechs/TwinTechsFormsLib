using System;

using Xamarin.Forms;

namespace TwinTechs.Example.Gestures
{
	public class GestureRecognizingAbsoluteLayout : Layout<View>
	{
		public event LayoutChildrenDelegate OnLayoutChildren;

		public bool IsHandlingLayoutManually = false;
		Rectangle oldSize;

		public GestureRecognizingAbsoluteLayout ()
		{
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			foreach (var child in Children) {
				child.Layout (child.Bounds);
				var layout = child as Layout<View>;
				if (layout != null) {
					layout.ForceLayout ();
				}
					
			}
//			if (!IsHandlingLayoutManually) {
//				base.LayoutChildren (x, y, width, height);
//			}
//			//TODO optimize this
//			//			var newSize = new Rectangle (x, y, width, height);
//			//			if (oldSize != newSize) {
//			//				oldSize = newSize;
//			if (OnLayoutChildren != null) {
//				OnLayoutChildren (x, y, width, height);
//			}
			//			}

		}
	}
}


