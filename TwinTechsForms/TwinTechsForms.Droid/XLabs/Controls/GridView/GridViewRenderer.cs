using System;
using Xamarin.Forms.Platform.Android;
using Android.Support.V7.Widget;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Android.Content.Res;
using Android.Views;
using LabsGridView = XLabs.Forms.Controls.GridView;
using System.Collections.ObjectModel;
using System.Collections;
using System.Runtime.InteropServices;
using Android.Widget;

[assembly: ExportRenderer (typeof(LabsGridView), typeof(GridViewRenderer))]
namespace XLabs.Forms.Controls
{
	public class GridViewRenderer :  ViewRenderer<LabsGridView, RecyclerView>
	{
		private readonly Android.Content.Res.Orientation _orientation = Android.Content.Res.Orientation.Undefined;

		ScrollRecyclerView _recyclerView;

		private RecyclerView.LayoutManager _layoutManager;
		private GridViewAdapter _adapter;

		RecyclerView.ItemDecoration _paddingDecoration;

		public GridViewRenderer ()
		{
		}


		#region overridden

		protected override void OnConfigurationChanged (Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			if (newConfig.Orientation != _orientation)
				OnElementChanged (new ElementChangedEventArgs<LabsGridView> (Element, Element));
		}

		protected override void OnElementChanged (ElementChangedEventArgs<XLabs.Forms.Controls.GridView> e)
		{
			base.OnElementChanged (e);
			if (e.NewElement != null) {
				CreateRecyclerView ();
				base.SetNativeControl (_recyclerView);
			}
			//TODO unset
			//			this.Unbind (e.OldElement);
			//			this.Bind (e.NewElement);

		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == "ItemsSource") {
				_adapter.Items = Element.ItemsSource;
			}

			if (e.PropertyName == "IsScrollEnabled") {
				Device.BeginInvokeOnMainThread (() => {
					_recyclerView.Enabled = Element.IsScrollEnabled;
//					Debug.WriteLine ("scroll enabled changed to " + _gridCollectionView.ScrollEnabled);
				}
				);

			
			}
		}

		#endregion

		void DestroyRecyclerview ()
		{
			//TODO
			_recyclerView.Touch -= _recyclerView_Touch;
		}

