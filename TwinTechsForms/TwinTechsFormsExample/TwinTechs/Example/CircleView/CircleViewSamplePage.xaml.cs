using System;
using System.Collections.Generic;

using Xamarin.Forms;
using TwinTechs.Controls;
using TwinTechs.Example.FastCells;

namespace TwinTechs.Example.CircleView
{
	public partial class CircleViewSamplePage : ContentPage
	{
		bool _isShadowOn = false;
		bool _applyToLayout = false;

		public CircleViewSamplePage ()
		{
			InitializeComponent ();
		}

		void OnClickedToggleButton (object sender, EventArgs ev)
		{
			_applyToLayout = !_applyToLayout;
			ApplyToLayoutButton.Text = _applyToLayout ? "Apply To Layout" : "Apply To Children";
		}

		void OnToggleShadow (object sender, EventArgs ev)
		{
			_isShadowOn = !_isShadowOn;
			ExampleBoxView.ToggleShadow (_isShadowOn);
			ButtonView.ToggleShadow (_isShadowOn);
			LabelView.ToggleShadow (_isShadowOn);
			ExampleImageview.ToggleShadow (_isShadowOn);
		}

		void OnClickedSelectionButton (object sender, EventArgs ev)
		{
			var index = Array.IndexOf (new Button[]{ NoneButton, CircleButton, TriangleButton, SquareButton }, sender);
			var type = (ViewMaskerType)index;
			if (_applyToLayout) {
				Stack.ApplyMask (type);
			} else {
				ExampleBoxView.ApplyMask (type);
				ButtonView.ApplyMask (type);
				LabelView.ApplyMask (type);
				ExampleImageview.ApplyMask (type);
			}
		}

	}
}

