using System;
using System.Threading.Tasks;
using vibe.Models;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace vibe.Services
{
    public enum PlaybackState
    {
        Play,
        Stop
    }

    public class MusicPlayer
    {
        private readonly MediaPlayer player;

        public MusicPlayer(MediaPlayer player)
        {
            this.player = player;
            this.State = PlaybackState.Stop;
            this.Volume = this.player.Volume;
        }

        public PlaybackState State { get; private set; }
        public double Volume { get; private set; }
        public Music? Current { get; private set; }

        public async Task PlaySongAsync(Music music)
        {
            var file = await StorageFile.GetFileFromPathAsync(music.Path);
            var source = MediaSource.CreateFromStorageFile(file);
            player.Source = source;
            player.Play();

            this.State = PlaybackState.Play;
            this.Current = music;

            this.CurrentChanged?.Invoke(this, music);
            this.StateChanged?.Invoke(this, this.State);
        }

        public void TogglePlayPause()
        {
            if (this.Current is null)
            {
                throw new InvalidOperationException("Aucun son n'est joué.");
            }

            if (this.State is PlaybackState.Stop)
            {
                this.player.Play();
                this.State = PlaybackState.Play;
            }
            else
            {
                this.player.Pause();
                this.State = PlaybackState.Stop;
            }

            this.StateChanged?.Invoke(this, this.State);
        }

        public void SetVolume(double volume)
        {
            if (volume < 0 || volume > 100)
            {
                throw new ArgumentException("Le volume doit être entre 0 et 100");
            }

            this.player.Volume = volume / 100;
            this.Volume = volume;
        }

        public event EventHandler<Music?>? CurrentChanged;
        public event EventHandler<PlaybackState>? StateChanged;
    }
}
