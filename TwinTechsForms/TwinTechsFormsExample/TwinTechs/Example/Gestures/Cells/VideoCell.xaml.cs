using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TwinTechs.Controls;
using TwinTechs.Gestures;
using System.Diagnostics;

namespace TwinTechs.Example.Gestures.Cells
{
	public partial class VideoCell : FastCell
	{
		protected override void InitializeCell ()
		{
			InitializeComponent ();
			ForegroundContent.ProcessGestureRecognizers ();
		}

		protected override void SetupCell (bool isRecycled)
		{
			var mediaItem = BindingContext as MediaItem;
			if (mediaItem != null) {
				UserThumbnailView.ImageUrl = mediaItem.ImagePath ?? "";
				NameLabel.Text = mediaItem.Name + " An item with a pannable content overlay";
			}
		}
	}
}

