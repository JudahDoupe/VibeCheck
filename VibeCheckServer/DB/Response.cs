using System.ComponentModel.DataAnnotations;
using VibeCheck.Model.ViewModels;

namespace VibeCheckServer.DB;

public class Response
{
    public Response(){}
    
    public Response(string surveyCode, string feelingPath)
    {
        SurveyCode = surveyCode;
        FeelingPath = feelingPath;
    }

    [Key]
    public int Id { get; set; }
    public string SurveyCode { get; set; }
    public string FeelingPath { get; set; }

    public SurveyResponse ToViewModel(string color) => new() { Feeling = new Feeling(FeelingPath, color) };
}
