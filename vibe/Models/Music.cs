using System;

namespace vibe.Models
{
    public sealed record Music (
        string Title,
        string Artist,
        string Path,
        TimeSpan Duration
        );
}
