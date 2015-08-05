using System;
using Xamarin.Forms;

namespace TwinTechs.Extensions
{
	public static class PointExtensions
	{
		/// <summary>
		/// more readable point for debugging
		/// </summary>
		/// <returns>point with 2 decimal places.</returns>
		/// <param name="point">Point.</param>
		public static string PrettyPrint (this Point point)
		{
			return string.Format ("[{0:0.0},{1:0.0}]", point.X, point.Y);
		}

	}
}

