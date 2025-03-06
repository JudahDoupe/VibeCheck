using System.ComponentModel.DataAnnotations;
using VibeCheck.Model.ViewModels;

namespace VibeCheckServer.DB;

public class Response
{
    [Key]
    public int Id { get; set; }
    public string FeelingName { get; set; }
    public string FeelingPath { get; set; }
    public string Color { get; set; }
    public string Code { get; set; }

    public SurveyResponse ToViewModel() => new() { Feeling = new Feeling(FeelingName, FeelingPath, Color) };
}
