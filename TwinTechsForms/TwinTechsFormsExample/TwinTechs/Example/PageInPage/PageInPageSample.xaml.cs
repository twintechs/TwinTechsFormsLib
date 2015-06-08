using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TwinTechs.Example.PageInPage
{
	public partial class PageInPageSample : ContentPage
	{
		public PageInPageSample ()
		{
			InitializeComponent ();
		}

		void OnPage1 (object s, EventArgs ev)
		{
			var page = CreatePage (1, Color.Blue);
			PageContainer.Content = page;
		}

		void OnPage2 (object s, EventArgs ev)
		{
			var page = CreatePage (2, Color.Yellow);
			PageContainer.Content = page;
		}

		void OnPage3 (object s, EventArgs ev)
		{
			var page = CreatePage (3, Color.Green);
			PageContainer.Content = page;
		}


		ContentPage CreatePage (int index, Color backgroundColor)
		{
			var page = new ContentPage () {
				Content = new StackLayout () {
					BackgroundColor = backgroundColor,
					VerticalOptions = LayoutOptions.Fill,
					Children = {
						new Label () {
							Text = "page " + index,
						},
					}
				}
			};
			return page;
		}



	}
}

