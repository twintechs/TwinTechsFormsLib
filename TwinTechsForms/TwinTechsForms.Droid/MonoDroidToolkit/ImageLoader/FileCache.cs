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
using Android.Content;
using Java.IO;

namespace MonoDroidToolkit.ImageLoader
{
	public class FileCache
	{
		private File m_CacheDir;

		public FileCache (Context context)
		{
			string stuff = Android.OS.Environment.ExternalStorageState;
			if (Android.OS.Environment.ExternalStorageState.Equals (Android.OS.Environment.MediaMounted)) {
				m_CacheDir = new File (Android.OS.Environment.ExternalStorageDirectory, "Android/data/" + context.ApplicationContext.PackageName);
				m_CacheDir = context.ExternalCacheDir;
			} else {
				m_CacheDir = context.CacheDir;
			}

			if (m_CacheDir == null)
				m_CacheDir = context.CacheDir;




			if (!m_CacheDir.Exists ()) {
				var success = m_CacheDir.Mkdirs ();
			}
		}

		public File GetFile (string url)
		{
			var fileName = url.GetHashCode ().ToString ();
			var file = new File (m_CacheDir, fileName);
			return file;
		}

		public void Clear ()
		{
			File[] files = m_CacheDir.ListFiles ();
			if (files == null)
				return;

			foreach (var file in files) {
				try {
					file.Delete ();
				} catch (Exception) {
					//TODO log exception
				}
			}
		}


	}
}