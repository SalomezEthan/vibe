using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using vibe.Messages;
using vibe.Models;
using vibe.Services;

namespace vibe.ViewModels
{
    public partial class MusicPlayerViewModel : BaseViewModel
    {
        private readonly MusicPlayer player;
        private readonly PlaybackQueue queue;
        private readonly Messenger messenger;

        private bool loading;
        private double volume;

        public MusicPlayerViewModel(MusicPlayer player, PlaybackQueue queue, Messenger messenger)
        {
            this.player = player;
            this.queue = queue;
            this.messenger = messenger;

            this.loading = false;
            this.volume = this.player.Volume;

            this.player.CurrentChanged += OnCurrentChanged;
            this.player.StateChanged += OnPlaybackStateChanged;
        }

        public bool IsPlaying => this.player.State is PlaybackState.Play;
        public bool IsShuffled => this.queue.IsShuffled;
        public Music? Current => this.player.Current;
        public bool PlayerIsEmpty => this.player.Current is null;

        public bool Loading
        {
            get => this.loading;
            private set => this.SetProperty(ref this.loading, value);
        }

        public double Volume
        {
            get => this.volume;
            set
            {
                if (this.volume != value)
                {
                    this.volume = value;
                    this.player.SetVolume(value);
                    this.OnPropertyChanged();
                }
            }
        }

        private void OnCurrentChanged(object? sender, Music? music)
        {
            OnPropertyChanged(nameof(this.Current));
            OnPropertyChanged(nameof(this.PlayerIsEmpty));
        }

        private void OnPlaybackStateChanged(object? sender, PlaybackState newState)
        {
            OnPropertyChanged(nameof(this.IsPlaying));
        }

        public void TogglePlaybackState()
        {
            try
            {
                this.player.TogglePlayPause();
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async Task NextAsync()
        {
            this.Loading = true;

            try
            {
                var song = this.queue.NextMusic();
                await this.player.PlaySongAsync(song);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            } 
            finally
            {
                this.Loading = false;
            }
        }

        public async Task PreviousAsync()
        {
            this.Loading = true;

            try
            {
                var song = this.queue.PreviousMusic();
                await this.player.PlaySongAsync(song);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.Loading = false;
            }
        }

        public void ToggleOrderingState()
        {
            this.queue.ToggleOrderingQueueState();
            OnPropertyChanged(nameof(this.IsShuffled));
        }
    }
}
