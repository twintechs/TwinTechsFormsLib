namespace XLabs.Forms.Controls
{
	using System.ComponentModel;
	using CoreGraphics;

	using CoreGraphics;
	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class GridViewCell.
	/// </summary>
	public class GridViewCell : UICollectionViewCell
	{


		UIView _view;
		object _originalBindingContext;

		FastGridCell _viewCell;

		public FastGridCell ViewCell { get { return _viewCell; } }

		public void RecycleCell (object data, DataTemplate dataTemplate, VisualElement parent)
		{
			if (_viewCell == null) {
				_viewCell = (dataTemplate.CreateContent () as FastGridCell);
				_viewCell.BindingContext = data;
				_viewCell.Parent = parent;
				_viewCell.PrepareCell (new Size (Bounds.Width, Bounds.Height));
				_originalBindingContext = _viewCell.BindingContext;
				var renderer = RendererFactory.GetRenderer (_viewCell.View);
				_view = renderer.NativeView;
				_view.AutoresizingMask = UIViewAutoresizing.All;
				_view.ContentMode = UIViewContentMode.ScaleToFill;
				ContentView.AddSubview (_view);
				return;
			} else if (data == _originalBindingContext) {
				_viewCell.BindingContext = _originalBindingContext;
			} else {
				_viewCell.BindingContext = data;
			}
		}

		/// <summary>
		/// The key
		/// </summary>
		public const string Key = "GridViewCell";

		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewCell"/> class.
		/// </summary>
		/// <param name="frame">The frame.</param>
		[Export ("initWithFrame:")]
		public GridViewCell (CGRect frame) : base (frame)
		{
			// SelectedBackgroundView = new GridItemSelectedViewOverlay (frame);
			// this.BringSubviewToFront (SelectedBackgroundView);
			BackgroundColor = UIColor.Black;

		}

		CGSize _lastSize;

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			if (_lastSize.Equals (CGSize.Empty) || !_lastSize.Equals (Frame.Size)) {

				_viewCell.View.Layout (Frame.ToRectangle ());
				_viewCell.OnSizeChanged (new Xamarin.Forms.Size (Frame.Size.Width, Frame.Size.Height));
				_lastSize = Frame.Size;
			}

			_view.Frame = ContentView.Bounds;
		}
	}
}

