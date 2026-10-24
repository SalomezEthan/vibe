using Microsoft.UI.System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using vibe.Factories;
using vibe.Messages;
using vibe.Models;
using vibe.Services;

namespace vibe.ViewModels
{
    public partial class FolderViewModel : BaseViewModel
    {
        private readonly Folder model;
        private readonly MusicPlayer player;
        private readonly PlaybackQueue queue;
        private readonly Messenger messenger;
        private readonly ViewModelFactory factory;

        private bool loadingFolders = false;
        private bool loadingMusics = false;

        public FolderViewModel(Folder folder, MusicPlayer player, PlaybackQueue queue, Messenger messenger, ViewModelFactory factory)
        {
            this.model = folder;
            this.player = player;
            this.queue = queue;
            this.messenger = messenger;
            this.factory = factory;
        }

        public string Name => this.model.Name;
        public string Path => this.model.Path;

        public ObservableCollection<FolderViewModel> Subfolders = [];
        public ObservableCollection<MusicViewModel> Musics = [];

        public bool LoadingFolders
        {
            get => this.loadingFolders;
            private set => this.SetProperty(ref this.loadingFolders, value);
        }

        public bool LoadingMusics
        {
            get => this.loadingMusics;
            private set => this.SetProperty(ref this.loadingMusics, value);
        }

        public async Task LoadSubFoldersLikeTreeAsync()
        {
            await this.LoadSubFoldersAsync();
            foreach (var subFolder in this.Subfolders)
            {
                await subFolder.LoadSubFoldersLikeTreeAsync();
            }
        }

        private async Task LoadSubFoldersAsync()
        {
            this.LoadingFolders = true;
            this.Subfolders.Clear();

            var folders = await UserStorage.ListSubFoldersAsync(this.Path);
            foreach (var folder in folders)
            {
                var viewModel = this.factory.CreateFolderViewModel(folder);
                this.Subfolders.Add(viewModel);
            }

            this.LoadingFolders = false;
        }

        public async Task LoadMusicsAsync()
        {
            this.LoadingMusics = true;
            this.Musics.Clear();

            foreach (var music in await UserStorage.ListMusicsAsync(this.Path))
            {
                var viewModel = this.factory.CreateMusicViewModel(music);
                this.Musics.Add(viewModel);
            }

            this.LoadingMusics = false;
        }

        public async Task PlayFolderAsync(MusicViewModel? firstMusic)
        {
            var folderMusics = await UserStorage.ListMusicsAsync(this.Path);
            var musics = folderMusics.ToList();

            this.queue.Clear();
            this.queue.InsertRangeOfSongs(musics, InsertPlacement.AtLast);

            Music musicPlayed = firstMusic is not null
                ? this.queue.JumpWhile(m => m.Path == firstMusic.Path)
                : this.queue.NextMusic();

            await this.player.PlaySongAsync(musicPlayed);
        }
    }
}
