using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using vibe.Models;

namespace vibe.Services
{
    public enum InsertPlacement
    {
        AfterIndex,
        AtLast,
    }

    public class PlaybackQueue
    {
        private readonly Random random;

        private readonly List<Music> queue;
        private readonly List<Music> shuffled;

        public PlaybackQueue()
        {
            this.random = new Random();

            this.queue = [];
            this.shuffled = [];

            this.Index = -1;
        }

        public bool IsShuffled { get; private set; }
        public int Index { get; private set; }

        public IReadOnlyList<Music> CurrentQueue => this.IsShuffled
            ? this.shuffled
            : this.queue;

        public void InsertRangeOfSongs(IEnumerable<Music> musics, InsertPlacement placement)
        {
            if (placement is InsertPlacement.AfterIndex)
            {
                musics = musics.Reverse();
            }

            foreach (var music in musics)
            {
                InsertSong(music, placement);
            }
        }
        
        public void InsertSong(Music music, InsertPlacement placement)
        {
            if (this.queue.Contains(music))
            {
                throw new InvalidOperationException("La musique est déjà contenue dans la file.");
            }

            if (placement is InsertPlacement.AfterIndex)
            {
                if (this.IsShuffled)
                {
                    this.shuffled.Insert(Index + 1, music);
                    this.queue.Add(music);
                }
                else
                {
                    this.queue.Insert(Index + 1, music);
                }
            }
            else
            {
                this.queue.Add(music);

                if (this.IsShuffled)
                {
                    this.shuffled.Add(music);
                }
            }

            if (Index == -1)
            {
                ++Index;
            }
        }

        public Music NextMusic()
        {
            if (this.queue.Count == 0)
            {
                throw new InvalidOperationException("Impossible d'aller au son suivant car la file est vide.");
            }

            Index = (Index + 1 + this.queue.Count) % this.queue.Count;
            return GetMusicAt(this.Index);
        }

        public Music PreviousMusic()
        {
            if (this.queue.Count == 0)
            {
                throw new InvalidOperationException("Impossible d'aller au son précédent car la file est vide.");
            }

            Index = (Index - 1 + this.queue.Count) % this.queue.Count;
            return GetMusicAt(this.Index);
        }


        public bool TryPeekNext([MaybeNullWhen(false)] out Music music)
        {
            if (this.queue.Count == 0)
            {
                music = null;
                return false;
            }
            else
            {
                music = GetMusicAt((Index + 1 + this.queue.Count) % this.queue.Count);
                return true;
            }
        }

        public void Remove(Music music)
        {
            if (this.queue.Contains(music))
            {
                if (GetMusicAt(Index) == music)
                {
                    Index--;
                }

                this.queue.Remove(music);
                
                if (this.IsShuffled)
                {
                    this.shuffled.Remove(music);
                }

                if (this.queue.Count == 0)
                {
                    this.Index = -1;
                }
            }
        }

        private Music GetMusicAt(int index)
        {
            return this.IsShuffled
                ? this.shuffled[index]
                : this.queue[index];
        }

        public void ToggleOrderingQueueState()
        {
            if (this.Index != -1)
            {
                if (this.IsShuffled)
                {
                    this.Index = this.queue.IndexOf(this.shuffled[this.Index]);
                }
                else
                {
                    var musics = this.queue.ToList();
                    var shuffled = new List<Music>();

                    var music = musics[this.Index];
                    musics.Remove(music);
                    shuffled.Add(music);

                    while (musics.Count > 0)
                    {
                        var randomIndex = random.Next(musics.Count);
                        music = musics[randomIndex];

                        musics.RemoveAt(randomIndex);
                        shuffled.Add(music);
                    }

                    this.shuffled.Clear();
                    this.shuffled.AddRange(shuffled);
                }
            }

            this.IsShuffled = !this.IsShuffled;
        }

        public void Clear()
        {
            this.queue.Clear();
            this.shuffled.Clear();
            this.Index = -1;
        }

        public Music JumpWhile(Func<Music, bool> predicate)
        {
            while (!predicate.Invoke(this.GetMusicAt(this.Index)))
            {
                this.NextMusic();
            }

            return this.GetMusicAt(this.Index);
        }
    }
}
