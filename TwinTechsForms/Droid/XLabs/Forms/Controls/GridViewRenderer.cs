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

[assembly: ExportRenderer (typeof(LabsGridView), typeof(GridViewRenderer))]
namespace XLabs.Forms.Controls
{
	public class GridViewRenderer :  ViewRenderer<LabsGridView, RecyclerView>
	{
		private readonly Android.Content.Res.Orientation _orientation = Android.Content.Res.Orientation.Undefined;

		RecyclerView _recyclerView;

		private RecyclerView.LayoutManager _layoutManager;
		private GridViewAdapter _adapter;

		SpacesItemDecoration _paddingDecoration;

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
		}

		#endregion

		void DestroyRecyclerview ()
		{
			//TODO
		}

		void CreateRecyclerView ()
		{
			_recyclerView = new RecyclerView (Android.App.Application.Context);
			_recyclerView.SetBackgroundColor (Android.Graphics.Color.Green);

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
			//				_gridViewContainer.GridView.ItemClick += CollectionViewItemClick;
		}


		//		void CollectionViewItemClick (object sender, AdapterView.ItemClickEventArgs e)
		//		{
		//			var item = this.Element.ItemsSource.Cast<object> ().ElementAt (e.Position);
		//			this.Element.InvokeItemSelectedEvent (this, item);
		//		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);
			UpdatePadding ();
		}

		void UpdatePadding ()
		{
			var source = Element.ItemsSource as ICollection;
			var numberOfItems = source == null ? 0 : source.Count;

			var horizontalPadding = 0;
			int numberOfItemsToUse = 1;
			if (_layoutManager != null) {
				if (Element.Width > 0) {
//					float width = (float)_recyclerView.Width - 2;
					float width = (float)Element.Width;
					int numberOfItemsThatFit = (int)Math.Floor ((width - (Element.ContentPaddingLeft + Element.ContentPaddingRight)) / (Element.ItemWidth + Element.ColumnSpacing));
					numberOfItemsToUse = Element.CenterAsFilledRow ? numberOfItemsThatFit : (int)Math.Min (numberOfItemsThatFit, numberOfItems);
					if (Element.MaxItemsPerRow != -1) {
						numberOfItemsToUse = Element.MaxItemsPerRow;
					}
					var remainingWidth = width - (numberOfItemsToUse * (Element.ItemWidth + Element.ColumnSpacing));
					horizontalPadding = (int)(remainingWidth / (numberOfItemsToUse + 1));

					Console.WriteLine (" width {0} items using {1} padding {2} iwdith {3} ", _recyclerView.Width, numberOfItemsToUse, horizontalPadding, Element.ItemWidth);
				}
			}


			var gridLayoutManager = _layoutManager as GridLayoutManager;
			if (gridLayoutManager != null) {
				//TODO calculate
				gridLayoutManager.SpanCount = numberOfItemsToUse;
			}
			//TODO
			if (_paddingDecoration != null) {
				_recyclerView.RemoveItemDecoration (_paddingDecoration);
			}
			_paddingDecoration = new SpacesItemDecoration (horizontalPadding, (int)Element.RowSpacing);
			_recyclerView.AddItemDecoration (_paddingDecoration);
		}
	}

	public class SpacesItemDecoration : RecyclerView.ItemDecoration
	{
		int _columnSpacing;
		int _rowSpacing;

		public SpacesItemDecoration (int columnSpacing, int rowSpacing)
		{
			_rowSpacing = rowSpacing;
			_columnSpacing = columnSpacing;
		}

		public override void GetItemOffsets (Android.Graphics.Rect outRect, int itemPosition, RecyclerView parent)
		{
			//TODO - work out if the rectangle is the last/first row/column
			outRect.Left = _columnSpacing;
			outRect.Right = _columnSpacing;
			outRect.Bottom = _rowSpacing;

			var rectangle = new Android.Graphics.Rect ();
			base.GetItemOffsets (rectangle, itemPosition, parent);
			if (rectangle.Top == 0) {
				outRect.Top = _rowSpacing;
			}
		}
	}
}

