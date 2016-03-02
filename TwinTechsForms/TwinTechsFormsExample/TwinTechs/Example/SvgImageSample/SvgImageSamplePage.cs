using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using TwinTechsForms.NControl;

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
			Content = new ScrollView {
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
						new Label () {
							Text = "Using TwinTechsForms.SvgImage",
						},
						new Label () {
							Text = "Proportional Scaling",
						},
						new SvgImage() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							WidthRequest = 50,
							HeightRequest = 50,
						},
						new SvgImage() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							WidthRequest = 100,
							HeightRequest = 100,
						},
						new Label () {
							Text = "9-Slice Scaling",
						},
						new SvgImage() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							SvgStretchableInsets = new ResizableSvgInsets (18),
							WidthRequest = 50,
							HeightRequest = 50,
							HorizontalOptions = LayoutOptions.Start,
						},
						new SvgImage() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							SvgStretchableInsets = new ResizableSvgInsets (18),
							WidthRequest = 100,
							HeightRequest = 100,
							HorizontalOptions = LayoutOptions.Start,
						},
						new SvgImage() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							SvgStretchableInsets = new ResizableSvgInsets (18),
							WidthRequest = 300,
							HeightRequest = 300,
							HorizontalOptions = LayoutOptions.Start,
						},
						new Label () {
							Text = "Using TwinTechsForms.NControl.SvgImageView",
						},
						new Label () {
							Text = "Proportional Scaling",
						},
						new SvgImageView() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							WidthRequest = 50,
							HeightRequest = 50,
						},
						new SvgImageView() {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							WidthRequest = 100,
							HeightRequest = 100,
						},
						new Label () {
							Text = "9-Slice Scaling",
						},
						new SvgImageView () {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							SvgStretchableInsets = new TwinTechsForms.NControl.ResizableSvgInsets (18),
							WidthRequest = 50,
							HeightRequest = 50,
							HorizontalOptions = LayoutOptions.Start,
						},
						new SvgImageView () {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							SvgStretchableInsets = new TwinTechsForms.NControl.ResizableSvgInsets (18),
							WidthRequest = 100,
							HeightRequest = 100,
							HorizontalOptions = LayoutOptions.Start,
						},
						new SvgImageView () {
							SvgAssembly = typeof(App).GetTypeInfo().Assembly,
							SvgPath = "TwinTechs.TwinTechs.Example.SvgImageSample.Assets.funky-border.svg",
							SvgStretchableInsets = new TwinTechsForms.NControl.ResizableSvgInsets (18),
							WidthRequest = 300,
							HeightRequest = 300,
							HorizontalOptions = LayoutOptions.Start,
						},
					},
					BindingContext = _ViewModel,
				},
			};
			svgButton.Clicked += (sender, e) => {
				DisplayAlert("Tapped!", "SVG button tapped!", "OK");
			};
		}
	}
}