		void CreateRecyclerView ()
		{
			_recyclerView = new ScrollRecyclerView (Android.App.Application.Context);
			_recyclerView.Touch += _recyclerView_Touch;
			var scrollListener = new GridViewScrollListener (Element, _recyclerView);
			_recyclerView.AddOnScrollListener (scrollListener);
			if (Element.IsHorizontal) {
				var linearLayoutManager = new LinearLayoutManager (Context, OrientationHelper.Horizontal, false);
				_layoutManager = linearLayoutManager;

			} else {
				var gridlayoutManager = new GridLayoutManager (Context, 1);

				_layoutManager = gridlayoutManager;

			}
			_recyclerView.SetLayoutManager (_layoutManager);
			_recyclerView.SetItemAnimator (null);
			_recyclerView.HasFixedSize = true;

			_recyclerView.HorizontalScrollBarEnabled = Element.IsHorizontal;
			_recyclerView.VerticalScrollBarEnabled = !Element.IsHorizontal;

			_adapter = new GridViewAdapter (Element.ItemsSource, _recyclerView, Element, Resources.DisplayMetrics);

			_recyclerView.SetAdapter (_adapter);
			UpdatePadding ();
		}


		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);
			UpdatePadding ();
		}

		void UpdatePadding ()
		{
			_recyclerView.SetPadding ((int)Element.ContentPaddingLeft, 
				(int)Element.ContentPaddingTop, 
				(int)Element.ContentPaddingRight, 
				(int)Element.ContentPaddingBottom);
			if (Element.IsHorizontal) {
				if (_paddingDecoration != null) {
					_recyclerView.RemoveItemDecoration (_paddingDecoration);
				}
				var source = Element.ItemsSource as ICollection;
				var numberOfItems = source == null ? 0 : source.Count;
				_paddingDecoration = new HorizontalSpacesItemDecoration (ConvertDpToPixels ((float)Element.ColumnSpacing / 2), ConvertDpToPixels ((int)Element.RowSpacing));
				_recyclerView.AddItemDecoration (_paddingDecoration);

			} else {
				UpdateGridLayout ();
			}

		}

		void UpdateGridLayout ()
		{
			var source = Element.ItemsSource as ICollection;
			var numberOfItems = source == null ? 0 : source.Count;

			var horizontalPadding = 0;
			int numberOfItemsToUse = 1;
			if (_layoutManager != null) {
				if (Element.Width > 0) {
					//					float width = (float)_recyclerView.Width - 2;
					if (Element.IsContentCentered) {

						float width = (float)Element.Width;
						int numberOfItemsThatFit = (int)Math.Floor ((width) / (Element.ItemWidth + Element.ColumnSpacing));
						numberOfItemsToUse = Element.CenterAsFilledRow ? numberOfItemsThatFit : (int)Math.Min (numberOfItemsThatFit, numberOfItems);
						if (Element.MaxItemsPerRow != -1) {
							numberOfItemsToUse = Element.MaxItemsPerRow;
						}
						var remainingWidth = (width - (Element.ContentPaddingLeft + Element.ContentPaddingRight)) - ((numberOfItemsToUse * Element.ItemWidth) + ((numberOfItemsToUse) * (Element.ColumnSpacing)));
						horizontalPadding = (int)(remainingWidth / (numberOfItemsToUse + 1));
					} else {
						horizontalPadding = (int)Element.ColumnSpacing;
					}

					Console.WriteLine (" width {0} items using {1} padding {2} iwdith {3} ", _recyclerView.Width, numberOfItemsToUse, horizontalPadding, Element.ItemWidth);
				}
			}


			var gridLayoutManager = _layoutManager as GridLayoutManager;
			if (gridLayoutManager != null) {
				//TODO calculate
				gridLayoutManager.SpanCount = Math.Max (1, numberOfItemsToUse);
			}
			//TODO
			if (_paddingDecoration != null) {
				_recyclerView.RemoveItemDecoration (_paddingDecoration);
			}
			_paddingDecoration = new SpacesItemDecoration (ConvertDpToPixels (horizontalPadding), ConvertDpToPixels ((int)Element.RowSpacing), 
				numberOfItems, numberOfItemsToUse, 
				ConvertDpToPixels ((int)Element.ContentPaddingTop), ConvertDpToPixels ((int)Element.ContentPaddingBottom));

			//			_paddingDecoration = new SpacesItemDecoration (horizontalPadding, (int)Element.RowSpacing, 
			//				numberOfItems, numberOfItemsToUse, 
			//				(int)Element.ContentPaddingTop, (int)Element.ContentPaddingBottom);
			_recyclerView.AddItemDecoration (_paddingDecoration);

		}


		private int ConvertDpToPixels (float dpValue)
		{
			var pixels = (int)((dpValue) * Resources.DisplayMetrics.Density);
			return pixels;
		}

		float _startEventY;
		float _heightChange;


		void _recyclerView_Touch (object sender, TouchEventArgs e)
		{
			Console.WriteLine ("ExtendedWebViewRenderer_Touch");
			var ev = e.Event;
			MotionEventActions action = ev.Action & MotionEventActions.Mask;
			switch (action) {
			case MotionEventActions.Down:
				_startEventY = ev.GetY ();
				_heightChange = 0;
				Element.RaiseOnStartScroll ();
				//				Console.WriteLine ("START start ", _startEventY);
				break;
			case MotionEventActions.Move:
				float delta = (ev.GetY () + _heightChange) - _startEventY;
				Element.RaiseOnScroll (delta, _recyclerView.GetVerticalScrollOffset ());

				//				Console.WriteLine ("scrolling delta is {0}, change {1}, start {2}", delta, _heightChange, _startEventY);
				//				Console.WriteLine ("SCROLLY  {0},", _recyclerView.GetVerticalScrollOffset ());
				break;
			case MotionEventActions.Up:
				Element.RaiseOnStopScroll ();
				break;
			}
			e.Handled = false;

		}
	}

	public class SpacesItemDecoration : RecyclerView.ItemDecoration
	{
		int _columnSpacing;
		int _rowSpacing;
		int _numberOfItemsPerRow;
		int _numberOfItems;
		int _topSpacing;
		int _bottomSpacing;

		public SpacesItemDecoration (int columnSpacing, int rowSpacing, int numberOfItems, int numberOfItemsPerRow, int topSpacing, int bottomSpacing)
		{
			_rowSpacing = rowSpacing;
			_columnSpacing = columnSpacing;
			_numberOfItems = numberOfItems;
			_numberOfItemsPerRow = numberOfItemsPerRow;
			_topSpacing = topSpacing;
			_bottomSpacing = bottomSpacing;
		}

		public override void GetItemOffsets (Android.Graphics.Rect outRect, int itemPosition, RecyclerView parent)
		{
			//TODO - work out if the rectangle is the last/first row/column
			if (itemPosition % _numberOfItemsPerRow == 0) {
				//first col
				outRect.Left = _columnSpacing;
			} else {
				outRect.Left = _columnSpacing / 2;
			}
			if (itemPosition % _numberOfItemsPerRow == (_numberOfItemsPerRow - 1)) {
				//last col
				outRect.Right = _columnSpacing;
			} else {
				outRect.Right = _columnSpacing / 2;
			}
			//TODO write a custom layout for android
			//			if (itemPosition < _numberOfItemsPerRow) {
			//				outRect.Top = _topSpacing;
			//			} 
			//			if (itemPosition > (_numberOfItems - _numberOfItemsPerRow)) {
			//				outRect.Bottom = _bottomSpacing;
			//			} else {
			outRect.Bottom = _rowSpacing;
			//			}
		}
	}

	public class HorizontalSpacesItemDecoration : RecyclerView.ItemDecoration
	{
		int _columnSpacing;
		int _rowSpacing;

		public HorizontalSpacesItemDecoration (int columnSpacing, int rowSpacing)
		{
			_rowSpacing = rowSpacing;
			_columnSpacing = columnSpacing;
			_columnSpacing = columnSpacing;
		}

		public override void GetItemOffsets (Android.Graphics.Rect outRect, int itemPosition, RecyclerView parent)
		{
			outRect.Left = _columnSpacing / 2;
			outRect.Right = _columnSpacing / 2;
			outRect.Bottom = _rowSpacing;
		}
	}

	public class ScrollRecyclerView : RecyclerView
	{
		public ScrollRecyclerView (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}


		public ScrollRecyclerView (Android.Content.Context context, Android.Util.IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
		}


		public ScrollRecyclerView (Android.Content.Context context, Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
		}


		public ScrollRecyclerView (Android.Content.Context context) : base (context)
		{
		}

		public int GetVerticalScrollOffset ()
		{
			return ComputeVerticalScrollOffset ();
		}

		public int GetHorizontalScrollOffset ()
		{
			return ComputeHorizontalScrollOffset ();
		}
	}

	public class GridViewScrollListener : RecyclerView.OnScrollListener
	{
		LabsGridView _gridView;

		ScrollRecyclerView _recyclerView;

		public GridViewScrollListener (GridView gridView, ScrollRecyclerView recyclerView)
		{
			_gridView = gridView;
			_recyclerView = recyclerView;
		}

		public override void OnScrolled (RecyclerView recyclerView, int dx, int dy)
		{
			base.OnScrolled (recyclerView, dx, dy);
			_gridView.RaiseOnScroll (dy, _recyclerView.GetVerticalScrollOffset ());
			Console.WriteLine (">>>>>>>>> {0},{1}", dy, _recyclerView.GetVerticalScrollOffset ());
		}
	}
}

