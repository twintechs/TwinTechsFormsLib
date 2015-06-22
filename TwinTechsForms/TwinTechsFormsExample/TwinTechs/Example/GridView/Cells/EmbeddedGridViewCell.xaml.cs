using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Example;
using System.Collections.ObjectModel;
using XLabs.Forms.Controls;
using TwinTechs.Extensions;

namespace TwinTechs.Example.GridView.Cells
{
	public partial class EmbeddedGridViewCell : FastGridCell
	{
		public EmbeddedGridViewCell ()
		{
		}

		protected override void InitializeCell ()
		{
			InitializeComponent ();
		}

		protected override void SetupCell (bool isRecycled)
		{
			var items = BindingContext as ObservableCollection<MediaItem>;
			ItemsGrid.Layout (new Rectangle (0, 0, CellSize.Width, CellSize.Height));
			ItemsGrid.ScrollToItemWithIndex (0, false);
			ItemsGrid.ItemsSource = items;
		}


		void OnItemSelected (object sender, XLabs.GridEventArgs<object> e)
		{
			var item = e.Value as MediaItem;
			this.ParentView.GetParentPage ().DisplayAlert ("you selected an item", item.Name, "Ok");
		}

		public override void OnSizeChanged (Size size)
		{
			base.OnSizeChanged (size);
			ItemsGrid.Layout (new Rectangle (0, 0, size.Width, size.Height));
		}
	}
}

