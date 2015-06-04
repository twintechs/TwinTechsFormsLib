using System;
using TwinTechs.Controls;
using System.Collections.Generic;
using Android.Views;
using System.Reflection;
using Xamarin.Forms;


//TODO temporary slapped up thing.
using XLabs.Forms.Controls;


namespace TwinTechs.Droid.Controls
{
	/// <summary>
	/// Make this class available through your favourite IOC/service method
	/// </summary>
	/// 
	public class FastGridCellCache : IFastCellCache
	{
		static FastGridCellCache _instance;

		FastGridCellCache ()
		{
			_cachedDataByView = new Dictionary<Android.Views.View, CachedData> ();
		}

		public static FastGridCellCache Instance { 
			get { 
				if (_instance == null) {
					_instance = new FastGridCellCache ();
				}
				return _instance; 
			} 
		}

		Dictionary<Android.Views.View,CachedData> _cachedDataByView;

		public CachedData GetCellCache (ViewGroup parent)
		{
			if (!_cachedDataByView.ContainsKey (parent)) {
				_cachedDataByView [parent] = new CachedData ();
			}
			return _cachedDataByView [parent];
		}

		#region IFastCellCache impl

		//TODO maintain mapping of cell to original?

		public void FlushAllCaches ()
		{
			foreach (var cachedData in _cachedDataByView.Values) {
				cachedData.Reset ();
			}

			_cachedDataByView = new Dictionary<Android.Views.View, CachedData> ();
		}

		#endregion

		public class CachedData
		{
			internal CachedData ()
			{
				Reset ();
			}

			/// <summary>
			/// Reset this instance. 
			/// </summary>
			internal void Reset ()
			{
				CellItemsByCoreCells = new Dictionary<Android.Views.View, FastGridCell> ();
				OriginalBindingContextsForReusedItems = new Dictionary<FastGridCell, object> ();
			}

			Dictionary<Android.Views.View,FastGridCell> CellItemsByCoreCells { get; set; }

			Dictionary<FastGridCell,object> OriginalBindingContextsForReusedItems { get; set; }

			public void RecycleCell (Android.Views.View view, FastGridCell newCell)
			{
				if (CellItemsByCoreCells.ContainsKey (view)) {
					var reusedItem = CellItemsByCoreCells [view];
					if (OriginalBindingContextsForReusedItems.ContainsKey (newCell)) {
						reusedItem.BindingContext = OriginalBindingContextsForReusedItems [newCell];
					} else {
						reusedItem.BindingContext = newCell.BindingContext;
					}
				}
			}

			public bool IsCached (Android.Views.View view)
			{
				return CellItemsByCoreCells.ContainsKey (view);
			}

			public void CacheCell (FastGridCell cell, Android.Views.View view)
			{
				CellItemsByCoreCells [view] = cell;
				OriginalBindingContextsForReusedItems [cell] = cell.BindingContext;
			}

			public object GetBindingContextForReusedCell (FastGridCell cell)
			{
				if (OriginalBindingContextsForReusedItems.ContainsKey (cell)) {
					return OriginalBindingContextsForReusedItems [cell];
				} else {
					return null;
				}
			}

			void CacheBindingContextForReusedCell (FastGridCell cell)
			{
				OriginalBindingContextsForReusedItems [cell] = cell.BindingContext;
			}




		}
	}
}

