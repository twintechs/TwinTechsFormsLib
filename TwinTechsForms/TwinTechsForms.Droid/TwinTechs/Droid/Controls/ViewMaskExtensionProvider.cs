using System;
using TwinTechs.Controls;
using System.Drawing;
using System.Runtime.InteropServices;
using Xamarin.Forms;
using TwinTechs.Droid.Extensions;
using Android.Widget;
using Android.Graphics;
using Android.Views;

namespace TwinTechs.Droid.Controls
{
	public class ShadowWrapper : Android.Widget.AbsoluteLayout
	{
		
		Paint paint;

		public ViewGroup Content { get; private set; }

		ViewGroup OriginalParent { get; set; }

		public ShadowWrapper (Android.Content.Context context, ViewGroup realContent) : base (context)
		{
			Content = realContent;
			OriginalParent = realContent.Parent as ViewGroup;
			OriginalParent.RemoveView (realContent);
			this.SetMinimumHeight (realContent.Height);
			this.SetMinimumWidth (realContent.Width);
			this.AddView (realContent, new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent,
				LinearLayout.LayoutParams.FillParent));
			OriginalParent.AddView (this, new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent,
				LinearLayout.LayoutParams.FillParent));
			this.SetBackgroundColor (Android.Graphics.Color.Red);

		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
			//TODO
		}

		public void ResetParent ()
		{
			var content = Content;
			this.RemoveView (content);
			OriginalParent.AddView (content, new LinearLayout.LayoutParams (LinearLayout.LayoutParams.FillParent,
				LinearLayout.LayoutParams.FillParent));
		}

	}

	public class ViewMaskExtensionProvider : IViewEffectExtensionProvider
	{
		public ViewMaskExtensionProvider ()
		{
		}

		#region IViewMaskExtensionProvider implementation

		public void ApplyMaskToView (Xamarin.Forms.View view, ViewMaskerType maskType)
		{
			//we need to wrap the view inside another view
		}


		public void ToggleViewShadow (Xamarin.Forms.View view, bool isOn)
		{
			var renderer = view.GetOrCreateRenderer ();
			var nativeView = renderer.ViewGroup;
			var shadowWrapper = nativeView.Parent as ShadowWrapper;

			if (shadowWrapper != null) {
				//move back to original parent
				shadowWrapper.ResetParent ();
				view.SizeChanged -= OnSizeChanged;
			}
			if (isOn) {
				shadowWrapper = new ShadowWrapper (nativeView.Context, nativeView);
				view.SizeChanged += OnSizeChanged;
			}
		}

		#endregion

		void OnSizeChanged (object sender, EventArgs e)
		{

			var view = sender as Xamarin.Forms.View;
			var renderer = view.GetOrCreateRenderer ();
			var nativeView = renderer.ViewGroup;
			var shadowWrapper = nativeView.Parent as ShadowWrapper;
			if (shadowWrapper != null) {
				
				shadowWrapper.Layout (0, 0, (int)view.Width, (int)view.Height);
			}
		}
	}
}

