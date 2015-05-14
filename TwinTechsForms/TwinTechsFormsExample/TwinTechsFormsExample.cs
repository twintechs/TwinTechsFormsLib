using System;

using Xamarin.Forms;
using TwinTechs.Example;

namespace TwinTechs
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new TabbedPage {
				Children = {
					new About (),
					new ViewCellSimple (),
					new ViewCellComplexSamplePage (),
					new FastCellSamplePage (),
					new FastCellComplexSamplePage (),
				}
			};
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

