// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
	
	public class FilterManager
	{
		private static Dictionary<string, IResponseFilter> DataList = new Dictionary<string, IResponseFilter>();

		public static IResponseFilter CreateFilter(string guid)
		{
			lock (DataList)
			{
				var filter = new TestImageFilter();
                DataList.Add(guid, filter);

				return filter;
			}
		}

		public static IResponseFilter GetFileter(string guid)
		{
			lock (DataList)
			{
				try
				{
					return DataList[guid];
				}
				catch (System.Collections.Generic.KeyNotFoundException e)
				{

					return null;
				}
			}
		}
	}	
	
}
