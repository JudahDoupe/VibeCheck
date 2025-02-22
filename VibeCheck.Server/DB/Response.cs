using System.ComponentModel.DataAnnotations;

namespace VibeCheckServer.DB;

public class Response
{
    [Key]
    public int Id { get; set; }
    public string Feeling { get; set; }
    public string Code { get; set; }

    public VibeCheck.Model.ViewModels.SurveyResponse ToViewModel() => new() { Feeling = Feeling };
}
