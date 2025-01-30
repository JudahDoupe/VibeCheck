namespace VibeCheck.Components.FeelingWheel;

public record FeelingModel(string Name, string? Parent);

public record FeelingTileModel(Angle Start, Angle End, double InnerRadius, double OuterRadius, string Color)
{
    
    public string GeneratePath()
    {
        var innerStart =
            $"{OuterRadius + InnerRadius * Math.Sin(Start.Radians)},{OuterRadius - InnerRadius * Math.Cos(Start.Radians)}";
        var outerStart =
            $"{OuterRadius + OuterRadius * Math.Sin(Start.Radians)},{OuterRadius - OuterRadius * Math.Cos(Start.Radians)}";
        
        var innerEnd =
            $"{OuterRadius + InnerRadius * Math.Sin(End.Radians)},{OuterRadius - InnerRadius * Math.Cos(End.Radians)}";
        var outerEnd =
            $"{OuterRadius + OuterRadius * Math.Sin(End.Radians)},{OuterRadius - OuterRadius * Math.Cos(End.Radians)}";
        
        
        var innerSvgRadius = $"{InnerRadius},{InnerRadius}";
        var outerSvgRadius = $"{OuterRadius},{OuterRadius}";

        var path = $"M{innerStart} ";
        path += $"L{outerStart} ";
        path += $"A{outerSvgRadius} 1 0 1 {outerEnd} ";
        path += $"L{innerEnd} ";
        path += $"A{innerSvgRadius} 1 0 0 {innerStart} ";
        path += "z";
        
        return path;
    }
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
    
}