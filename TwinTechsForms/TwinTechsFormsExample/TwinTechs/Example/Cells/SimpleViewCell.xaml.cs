using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TwinTechs.Example.Cells
{
	public partial class SimpleViewCell : ViewCell
	{
		public SimpleViewCell ()
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
			}
		}
	}
}

