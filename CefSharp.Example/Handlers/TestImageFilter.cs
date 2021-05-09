// Copyright © 2012 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CefSharp.Handler;

namespace CefSharp.Example.Handlers
{
	
public class TestImageFilter : IResponseFilter  
    {  
        public event Action<byte[]> NotifyData;  
        private int contentLength = 0;  
        public List<byte> dataAll = new List<byte>();  

        public void SetContentLength(int contentLength)  
        {  
            this.contentLength = contentLength;  
        }  

        public FilterStatus Filter(System.IO.Stream dataIn, out long dataInRead, System.IO.Stream dataOut, out long dataOutWritten)  
        {  
            try  
            {  
                if (dataIn == null)  
                {
                    Debug.WriteLine("Debug dataIn filter=null");
                    dataInRead = 0;  
                    dataOutWritten = 0;  

                    return FilterStatus.Done;  
                }  

                dataInRead = dataIn.Length;
                Debug.WriteLine("Debug dataInRead filter={0}", dataInRead);
                dataOutWritten = Math.Min(dataInRead, dataOut.Length);  

                dataIn.CopyTo(dataOut);  
                dataIn.Seek(0, SeekOrigin.Begin);  
                byte[] bs = new byte[dataIn.Length];  
                dataIn.Read(bs, 0, bs.Length);  
                dataAll.AddRange(bs);  

                if (dataAll.Count == this.contentLength)  
                {  
                    // Notification via here  
                    NotifyData(dataAll.ToArray());  

                    return FilterStatus.Done;  
                }  
                else if (dataAll.Count < this.contentLength)  
                {  
                    dataInRead = dataIn.Length;  
                    dataOutWritten = dataIn.Length;  

                    return FilterStatus.NeedMoreData;  
                }  
                else  
                {  
                    return FilterStatus.Error;  
                }  
            }  
            catch (Exception ex)  
            {  
                dataInRead = dataIn.Length;  
                dataOutWritten = dataIn.Length;  

                return FilterStatus.Done;  
            }  
        }  

        public bool InitFilter()  
        {  
            return true;  
        }

        public void Dispose()
        {

        }
    }  	
	
}
