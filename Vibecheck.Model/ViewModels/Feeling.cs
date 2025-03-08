namespace VibeCheck.Model.ViewModels;

public record Feeling(string Path, string Color)
{
    public string Name => Path[(Path.LastIndexOf('/') + 1)..];
    public int Depth => Path.Count(c => c == '/');
    public Feeling? Parent =>
        string.IsNullOrEmpty(Path) || !Path.Contains('/')
            ? null
            : this with { Path = Path[..Path.LastIndexOf('/')] };
}
