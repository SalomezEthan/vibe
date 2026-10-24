using System;
using System.Threading.Tasks;
using System.Windows.Input;
using vibe.Messages;
using vibe.Models;
using vibe.Services;

namespace vibe.ViewModels
{
    public partial class MusicViewModel : BaseViewModel
    {
        private readonly Music model;
        private readonly MusicPlayer player;
        private readonly PlaybackQueue queue;
        private readonly Messenger messenger;

        public MusicViewModel(Music music, MusicPlayer player, PlaybackQueue queue, Messenger messenger)
        {
            this.model = music;
            this.player = player;
            this.queue = queue;
            this.messenger = messenger;

            this.PlayCommand = new RelayCommandAsync(() => PlayAsync());

            this.player.CurrentChanged += OnCurrentChanged;
        }

        public string Title => this.model.Title;
        public string Artist => this.model.Artist;
        public string Path => this.model.Path;
        public TimeSpan Duration => this.model.Duration;
        public bool IsPlaying => this.player.Current == this.model;

        public ICommand PlayCommand { get; }

        private void OnCurrentChanged(object? sender, Music? music)
        {
            OnPropertyChanged(nameof(this.IsPlaying));
        }

        public async Task PlayAsync()
        {
            this.queue.InsertSong(this.model, InsertPlacement.AfterIndex);
            this.queue.NextMusic();
            
            await this.player.PlaySongAsync(this.model);
        }
    }
}
