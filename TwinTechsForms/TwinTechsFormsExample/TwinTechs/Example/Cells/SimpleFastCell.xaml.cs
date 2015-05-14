using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TwinTechs.Controls;

namespace TwinTechs.Example.Cells
{
	public partial class SimpleFastCell : FastCell
	{

		protected override void InitializeCell ()
		{
			InitializeComponent ();
		}

		protected override void SetupCell (bool isRecycled)
		{
			var mediaItem = BindingContext as MediaItem;
			if (mediaItem != null) {
				UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
				ImageView.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				NameLabel.Text = mediaItem.Name;
				DescriptionLabel.Text = mediaItem.Description;
			}
		}
	}
}

