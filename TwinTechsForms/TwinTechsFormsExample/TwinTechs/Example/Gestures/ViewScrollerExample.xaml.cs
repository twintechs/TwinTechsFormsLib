using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TwinTechs.Example.Gestures
{
	public partial class ViewScrollerExample : ContentPage
	{
		public ViewScrollerExample ()
		{
			InitializeComponent ();
		}

		void OnClicked (object o, EventArgs e)
		{
			PagerView.CurrentPageIndex++;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			PagerView.Cleanup ();
		}
	}
}

