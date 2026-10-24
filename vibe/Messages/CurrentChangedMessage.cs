using vibe.Models;

namespace vibe.Messages
{
    public class CurrentChangedMessage : Message<Music>
    {
        public CurrentChangedMessage(Music item) : base(item)
        {
        }
    }
}
