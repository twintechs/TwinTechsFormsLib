using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TwinTechs.Controls;

namespace TwinTechs.Example.FastCells.Cells
{
	public partial class UnevenFastCell : FastCell
	{

		protected override void InitializeCell ()
		{
			InitializeComponent ();
		}

		protected override void SetupCell (bool isRecycled)
		{
			var mediaItem = BindingContext as UnevenSizedMediaItem;
			if (mediaItem != null) {
				NameLabel.Text = mediaItem.Name;
				DescriptionLabel.Text = " ROW HEIGHT " + mediaItem.RowHeight;

				//terrible hack to see if I can fudge in the height.
				//and no- I can not.. :(
				View.HeightRequest = mediaItem.RowHeight;
				var tableView = Parent as ListView;
				tableView.RowHeight = (int)mediaItem.RowHeight;
			}
		}
	}
}

