using System;
using Android.Support.V7.Widget;
using System.Collections.ObjectModel;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Android.Content.Res;
using Android.Util;

namespace XLabs.Forms.Controls
{
	public class GridViewAdapter : RecyclerView.Adapter
	{
		RecyclerView _recyclerView;
		IEnumerable _items;

		DisplayMetrics _displayMetrics;

		GridView Element { get; set; }

		public IEnumerable Items {
			get {
				return _items;
			}
			set {
				var oldColleciton = _items as INotifyCollectionChanged;
				if (oldColleciton != null) {
					oldColleciton.CollectionChanged -= NewColleciton_CollectionChanged;
				}
				_items = value;
				var newColleciton = _items as INotifyCollectionChanged;
				if (newColleciton != null) {
					newColleciton.CollectionChanged += NewColleciton_CollectionChanged;
				}
				//TODO add remove change listeners
			}
			
		}

		void NewColleciton_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null) {
				//TODO handle changes here, reload data etc.. if required.. 
				//or brute force.. or something else
				foreach (var item in e.NewItems) {
					var index = IndexOf (_items, item);
					NotifyItemInserted (index);
				}
			}
			if (e.OldItems != null) {
				foreach (var item in e.OldItems) {
					var index = IndexOf (_items, item);
					NotifyItemRemoved (index);
				}
			}
		}

		public GridViewAdapter (IEnumerable items, RecyclerView recyclerView, GridView gridView, DisplayMetrics displayMetrics)
		{
			Items = items;
			_recyclerView = recyclerView;
			Element = gridView;
			_displayMetrics = displayMetrics;
		}

		public class GridViewCell : RecyclerView.ViewHolder
		{
			public GridViewCellContainer ViewCellContainer { get; set; }

			public GridViewCell (GridViewCellContainer view) : base (view)
			{
				ViewCellContainer = view;
			}

			//			public Android.Views.View ViewCellContainer { get; set; }
			//
			//			public GridViewCell (Android.Views.View view) : base (view)
			//			{
			//				ViewCellContainer = view;
			//			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			var gridViewCell = Element.ItemTemplate.CreateContent () as FastGridCell;
			var initialCellSize = new Xamarin.Forms.Size (Element.ItemWidth, Element.ItemHeight);
			var view = new GridViewCellContainer (parent.Context, gridViewCell, parent, initialCellSize);
			view.Click += mMainView_Click;

			var width = Convert.ToInt32 (Element.ItemWidth);
			var height = Convert.ToInt32 (Element.ItemHeight);
			var dpW = ConvertDpToPixels (width);
			var dpH = ConvertDpToPixels (height);
			view.LayoutParameters = new  Android.Widget.GridView.LayoutParams (dpW, dpH);
			GridViewCell myView = new GridViewCell (view);
			return myView;
		}

		private int ConvertDpToPixels (float dpValue)
		{
			var pixels = (int)((dpValue) * _displayMetrics.Density);
			return pixels;
		}

		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			GridViewCell myHolder = holder as GridViewCell;
			var item = Items.Cast<object> ().ElementAt (position);
			myHolder.ViewCellContainer.Update (item);

		}

		void mMainView_Click (object sender, EventArgs e)
		{
			int position = _recyclerView.GetChildPosition ((Android.Views.View)sender);
			var item = Items.Cast<object> ().ElementAt (position);
			Console.WriteLine (item);
		}

		public override int ItemCount {
			
			get {
				var count = (Items as ICollection) != null ? (Items as ICollection).Count : 0;
				return count;
			}

		}


		public static int IndexOf (IEnumerable collection, object element, IEqualityComparer comparer = null)
		{
			int i = 0;
			comparer = comparer ?? EqualityComparer<object>.Default;
			foreach (var currentElement in collection) {
				if (comparer.Equals (currentElement, element)) {
					return i;
				}

				i++;
			}

			return -1;
		}

	}
}

