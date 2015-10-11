using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Controls;
using TwinTechs.Example.FastCells;

namespace TwinTechs.Example.PageInPage
{
	public partial class NavigationPageInPage : ContentPage
	{
		int _index;
		NavigationPage _navigationPage;

		Page _rootPage;

		public NavigationPageInPage ()
		{
			InitializeComponent ();
			_rootPage = CreatePage (Color.Red);
			_navigationPage = new NavigationPage (_rootPage);
			PageContainer.Content = _navigationPage;

		}

		ContentPage CreatePage (Color backgroundColor)
		{
			_index++;
			var button = new Button () {
				Text = "next Page",
			};
			button.Clicked += (sender, e) => {
				var page = CreatePage (Color.Green);
				_navigationPage.PushAsync (page);
			};

			var contentPage = new ContentPage () {
				Content = new StackLayout () {
					BackgroundColor = backgroundColor,
					VerticalOptions = LayoutOptions.Fill,
					Children = {
						new Label () {
							Text = "page " + _index,
						},

					}
				}
			};
			return contentPage;
		}



	}
}

