namespace VibeCheck.Model.ViewModels;

public record Feeling(string Name, string FullPath, string Color)
{
    public Feeling? Parent()
    {
        if (string.IsNullOrEmpty(FullPath) || !FullPath.Contains('/'))
            return null;

        var parentPath = FullPath.Substring(0, FullPath.LastIndexOf('/'));
        var parentName = parentPath.Substring(parentPath.LastIndexOf('/') + 1);
        return new Feeling(parentName, parentPath, Color);
    }

    public int Depth => FullPath.Count(c => c == '/');
}
