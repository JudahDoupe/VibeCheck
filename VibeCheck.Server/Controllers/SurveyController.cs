using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VibeCheck.Model.ViewModels;
using VibeCheckServer.DB;
using Response = VibeCheckServer.DB.Response;
using Survey = VibeCheck.Model.ViewModels.Survey;

namespace VibeCheckServer.Controllers;

[ApiController]
[Route("survey")]
public class SurveyController : ControllerBase
{
    private readonly AppDbContext _context;

    public SurveyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Survey>> CreateSurvey([FromBody] SurveyRequest request)
    {
        var code = "";
        do code = GenerateCode();
        while (_context.Surveys.Any(s => s.Code == code));

        var survey = new DB.Survey { Question = request.Question, Code = code };
        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();

        return Ok(survey.ToViewModel());
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<Survey>> GetSurvey(string code)
    {
        var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.Code == code);
        if (survey == null)
        {
            return NotFound();
        }
        return Ok(survey.ToViewModel());
    }

    [HttpPost("submit/{code}")]
    public async Task<IActionResult> SubmitResponse(string code, [FromBody] SurveyResponse response)
    {
        _context.Responses.Add(new Response { Feeling = response.Feeling, Code = code });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("responses/{code}")]
    public async Task<ActionResult<List<Response>>> GetResponses(string code)
    {
        var responses = await _context.Responses.Where(r => r.Code == code).Select(r => r.ToViewModel()).ToListAsync();

        return Ok(responses);
    }

    [HttpGet("feelings")]
    public ActionResult<List<Feeling>> GetFeelings()
    {
        var feelings = new List<Feeling>
        {
            new("Anger", null, 0),
            new("Happy", null, 0),
            new("Disgust", null, 0),
            new("Sad", null, 0),
            new("Fear", null, 0),
            new("Frustrated", "Anger", 1),
            new("Aggressive", "Anger", 1),
        };

        return Ok(feelings);
    }

    private string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
