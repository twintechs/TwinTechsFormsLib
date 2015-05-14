using System;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace TwinTechs.Example
{
	public static class DataProvider
	{
		/// <summary>
		/// Gets the media items. - some canned data
		/// </summary>
		/// <returns>The media items.</returns>
		public static ObservableCollection<MediaItem> GetMediaItems ()
		{
			var imagePaths = new String[] {
				"http://www.infragistics.com/community/cfs-file.ashx/__key/CommunityServer.Blogs.Components.WeblogFiles/brent_5F00_schooley.metablogapi/6521.evolve_2D00_th_5F00_2FC8A2E4.jpg",
				"https://www.google.com/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&cad=rja&uact=8&ved=&url=http%3A%2F%2Fwww.hanselman.com%2Fblog%2FXamarinEvolve2013TalkVideoHowCSavedMyMarriage.aspx&ei=uu5IVcrSLsmrgwTNvIHQAg&bvm=bv.92291466,d.eXY&psig=AFQjCNFXgQ4M3FZzJn3mlB3UeqqmUsqYZw&ust=1430929467200256",
				"http://www.infragistics.com/community/cfs-file.ashx/__key/CommunityServer.Blogs.Components.WeblogFiles/nick-landry.metablogapi/5545.XamarinEvolveBannerNick_5F00_26AE7CBA.png",
				"http://blog.xamarin.com/wp-content/uploads/2013/05/xamarin-stage.jpg",
				"http://blog.xamarin.com/wp-content/uploads/2012/11/Austin-Conv-Center1-300x232.jpg",
				"https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcR9Bf0UonMZGYjDzrUAuFlYLp0nFSiyyCtN_tSnX_ly4MsDv-qm",
				"https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcQ4YlPrkvqRk4SzwOlKqazpKHfQTsjX8LPNc68KdKKOxOe1gTZ6",
				"http://i.ytimg.com/vi/SKEdFsLKH-U/maxresdefault.jpg",
				"http://i.ytimg.com/vi/v9vEhwomUEY/maxresdefault.jpg",
				"http://i.ytimg.com/vi/rBKxG67gAqE/maxresdefault.jpg",
				"http://blog.xamarin.com/wp-content/uploads/2013/02/josh-speaks-300x200.jpg",
				"http://i.ytimg.com/vi/GK9h71vGkpQ/maxresdefault.jpg",
				"http://www.mirskayamusicstudio.com/images/2007SrRecital.jpg",
				"http://i.ytimg.com/vi/aVmeb7im6M0/maxresdefault.jpg",
			};

			var thumbnailPaths = new String[] {
				"https://www.cambridgenetwork.co.uk/public/news/thumbnail_59902__suffolk-tap.png",
				"http://datateca.unad.edu.co/contenidos/551059/OVAS/lecc4-scorm/story_content/thumbnail.jpg",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcTbXpym6R2tgnAeKC1soKmEfHP4R5UFK9k9mixksHFT_n2uwlfBnA",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcSBuOuHm62yob2nL8DIWwWeOrI7kmnFuURCtLY0H53Z9Uca9bME",
				"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTTRtBIFU25jPkjSbYPOd244ft99WC2-_mrH3FbkRgnZM6rpdDD",
				"https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcSXtrv2J733DzB779jniZEtncYJ5zyU-ovyapTOrxRRQKHWWlXw5Q",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQQ1Tgn4cRdvvxmyblIJt9_kqb2Vuzr7v51l0u0I7L7Ef4OsbIUcQ",
				"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT3Di56JKw-IvOKFLJqgkVo9g4LeZrA2ZZFGq5t1-QR_5T45kNC",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQFm2AtW1KzyWPpa2iGeopWpGPGN0LeevSJanO9SJO0bRyTYuIs",
				"https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcRgvmWx5KtJQBCyW5kTfE3SO6Q3_ML1reqCPcV3qjLcV8Yz_eHmXA",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcT_giyE73DP5fX2FxnluI1B6f9ljmFMTSIMMxeTEV0S0429z5yH",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcRPdi-AFq6Aln92RhhyNmZCoECy2p_FFEP2aGhhQd2AFWjLhhH6",
				"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ032CaSsv6sbufx_7FO8Mr-rWPTOXcVzmyhudLh7i09MRLwVMY1w",
				"https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcTSKDEx6X-Dq90nSFZS7FBE3OplihNPU8L38h5yMde815uVUVwHTg",
			};
			var imageNames = new String[] {
				"xaml is fast!",
				"an interesting name",
				"somethign I thought about",
				"too quick for me",
				"that's really nice",
				"slow down!",
				"move too fast",
				"got to make",
				"the moment last",
				"kicking around",
				"the cobbled stone",
				"looking for fun",
				"and feeling",
				"groovy"
			};


			var userNames = new String[] {
				"bob",
				"shirly",
				"ethel",
				"jack",
				"virginia",
				"erik",
				"rusty",
				"mickey",
				"Falcon",
				"Henry",
				"Lazarus",
				"Jackie",
				"Ashley",
				"Penelope",
				"Heather"
			};

			ObservableCollection<MediaItem> items = new ObservableCollection<MediaItem> ();

			for (int index = 0; index < 10; index++) {
				for (int i = 0; i < imagePaths.Length; i++) {
					var item = new MediaItem (imageNames [i], imageNames [i], userNames [i], imagePaths [i], imagePaths [i], thumbnailPaths [i]);
					items.Add (item);
				}
					
			}
			return items;
		}
	}
}


