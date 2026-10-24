using Microsoft.UI.Xaml.Controls;
using System.Linq;
using System.Threading.Tasks;
using vibe.Factories;
using vibe.Models;
using vibe.Views;

namespace vibe.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Créer une abstraction serait du travail supplémentaire pour peu de valeur
        private readonly Frame frame;
        private FolderViewModel? selectedFolder;

        public MainViewModel(Frame frame, MusicPlayerViewModel player, ViewModelFactory factory)
        {
            this.frame = frame;
            this.Player = player;
            var rootFolder = new Folder("Root", "C:\\Users\\sceth\\Music");
            this.RootFolder = factory.CreateFolderViewModel(rootFolder);
        }

        public FolderViewModel RootFolder { get; }

        public MusicPlayerViewModel Player { get; }

        public FolderViewModel? SelectedFolder
        {
            get => this.selectedFolder;
            set
            {
                if (this.selectedFolder != value)
                {
                    this.selectedFolder = value;
                    OnPropertyChanged(nameof(SelectedFolder));

                    this.frame.Navigate(typeof(FolderPageView), value);
                }
            }
        }

        public async Task LoadFolderTreeAsync()
        {
            await this.RootFolder.LoadSubFoldersLikeTreeAsync();
            this.SelectedFolder = this.RootFolder.Subfolders.FirstOrDefault();
        }
    }
}
