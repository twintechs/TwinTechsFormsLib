/*
 * Copyright (C) 2013 @JamesMontemagno http://www.montemagno.com http://www.refractored.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Ported from Xamarin Sample App
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Android.Graphics;

namespace MonoDroidToolkit.ImageLoader
{
	internal class MemoryCache
	{
		private IDictionary<String, Bitmap> m_Cache = new ConcurrentDictionary<String, Bitmap> ();

		private List<String> m_CacheList = new List<String> ();

		public void PopCache (int max)
		{
			if (max == 0)
				return;

			if (m_CacheList.Count >= max) {
				if (m_Cache.ContainsKey (m_CacheList [0]))
					m_Cache.Remove (m_CacheList [0]);

				m_CacheList.RemoveAt (0);
			}


		}

		public Bitmap Get (String id)
		{
			if (!m_Cache.ContainsKey (id))
				return null;

			return m_Cache [id];
		}

		public void Put (string id, Bitmap bitmap)
		{
			if (!m_Cache.ContainsKey (id))
				m_Cache.Add (id, bitmap);

			if (!m_CacheList.Contains (id))
				m_CacheList.Add (id);

			//if(m_CacheList.Count == 60)
			//{
			//    for(int i = 30; i >=0; i--)
			//    {
			//        m_Cache.Remove(m_CacheList[i]);
			//        m_CacheList.RemoveAt(i);
			//    }
			//}

		}

		public void Clear ()
		{
			m_Cache.Clear ();
			m_CacheList.Clear ();
		}

	}
}