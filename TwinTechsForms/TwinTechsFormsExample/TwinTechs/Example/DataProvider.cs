using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace TwinTechs.Example
{
	public static class DataProvider
	{
		/// <summary> 		/// Gets the media items. - some canned data 		/// </summary> 		/// <returns>The media items.</returns>         public static ObservableCollection<MediaItem> GetMediaItems () 		{
			//	Random images as placeholders from https://unsplash.it
			//	URL Syntax:
			//	https://unsplash.it/<width>/<height>/?random  
			//	Example:
			//	https://unsplash.it/200/300/?random
			//  You also need to add an additional random number to the URL to avoid the browser cache returning always the same image:
			//	https://unsplash.it/1920/1280/?random&num=<some random number>



			Random rand = new Random ();  			var imagePaths = new String [] { 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 			};     			var thumbnailPaths = new String [] { 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 				"https://unsplash.it/1920/1280/?random&num=" + rand.Next(), 			};      			var imageNames = new String [] { 				"xaml is fast!", 				"an interesting name", 				"somethign I thought about", 				"too quick for me", 				"that's really nice", 				"slow down!", 				"move too fast", 				"got to make", 				"the moment last", 				"kicking around", 				"the cobbled stone", 				"looking for fun", 				"and feeling", 				"groovy" 			};   			var userNames = new String [] { 				"bob", 				"shirly", 				"ethel", 				"jack", 				"virginia", 				"erik", 				"rusty", 				"mickey", 				"Falcon", 				"Henry", 				"Lazarus", 				"Jackie", 				"Ashley", 				"Penelope", 				"Heather" 			};  			ObservableCollection<MediaItem> items = new ObservableCollection<MediaItem> ();  			int counter = 0; 			for (int index = 0; index < 20; index++) { 				for (int i = 0; i < imagePaths.Length; i++) { 					var item = new MediaItem (imageNames [i], imageNames [i], userNames [i], imagePaths [i], imagePaths [i], thumbnailPaths [i], counter); 					items.Add (item); 					counter++; 				}

			} 			return items; 		}
	}
}


