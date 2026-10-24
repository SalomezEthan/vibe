using vibe.Models;
using vibe.Services;
using vibe.ViewModels;

namespace vibe.Factories
{
    public class ViewModelFactory
    {
        private readonly MusicPlayer player;
        private readonly PlaybackQueue queue;
        private readonly Messenger messenger;

        public ViewModelFactory(MusicPlayer player, PlaybackQueue queue, Messenger messenger)
        {
            this.player = player;
            this.queue = queue;
            this.messenger = messenger;
        }

        public FolderViewModel CreateFolderViewModel(Folder folder)
        {
            return new FolderViewModel(
                folder,
                this.player,
                this.queue,
                this.messenger,
                this
                );
        }

        public MusicViewModel CreateMusicViewModel(Music music)
        {
            return new MusicViewModel(
                music,
                this.player,
                this.queue,
                this.messenger
                );
        }
    }
}
