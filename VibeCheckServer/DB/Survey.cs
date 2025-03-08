using System.ComponentModel.DataAnnotations;

namespace VibeCheckServer.DB;

public class Survey
{
    [Key]
    public string Code { get; set; }
    public string Question { get; set; }

    public VibeCheck.Model.ViewModels.Survey ToViewModel() => new(Question, Code);
}
