using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;

namespace TwinTechsLib.UWP.TwinTechs.UWP.Extensions
{
    public static class VisualElementRendererExtensions
    {
        public static FrameworkElement GetNativeControl(this IVisualElementRenderer renderer)
        {
            return renderer.GetType().GetProperty("Control").GetValue(renderer) as FrameworkElement;
        }

        public static FrameworkElement ConvertFormsToNative(this View view)
        {
            var renderer = Platform.CreateRenderer(view);

            view.Layout(new Xamarin.Forms.Rectangle(new Point(0, 0),
                new Size(renderer.ContainerElement.RenderSize.Width, renderer.ContainerElement.RenderSize.Height)));

            return renderer.ContainerElement;
        }
    }
}
