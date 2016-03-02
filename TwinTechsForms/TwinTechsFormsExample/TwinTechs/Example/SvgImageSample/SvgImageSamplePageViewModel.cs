using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using TwinTechs.Controls;

namespace TwinTechs.Example.SvgImageSample
{
	public class SvgImageSamplePageViewModel : INotifyPropertyChanged {
		int _AllSidesInset;
		public int AllSidesInset {
			get { return _AllSidesInset; }
			set {
				if (value != _AllSidesInset) {
					_AllSidesInset = value;
					_SvgInsets = new ResizableSvgInsets(AllSidesInset);
					if (PropertyChanged != null) {
						PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AllSidesInset)));
						PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SvgInsets)));
					}
				}
			}
		}
		ResizableSvgInsets _SvgInsets;
		public ResizableSvgInsets SvgInsets {
			get {
				return _SvgInsets;
			}
		}
		int _SvgResourceIndex = 0;
		public int SvgResourceIndex {
			get { return _SvgResourceIndex; }
			set {
				if (value != _SvgResourceIndex) {
					_SvgResourceIndex = value;
					if (PropertyChanged != null) {
						PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SvgResourceIndex)));
						PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SvgResourcePath)));
					}
				}
			}
		}
		public string SvgResourcePath {
			get { return _AvailableResources[_SvgResourceIndex].Value; }
		}
		static public string[] AvailableResourceNames {
			get {
				return _AvailableResources.Select(kvp => kvp.Key).ToArray();
			}
		}

		// Since PCLs don't get `Assembly.GetCallingAssembly`, we're doing this manually.
		static readonly List<KeyValuePair<string, string>> _AvailableResources = new[] {
			"twintechs-logo",
			"test-button",
			"ErulisseuiinSpaceshipPack",
			"MocastIcon",
			"repeat",
			"sliderThumb",
			"Smile",
			"SunAtNight",
			"TextVariations",
			"mozilla.BezierCurves1",
			"mozilla.BezierCurves2",
			"mozilla.ellipse",
			"mozilla.path",
			"mozilla.Text1",
			"mozilla.Text2",
			"mozilla.Text3",
			"mozilla.Text4",
			"mozilla.transform",
			"funky-border",
		}.ToDictionary(name => name, name => $"TwinTechs.TwinTechs.Example.SvgImageSample.Assets.{name}.svg").ToList();

		public SvgImageSamplePageViewModel() {
			AllSidesInset = 0;
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged = delegate { };
		#endregion
	}
	
}
