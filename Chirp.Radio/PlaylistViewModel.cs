using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Chirp.Radio
{
    public class PlaylistViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PlaylistViewModel()
        {
            this._loadDataCommand = new DelegateCommand(this.LoadDataAction);
        }

        public ObservableCollection<Song> RecentTracks
        {
            get
            {
                if (this._playlist == null)
                {
                    this._playlist = new ObservableCollection<Song>();
                }
                return this._playlist;
            }
        }

        public Song CurrentTrack
        {
            get
            {
                return this._currentTrack;
            }
            set
            {
                if (this._currentTrack != value)
                {
                    this._currentTrack = value;
                    this.RaisePropertyChanged("CurrentTrack");
                }
            }
        }

        public bool Busy
        {
            get { return _busy; }
            set
            {
                _busy = value;
                RaisePropertyChanged("Busy");
            }
        }

        public ICommand LoadDataCommand
        {
            get { return this._loadDataCommand; }
        }

        private void LoadDataAction(object o)
        {
            this._currentTrack = new Song() 
            {
            };
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Song> _playlist;
        private Song _currentTrack;
        private ICommand _loadDataCommand;
        private bool _busy;
    }
}
