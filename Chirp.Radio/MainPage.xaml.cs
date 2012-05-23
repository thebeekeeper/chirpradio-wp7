using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.BackgroundAudio;
using Chirp.Radio.Agent;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using System.Threading;
using System.Windows.Threading;

namespace Chirp.Radio
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            _requestHelper = new PlaylistRequestHelper();
            _requestHelper.RequestCompleted += new RequestCompletedHandler(requestHelper_RequestCompleted);
            _requestHelper.GetUpdatedPlaylist();

            _viewModel = new PlaylistViewModel();
            _viewModel.LoadDataCommand.Execute(null);
            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var playButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                playButton.Text = "Stop";
                playButton.IconUri = new Uri("/Images/pause.png", UriKind.Relative);
            }
            else
            {
                playButton.Text = "Play";
                playButton.IconUri = new Uri("/Images/play.png", UriKind.Relative);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            _requestHelper.GetUpdatedPlaylist();
        }

        void requestHelper_RequestCompleted(object sender, RequestCompletedEventArgs args)
        {
            _viewModel.CurrentTrack = args.CurrentTrack;
            _viewModel.RecentTracks.Clear();
            foreach (var t in args.PreviousTracks)
            {
                _viewModel.RecentTracks.Add(t);
            }
        }

        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            var playState = BackgroundAudioPlayer.Instance.PlayerState;
            switch (playState)
            {
                case PlayState.TrackReady:
                    Debug.WriteLine("Track ready");
                    break;
                case PlayState.BufferingStarted:
                    Debug.WriteLine("Buffering");
                    break;
                default:
                    Debug.WriteLine(playState);
                    break;
            }
        }

        private void PlayButton_Click_1(object sender, EventArgs e)
        {
            var playButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;

            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
            {
                playButton.Text = "Play";
                playButton.IconUri = new Uri("/Images/play.png", UriKind.Relative);
                BackgroundAudioPlayer.Instance.Stop();
            }
            else
            {
                playButton.Text = "Stop";
                playButton.IconUri = new Uri("/Images/pause.png", UriKind.Relative);
                BackgroundAudioPlayer.Instance.Play();
            }
        }

        private PlaylistViewModel _viewModel;
        private PlaylistRequestHelper _requestHelper;
    }
}