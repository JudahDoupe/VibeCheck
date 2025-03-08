namespace VibeCheck.Model.Utilities;

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