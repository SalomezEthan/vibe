using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vibe.Models;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;

namespace vibe.Services
{
    public class UserStorage
    {
        public static async Task<IReadOnlyList<Folder>> ListSubFoldersAsync(string rootFolderPath)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(rootFolderPath);
            var subFolders = await folder.GetFoldersAsync();

            return subFolders
                .Select(f => new Folder(f.DisplayName, f.Path))
                .ToList();
        }

        public static async Task<IReadOnlyList<Music>> ListMusicsAsync(string rootFolderPath)
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(rootFolderPath);
            var query = new QueryOptions(CommonFileQuery.DefaultQuery, [".mp3", ".wav"]);

            var files = await folder
                .CreateFileQueryWithOptions(query)
                .GetFilesAsync();

            var musics = new List<Music>();
            foreach(var file in files)
            {
                var metadata = await file.Properties.GetMusicPropertiesAsync();
                musics.Add(CreateMusic(file, metadata));
            }

            return musics;
        }

        public static async Task<Music> GetMusicAsync(string fileMusicPath)
        {
            var file = await StorageFile.GetFileFromPathAsync(fileMusicPath);
            var metadata = await file.Properties.GetMusicPropertiesAsync();
            return CreateMusic(file, metadata);
        }

        private static Music CreateMusic(StorageFile file, MusicProperties metadata)
        {
            string title = string.IsNullOrEmpty(metadata.Title)
                    ? file.DisplayName
                    : metadata.Title;

            return new Music(
                title,
                metadata.Artist,
                file.Path,
                metadata.Duration
                );
        }
    }
}
