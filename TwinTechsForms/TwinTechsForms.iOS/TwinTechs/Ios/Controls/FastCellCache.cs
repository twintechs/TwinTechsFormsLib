using System;
using TwinTechs.Controls;

namespace TwinTechs.Ios.Controls
{
	public class FastCellCache : IFastCellCache
	{
		
		static FastCellCache _instance;

		public static FastCellCache Instance { 
			get { 
				if (_instance == null) {
					_instance = new FastCellCache ();
				}
				return _instance; 
			} 
		}

		public FastCellCache ()
		{
		}

		#region IFastCellCache implementation

		public void FlushAllCaches ()
		{
		}

		#endregion
	}
}

