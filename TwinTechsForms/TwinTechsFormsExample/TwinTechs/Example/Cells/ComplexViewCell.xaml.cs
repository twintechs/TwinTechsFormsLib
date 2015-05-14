using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TwinTechs.Example.Cells
{
	public partial class ComplexViewCell : ViewCell
	{
		public ComplexViewCell ()
		{
			InitializeComponent ();
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
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

