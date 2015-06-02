using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TwinTechs.Controls;

namespace TwinTechs.Example.FastCells.Cells
{
	public partial class SmallSimpleFastCell : FastCell
	{

		protected override void InitializeCell ()
		{
			InitializeComponent ();
		}

		protected override void SetupCell (bool isRecycled)
		{
			var mediaItem = BindingContext as MediaItem;
			if (mediaItem != null) {
				NameLabel.Text = mediaItem.Name;
				DescriptionLabel.Text = mediaItem.Description;
			}
		}
	}
}

