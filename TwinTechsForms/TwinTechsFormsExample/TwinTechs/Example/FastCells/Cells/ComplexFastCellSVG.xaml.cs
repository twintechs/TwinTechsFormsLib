using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TwinTechsForms.NControl;
using TwinTechs.Controls;
using System.Reflection;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace TwinTechs.Example.FastCells.Cells
{
	public partial class ComplexFastCellSVG : FastCell, INotifyPropertyChanged
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

				svgPic1.SvgAssembly = this.SvgAssembly;
				svgPic1.SvgPath = this.SmilePath;

				svgPic2.SvgAssembly = this.SvgAssembly;
				svgPic2.SvgPath = this.SunPath;


			}
		}


		private Assembly _svgAssembly;

		public Assembly SvgAssembly {
			get {
				if (_svgAssembly == null) {
					_svgAssembly = typeof (App).GetTypeInfo ().Assembly;
				}
				return _svgAssembly;
			}
		}

		public string SmilePath {
			get { return @"TwinTechs.TwinTechs.Example.SvgImageSample.Assets.Smile.svg"; }
		}

		public string SunPath {
			get { return @"TwinTechs.TwinTechs.Example.SvgImageSample.Assets.SunAtNight.svg"; }
		}
	





	}
}

