namespace VibeCheck.Components;

public record FeelingModel(string Name, string? Parent);

public record FeelingTileModel(Angle Start, Angle End, double InnerRadius, double OuterRadius, string Color)
{
    public double Length => OuterRadius - InnerRadius;
}

public record Angle
{
    public double Ratio { get; }
    public double Degree => Ratio * 360.0;
    public double Radians => Ratio * (2 * Math.PI);

    private Angle(double ratio)
    {
        Ratio = ratio;
    }

    public static Angle FromRatio(double ratio) => new(ratio);
    public static Angle FromDregrees(double degrees) => new(degrees / 180.0);
    public static Angle FromRadians(double radians) => new(radians / (2 * Math.PI));

    public Point2D Project(double distance) => new Point2D(distance * Math.Sin(Radians), distance * Math.Cos(Radians));
    
    public Angle Inverse() => FromRatio(1-Ratio);

}

public record Point2D(double X, double Y)
{
    public string ToSvg() => $"{X},{Y}";
    
    public Point2D Add(Point2D other) => new(X + other.X, Y + other.Y); 
    public Point2D Add(double x, double y) => new(X + x, Y + y); 
};