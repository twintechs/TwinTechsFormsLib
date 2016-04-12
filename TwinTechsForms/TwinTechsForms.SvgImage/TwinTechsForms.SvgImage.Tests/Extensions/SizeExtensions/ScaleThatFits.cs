using NUnit.Framework;
using NGraphics;
using TwinTechs.Extensions;

namespace TwinTechsForms.SvgImage.Tests
{
	[TestFixture]
	public class ScaleThatFits
	{
		[Test]
		[TestCase(0, 50, 1d)]
		[TestCase(50, 0, 1d)]
		[TestCase(-5, 50, 1d)]
		[TestCase(50, -5, 1d)]
		[TestCase(double.Epsilon, 50, 1d)]
		[TestCase(50, double.Epsilon, 1d)]
		public void VariousInvalids_ReturnsInvalid (double width, double height, double expectedScale)
		{
			var original = new Size (width, height);
			var max = new Size (500, 500);

			var result = original.ScaleThatFits (max);

			Assert.AreEqual (expectedScale, result);
		}

		[Test]
		[TestCase(100, 50, 50, 50, 0.5d)]
		[TestCase(50, 100, 50, 50, 0.5d)]
		[TestCase(50, 100, 25, 25, 0.25d)]
		[TestCase(100, 50, 25, 25, 0.25d)]
		public void SmallerOutput_ScalesDownToLargestOriginal (double originalWidth, double originalHeight, double maxWidth, double maxHeight, double expectedScale)
		{
			var original = new Size (originalWidth, originalHeight);
			var max = new Size (maxWidth, maxHeight);

			var result = original.ScaleThatFits (max);

			Assert.AreEqual (expectedScale, result);
		}

		[Test]
		[TestCase(100, 50, 200, 200, 2d)]
		[TestCase(50, 100, 200, 200, 2d)]
		[TestCase(50, 100, 400, 400, 4d)]
		[TestCase(100, 50, 400, 400, 4d)]
		public void LargerOutput_ScalesUpToLargestOriginal (double originalWidth, double originalHeight, double maxWidth, double maxHeight, double expectedScale)
		{
			var original = new Size (originalWidth, originalHeight);
			var max = new Size (maxWidth, maxHeight);

			var result = original.ScaleThatFits (max);

			Assert.AreEqual (expectedScale, result);
		}
	}

	[TestFixture]
	public class ScaleThatFills
	{
		[Test]
		[TestCase(0, 50, 1d)]
		[TestCase(50, 0, 1d)]
		[TestCase(-5, 50, 1d)]
		[TestCase(50, -5, 1d)]
		[TestCase(double.Epsilon, 50, 1d)]
		[TestCase(50, double.Epsilon, 1d)]
		public void VariousInvalids_ReturnsInvalid (double width, double height, double expectedScale)
		{
			var original = new Size (width, height);
			var max = new Size (500, 500);

			var result = original.ScaleThatFills (max);

			Assert.AreEqual (expectedScale, result);
		}

		[Test]
		[TestCase(80, 20, 10, 10, 0.5d)]
		[TestCase(20, 80, 10, 10, 0.5d)]
		[TestCase(40, 80, 5, 5, 0.25d)]
		[TestCase(80, 40, 5, 5, 0.25d)]
		[TestCase(100, 150, 50, 100, 0.5d)]
		public void SmallerOutput_ScalesDownToSmallestOriginal (double originalWidth, double originalHeight, double maxWidth, double maxHeight, double expectedScale)
		{
			var original = new Size (originalWidth, originalHeight);
			var max = new Size (maxWidth, maxHeight);

			var result = original.ScaleThatFills (max);

			Assert.AreEqual (expectedScale, result);
		}

		[Test]
		[TestCase(100, 150, 200, 200, 2d)]
		[TestCase(150, 100, 200, 200, 2d)]
		[TestCase(100, 150, 400, 400, 4d)]
		[TestCase(150, 100, 400, 400, 4d)]
		[TestCase(150, 100, 50, 100, 1d)]
		public void LargerOutput_ScalesUpToSmallestOriginal (double originalWidth, double originalHeight, double maxWidth, double maxHeight, double expectedScale)
		{
			var original = new Size (originalWidth, originalHeight);
			var max = new Size (maxWidth, maxHeight);

			var result = original.ScaleThatFills (max);

			Assert.AreEqual (expectedScale, result);
		}
	}
}
