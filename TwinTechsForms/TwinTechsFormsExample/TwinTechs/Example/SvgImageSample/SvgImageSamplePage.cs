using System;
using System.Reflection;
using Xamarin.Forms;
using TwinTechs.Controls;

namespace TwinTechs.Example.SvgImageSample
{
	public class SvgImageSamplePage : ContentPage
	{
		readonly SvgImageSamplePageViewModel _ViewModel;
		public SvgImageSamplePage ()
		{
			_ViewModel = new SvgImageSamplePageViewModel();
			var insetLabel = new Label();
			insetLabel.SetBinding(Label.TextProperty, nameof(SvgImageSamplePageViewModel.SvgInsets), stringFormat: "Stretchable Insets: {0:C2}");
			var resourcePicker = new Picker() {
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			foreach (var resourceName in SvgImageSamplePageViewModel.AvailableResourceNames) {
				resourcePicker.Items.Add(resourceName);
			}
			resourcePicker.SetBinding(Picker.SelectedIndexProperty, nameof(SvgImageSamplePageViewModel.SvgResourceIndex), BindingMode.TwoWay);
			var insetSlider = new Slider() {
				Minimum = 0,
				Maximum = 35,
				Value = _ViewModel.AllSidesInset,
			};
			insetSlider.SetBinding(Slider.ValueProperty, nameof(SvgImageSamplePageViewModel.AllSidesInset), BindingMode.TwoWay);
			var slicingSvg = new SvgImage() {
				SvgAssembly = typeof(App).GetTypeInfo().Assembly,
				WidthRequest = 300,
				HeightRequest = 300,
			};
			slicingSvg.SetBinding(SvgImage.SvgStretchableInsetsProperty, nameof(SvgImageSamplePageViewModel.SvgInsets));
			slicingSvg.SetBinding(SvgImage.SvgPathProperty, nameof(SvgImageSamplePageViewModel.SvgResourcePath));
			var svgButton = new Button() {
				WidthRequest = 300,
				HeightRequest = 300,
				BackgroundColor = Color.Transparent,
			};

			// The root page of your application
			Title = "9-Slice SVG Scaling";
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					insetLabel,
					resourcePicker,
					insetSlider,
					new AbsoluteLayout() {
						WidthRequest = 300,
						HeightRequest = 300,
						Children = {
							slicingSvg,
							svgButton,
						},
					},
				},
				BindingContext = _ViewModel,
			};
			svgButton.Clicked += (sender, e) => {
				DisplayAlert("Tapped!", "SVG button tapped!", "OK");
			};
		}
	}
}
