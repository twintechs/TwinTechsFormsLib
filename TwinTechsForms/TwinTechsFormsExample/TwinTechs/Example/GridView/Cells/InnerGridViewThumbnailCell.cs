using System;
using XLabs.Forms.Controls;
using TwinTechs.Example;
using TwinTechs.Controls;
using Xamarin.Forms;

namespace TwinTechs.Example.GridView.Cells
{
	public class InnerGridViewThumbnailCell : FastGridCell
	{
		//TODO read this from the parent grid
		const int CellWidth = 140;
		const int CellHeight = 100;

		FastImage _image;

		public Label _titleLabel { get; set; }


		public InnerGridViewThumbnailCell ()
		{
		}

		protected override void SetupCell (bool isRecycled)
		{
			if (MediaItem != null) {
				_image.ImageUrl = MediaItem.ThumbnailImagePath;
			}
			_titleLabel.Text = MediaItem != null ? MediaItem.Name : "";
		}

		protected override void InitializeCell ()
		{
			var width = CellWidth;
			var height = CellHeight;

			_titleLabel = new Label ();

			AbsoluteLayout simpleLayout = new AbsoluteLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				WidthRequest = CellWidth,
				HeightRequest = CellHeight,
			};

			_image = new FastImage () {
				Aspect = Aspect.AspectFill,
				BackgroundColor = Color.Black,
			};

			_titleLabel = new Label {
				Text = "",
				TextColor = Color.White,
				FontSize = 14,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				LineBreakMode = LineBreakMode.TailTruncation,
				BackgroundColor = Color.FromHex ("#272727"),
			};

			simpleLayout.Children.Add (_image, new Rectangle (0, 0, width, height - 30));
			simpleLayout.Children.Add (_titleLabel, new Rectangle (0, height - 30, width, 30));
			View = simpleLayout;
		}


		public MediaItem MediaItem {
			get { return (MediaItem)BindingContext; }
		}
	}
}

