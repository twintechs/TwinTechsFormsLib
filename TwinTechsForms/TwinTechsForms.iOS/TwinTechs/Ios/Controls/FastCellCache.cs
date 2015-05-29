using System;
using TwinTechs.Controls;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using UIKit;


namespace TwinTechs.Controls
{
	/// <summary>
	/// Make this class available through your favourite IOC/service method
	/// </summary>
	/// 
	public class FastCellCache : IFastCellCache
	{
		static FastCellCache _instance;

		FastCellCache ()
		{
			_cachedDataByView = new Dictionary<UIView, CachedData> ();
		}

		public static FastCellCache Instance { 
			get { 
				if (_instance == null) {
					_instance = new FastCellCache ();
				}
				return _instance; 
			} 
		}

		Dictionary<UIView,CachedData> _cachedDataByView;

		public CachedData GetCellCache (UIView parent)
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

			_cachedDataByView = new Dictionary<UIView, CachedData> ();
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
				CellItemsByCoreCells = new Dictionary<NativeCell, FastCell> ();
				OriginalBindingContextsForReusedItems = new Dictionary<FastCell, object> ();
			}

			Dictionary<NativeCell,FastCell> CellItemsByCoreCells { get; set; }

			Dictionary<FastCell,object> OriginalBindingContextsForReusedItems { get; set; }

			public void RecycleCell (NativeCell view, FastCell newCell)
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

			public bool IsCached (NativeCell view)
			{
				return CellItemsByCoreCells.ContainsKey (view);
			}

			public void CacheCell (FastCell cell, NativeCell view)
			{
				CellItemsByCoreCells [view] = cell;
				OriginalBindingContextsForReusedItems [cell] = cell.BindingContext;
			}

			public object GetBindingContextForReusedCell (FastCell cell)
			{
				if (OriginalBindingContextsForReusedItems.ContainsKey (cell)) {
					return OriginalBindingContextsForReusedItems [cell];
				} else {
					return null;
				}
			}

			void CacheBindingContextForReusedCell (FastCell cell)
			{
				OriginalBindingContextsForReusedItems [cell] = cell.BindingContext;
			}




		}
	}
}

