using System;
using NGraphics;
using Point = NGraphics.Point;
using Size = NGraphics.Size;

namespace TwinTechsForms.NControl {
    /// <summary>
    /// Totally unnecessary, but makes some of the logic easier to read.
    /// Internal since only `ResizableSvgInsets` and `SvgImageView.RenderSvgToCanvas` is using it.
    /// </summary>
    internal enum ResizableSvgSection {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        CenterCenter,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }
    public struct ResizableSvgInsets : IEquatable<ResizableSvgInsets> {
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }

        public static ResizableSvgInsets Zero = new ResizableSvgInsets(0, 0, 0, 0);
        public ResizableSvgInsets(int top, int right, int bottom, int left) {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
        public ResizableSvgInsets(int vertical, int horizontal) : this(vertical, horizontal, vertical, horizontal) { }
        public ResizableSvgInsets(int allSides) : this(allSides, allSides, allSides, allSides) { }

        // NOTE: Returns actual section from original SVG, not what is used for the ViewBox when slicing
        //       (which seems to require the original SVG size, or a proportional scale of it for resizing).
        internal Rect GetSection(Size originalSvgSize, ResizableSvgSection section) {
            switch (section) {
                case ResizableSvgSection.TopLeft:
                    return new Rect(Point.Zero, new Size(Left, Top));
                case ResizableSvgSection.TopCenter:
                    return new Rect(new Point(Left, 0), new Size(originalSvgSize.Width - Right - Left, Top));
                case ResizableSvgSection.TopRight:
                    return new Rect(new Point(originalSvgSize.Width - Right, 0), new Size(Right, Top));
                case ResizableSvgSection.CenterLeft:
                    return new Rect(new Point(0, Top), new Size(Right, originalSvgSize.Height - Bottom - Top));
                case ResizableSvgSection.CenterCenter:
                    return new Rect(new Point(Left, Top), new Size(originalSvgSize.Width - Right - Left, originalSvgSize.Height - Bottom - Top));
                case ResizableSvgSection.CenterRight:
                    return new Rect(new Point(originalSvgSize.Width - Right, Top), new Size(Right, originalSvgSize.Height - Bottom - Top));
                case ResizableSvgSection.BottomLeft:
                    return new Rect(new Point(0, originalSvgSize.Height - Bottom), new Size(Right, Bottom));
                case ResizableSvgSection.BottomCenter:
                    return new Rect(new Point(Left, originalSvgSize.Height - Bottom), new Size(originalSvgSize.Width - Right - Left, Bottom));
                case ResizableSvgSection.BottomRight:
                    return new Rect(new Point(originalSvgSize.Width - Right, originalSvgSize.Height - Bottom), new Size(Right, Bottom));
                default:
                    throw new ArgumentOutOfRangeException("section", "Invalid resizable SVG section");
            }
        }

        internal Rect ScaleSection(Size finalImageSize, ResizableSvgSection section) {
            // TODO: Decide if the corners scale proportionally (an option with SVGs) or if they always stay the same size (current) or if it is optional.
            // TODO: Factor in `scaleCorners`, if needed.
            //public Rect ScaleSection(Size originalSvgSize, Size finalImageSize, ResizableSvgSection section, bool scaleCorners) {
            //      int horizontalScale = finalImageSize.Width / originalSvgSize.Width;
            //      int verticalScale = finalImageSize.Height / originalSvgSize.Height;
            //      Rect originalSection = GetSection(originalSvgSize, section);
            switch (section) {
                case ResizableSvgSection.TopLeft:
                    return new Rect(Point.Zero, new Size(Left, Top));
                case ResizableSvgSection.TopCenter:
                    return new Rect(new Point(Left, 0), new Size(finalImageSize.Width - Right - Left, Top));
                case ResizableSvgSection.TopRight:
                    return new Rect(new Point(finalImageSize.Width - Right, 0), new Size(Right, Top));
                case ResizableSvgSection.CenterLeft:
                    return new Rect(new Point(0, Top), new Size(Right, finalImageSize.Height - Bottom - Top));
                case ResizableSvgSection.CenterCenter:
                    return new Rect(new Point(Left, Top), new Size(finalImageSize.Width - Right - Left, finalImageSize.Height - Bottom - Top));
                case ResizableSvgSection.CenterRight:
                    return new Rect(new Point(finalImageSize.Width - Right, Top), new Size(Right, finalImageSize.Height - Bottom - Top));
                case ResizableSvgSection.BottomLeft:
                    return new Rect(new Point(0, finalImageSize.Height - Bottom), new Size(Right, Bottom));
                case ResizableSvgSection.BottomCenter:
                    return new Rect(new Point(Left, finalImageSize.Height - Bottom), new Size(finalImageSize.Width - Right - Left, Bottom));
                case ResizableSvgSection.BottomRight:
                    return new Rect(new Point(finalImageSize.Width - Right, finalImageSize.Height - Bottom), new Size(Right, Bottom));
                default:
                    throw new ArgumentOutOfRangeException("section", "Invalid resizable SVG section");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ResizableSvgInsets)) {
                return false;
            }
            return Equals((ResizableSvgInsets)obj);
        }

        public override int GetHashCode()
        {
            return (Top + Right + Bottom + Left).GetHashCode();
        }

        public static bool operator ==(ResizableSvgInsets inset1, ResizableSvgInsets inset2) 
        {
            return inset1.Equals(inset2);
        }

        public static bool operator !=(ResizableSvgInsets inset1, ResizableSvgInsets inset2) 
        {
            return !inset1.Equals(inset2);
        }

        #region IEquatable implementation
        public bool Equals(ResizableSvgInsets other)
        {
            return Top == other.Top
                && Right == other.Right
                && Bottom == other.Bottom
                && Left == other.Left;
        }
        #endregion
    }
}
