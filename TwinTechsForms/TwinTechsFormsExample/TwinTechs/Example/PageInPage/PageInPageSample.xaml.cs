using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Controls;
using TwinTechs.Example.FastCells;

namespace TwinTechs.Example.PageInPage
{
	public partial class PageInPageSample : ContentPage
	{
		Page page1;
		Page page2;
		Page page3;

		bool _toggleReuse;

		public PageInPageSample ()
		{
			InitializeComponent ();
		}

		void OnPage1 (object s, EventArgs ev)
		{
			var page = page1 == null ? CreatePage (1, Color.Blue) : page1;
			PageContainer.Content = page;
			if (_toggleReuse) {
				page1 = page;
			}
		}

		void OnPage2 (object s, EventArgs ev)
		{
			var page = page2 == null ? new FastSimpleCellLots () : page2;
			PageContainer.Content = page;
			if (_toggleReuse) {
				page2 = page;
			}
		}

		void OnPage3 (object s, EventArgs ev)
		{
			var page = page3 == null ? new SampleMenu () : page3;
			PageContainer.Content = page;
			if (_toggleReuse) {
				page3 = page;
			}
		}

		void OnToggleReuse (object s, EventArgs ev)
		{
			_toggleReuse = !_toggleReuse;
			if (!_toggleReuse) {
				page1 = null;
				page2 = null;
				page3 = null;
			}
			var button = s as Button;
			button.Text = _toggleReuse ? "reuse pages off" : "reuse pages on";
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

