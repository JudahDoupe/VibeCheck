namespace VibeCheck.Model.Utilities;

public record Point2D(double X, double Y)
{
    public string ToSvg() => $"{X},{Y}";
    
    public Point2D Add(Point2D other) => new(X + other.X, Y + other.Y); 
    public Point2D Add(double x, double y) => new(X + x, Y + y); 
};