﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable Await1 // Method is not configured to be awaited

namespace _02._2_DeadLock_Async
{
    public class VM : INotifyPropertyChanged
    {
        //private const string URL = "http://lorempixel.com/800/800/animals/";
        private const string URL = "https://source.unsplash.com/800x800/?dog";

        public event PropertyChangedEventHandler PropertyChanged;

        public VM()
        {
            Process();
        }

        private async Task Process()
        {
            for (int i = 0; i < 10; i++)
            {
                Data = await DownloadAsync(URL);//.Result; // deadlock
                //Data = await DownloadAsync(URL);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));

            }
        }

        private async Task<byte[]> DownloadAsync(string url)
        {
            using (var http = new HttpClient())
            {
                byte[] data = await http.GetByteArrayAsync(url).ConfigureAwait(false);
                // cannot get into the this line because the calling method
                // hold the synchronization context (.Result)
                return data;
            }
        }

        public byte[] Data { get; set; }
    }
}
