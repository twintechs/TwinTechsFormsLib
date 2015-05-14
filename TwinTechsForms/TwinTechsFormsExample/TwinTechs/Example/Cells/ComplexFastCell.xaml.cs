using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TwinTechs.Controls;

namespace TwinTechs.Example.Cells
{
	public partial class ComplexFastCell : FastCell
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
				UserThumbnailView2.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				UserThumbnailView3.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				UserThumbnailView4.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				UserThumbnailView5.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				UserThumbnailView6.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
				UserThumbnailView7.ImageUrl = mediaItem.ThumbnailImagePath ?? "";
			}
		}
	}
}

