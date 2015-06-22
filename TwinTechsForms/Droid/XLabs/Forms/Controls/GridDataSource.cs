using System;
using Android.Widget;
using Android.Content;
using Android.Views;

namespace XLabs.Forms.Controls
{

	public class GridDataSource : BaseAdapter
	{
		Context _context;

		public delegate global::Android.Views.View OnGetCell (int position, global::Android.Views.View convertView, ViewGroup parent);

		public delegate int OnRowsInSection ();

		private readonly OnGetCell _onGetCell;
		private readonly OnRowsInSection _onRowsInSection;

		public GridDataSource (OnGetCell onGetCell, OnRowsInSection onRowsInSection)
		{
			this._onGetCell = onGetCell;
			this._onRowsInSection = onRowsInSection;
		}

		public GridDataSource (Context c)
		{
			_context = c;
		}

		public override int Count {
			get { return _onRowsInSection (); }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override global::Android.Views.View GetView (int position, global::Android.Views.View convertView, ViewGroup parent)
		{
			return _onGetCell (position, convertView, parent);
		}

	}


}

