using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Chirp.Radio
{
    public delegate void RequestCompletedHandler(object sender, RequestCompletedEventArgs args);

    public class PlaylistRequestHelper
    {
        public event RequestCompletedHandler RequestCompleted;

        public void GetUpdatedPlaylist()
        {
            var client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(_playlistUrl, UriKind.Absolute));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var playlistString = e.Result;
                try
                {
                    var json = JObject.Parse(playlistString);
                    var currentTrackFragment = json["now_playing"];
                    var currentTrack = JsonConvert.DeserializeObject<Song>(currentTrackFragment.ToString());

                    var recentlyPlayedFragment = json["recently_played"];
                    var recentlyPlayed = JsonConvert.DeserializeObject<List<Song>>(recentlyPlayedFragment.ToString());

                    var args = new RequestCompletedEventArgs()
                    {
                        CurrentTrack = currentTrack,
                        PreviousTracks = recentlyPlayed
                    };
                    if (RequestCompleted != null)
                    {
                        RequestCompleted(this, args);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                if(RequestCompleted != null)
                {
                    RequestCompleted(this, new RequestCompletedEventArgs() { Error = e.Error.Message });
                }
                else
                {
                    throw new Exception("Unable to retrieve playlist");
                }
            }
        }

        private string _playlistUrl = "http://chirpradio.appspot.com/api/current_playlist?src=chirpradio-wp7";
    }

    public class RequestCompletedEventArgs
    {
        public string Error { get; set; }
        
        public Song CurrentTrack { get; set; }
        public IList<Song> PreviousTracks { get; set; }
    }
}
