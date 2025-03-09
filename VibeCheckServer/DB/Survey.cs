using System.ComponentModel.DataAnnotations;

namespace VibeCheckServer.DB;

public class Survey
{
    [Key]
    public string Code { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;

    public VibeCheck.Model.ViewModels.Survey ToViewModel() => new(Question, Code);
}
