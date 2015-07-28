using System;

namespace TwinTechs.Example
{
	public class UnevenSizedMediaItem : MediaItem
	{
		public UnevenSizedMediaItem (string name, string imageName, string userName, string description, string imagePath, string thumbnailImagePath, int counter) : base (name, imageName, userName, description, imagePath, thumbnailImagePath, counter)
		{
		}

		public double RowHeight {
			get;
			set;
		}
	}
}

