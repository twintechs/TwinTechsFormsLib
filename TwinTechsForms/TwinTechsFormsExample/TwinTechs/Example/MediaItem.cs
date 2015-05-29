using System;

namespace TwinTechs.Example
{
	public class MediaItem
	{
		public string Name { get; set; }

		public string ImageName { get; set; }

		public string UserName { get; set; }

		public string Description { get; set; }

		public string ImagePath { get; set; }

		public string ThumbnailImagePath { get; set; }

		public MediaItem (string name, string imageName, string userName, string description, string imagePath, string thumbnailImagePath, int counter)
		{
//			this.Name = name;
			this.ImageName = imageName;
			this.UserName = userName;
			this.Description = description;
			this.ImagePath = imagePath;
			this.ThumbnailImagePath = thumbnailImagePath;
			this.Name = counter.ToString ();
		}

		public override string ToString ()
		{
			return string.Format ("[MediaItem: Name={0}]", Name);
		}
		
		
	}
}

