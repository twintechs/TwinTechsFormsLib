using System;
using Xamarin.Forms;

namespace TwinTechs.Controls
{
	//specialized class for showing a page within a page
	public class PageViewContainer : View
	{
		public PageViewContainer ()
		{
		}

		public static readonly BindableProperty ContentProperty = BindableProperty.Create<PageViewContainer,Page> (s => s.Content, null);

		public Page Content {
			get{ return (Page)GetValue (ContentProperty); }
			set{ SetValue (ContentProperty, value); }
		}
	}
}

