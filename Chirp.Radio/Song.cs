using System;
using System.Windows;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chirp.Radio
{
    public class Song : INotifyPropertyChanged
    {
        public Song()
        {
            this.lastfm_urls = new Images() { sm_image = "Images/tape.png" };
        }
        public string Track
        {
            get
            {
                return _track;
            }
            set
            {
                if (this._track != value)
                {
                    this._track = value;
                    this.RaisePropertyChanged("Track");
                }
            }
        }

        public string Dj
        {
            get { return this._dj; }
            set
            {
                if (this._dj != value)
                {
                    this._dj = value;
                    RaisePropertyChanged("Dj");
                }
            }
        }

        public string Artist
        {
            get { return this._artist; }
            set
            {
                if (this._artist != value)
                {
                    this._artist = value;
                    RaisePropertyChanged("Artist");
                }
            }
        }

        public string Label
        {
            get { return this._label; }
            set
            {
                if (this._label != value)
                {
                    this._label = value;
                    RaisePropertyChanged("Label");
                }
            }
        }

        public string Release
        {
            get { return this._release; }
            set
            {
                if (this._release != value)
                {
                    this._release = value;
                    RaisePropertyChanged("Release");
                }
            }
        }

        // i wish i knew how to do this...
        [JsonProperty("lastfm_urls/large_image")]
        public string ImageLarge
        {
            get 
            {
                if(this.lastfm_urls.large_image == null)
                    return "Images/bird.png";
                return this.lastfm_urls.large_image; 
            }
            //get { return this._imageLarge; }
            //set
            //{
            //    if (this._imageLarge != value)
            //    {
            //        this._imageLarge = value;
            //        RaisePropertyChanged("ImageLarge");
            //    }
            //}
        }

        [JsonProperty("sm_image")]
        public string ImageSmall
        {
            get 
            { 
                if(this.lastfm_urls.sm_image == null)
                    return "Images/tape.png";
                return this.lastfm_urls.sm_image; 
            }
            //get { return this._imageSmall; }
            //set
            //{
            //    if (this._imageSmall != value)
            //    {
            //        this._imageSmall = value;
            //        RaisePropertyChanged("ImageSmall");
            //    }
            //}
        }

        public Images lastfm_urls 
        {
            get; 
            set;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _dj;
        private string _artist;
        private string _track;
        private string _playedAt;
        private string _label;
        private string _release;
        private string _imageLarge;
        private string _imageSmall;

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Images
    {
        public string med_image { get; set; }
        public string sm_image { get; set; }
        public string large_image { get; set; }
    }
}
