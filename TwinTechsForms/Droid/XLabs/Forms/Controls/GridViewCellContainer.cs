using System;
using Xamarin.Forms.Platform.Android;

using XLabs.Forms.Controls;
using TwinTechs.Droid.Controls;
using Xamarin.Forms;
using Android.Views;
using Android.Content;
using Android.Widget;
using TwinTechs.Droid.Extensions;
using TwinTechs.Droid.Helper;

namespace XLabs.Forms.Controls
{
	public class GridViewCellContainer : ViewGroup
	{

		global::Android.Views.View _parent;
		FastGridCell _viewCell;

		ViewGroup _nativeView;

		public GridViewCellContainer (Context context, FastGridCell fastGridCell, global::Android.Views.View parent, Size initialCellSize) : base (context)
		{
			_parent = parent;
			_viewCell = fastGridCell;
			fastGridCell.PrepareCell (initialCellSize);
			_viewCell.View.BackgroundColor = Xamarin.Forms.Color.Blue;
			var renderer = RendererHelper.GetOrCreateRenderer (fastGridCell.View);
			_nativeView = renderer.ViewGroup;
			SetBackgroundColor (Android.Graphics.Color.Orange);
			AddView (_nativeView);
		}

		public void Update (object bindingContext)
		{
			_viewCell.BindingContext = bindingContext;
			SetBackgroundColor (Android.Graphics.Color.Blue);

		}


		Size _previousSize;
		bool _isLaidOut;

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			double width = base.Context.FromPixels ((double)(r - l));
			double height = base.Context.FromPixels ((double)(b - t));
			var size = new Size (width, height);

			var msw = MeasureSpec.MakeMeasureSpec (r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec (b - t, MeasureSpecMode.Exactly);
			_nativeView.Measure (msw, msh);
			_nativeView.Layout (0, 0, r - l, b - t);
			
//			if (size != _previousSize) {
			var layout = _viewCell.View as Layout<Xamarin.Forms.View>;
			if (layout != null) {
				layout.Layout (new Rectangle (0, 0, width, height));
				layout.ForceLayout ();
				FixChildLayouts (layout);
				_isLaidOut = true;
			}
			_previousSize = size;
//			}
		}

		void FixChildLayouts (Layout<Xamarin.Forms.View> layout)
		{
			foreach (var child in layout.Children) {
				if (child is Layout<Xamarin.Forms.View>) {
					((Layout<Xamarin.Forms.View>)child).ForceLayout ();
					FixChildLayouts (child as Layout<Xamarin.Forms.View>);
				}
			}
		}

		//TODO should really be an extension

	}
}

