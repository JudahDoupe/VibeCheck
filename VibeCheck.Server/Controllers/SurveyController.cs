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
            new("Joy", null, 0, "#8ac926"),
            new("Optimistic", "Joy", 1, "#8ac926"),
            new("Inspired", "Optimistic", 2, "#8ac926"),
            new("Hopeful", "Optimistic", 2, "#8ac926"),
            new("Proud", "Joy", 1, "#8ac926"),
            new("Confident", "Proud", 2, "#8ac926"),
            new("Successful", "Proud", 2, "#8ac926"),
            new("Cheerful", "Joy", 1, "#8ac926"),
            new("Happy", "Cheerful", 2, "#8ac926"),
            new("Delighted", "Cheerful", 2, "#8ac926"),
            new("Peaceful", "Joy", 1, "#8ac926"),
            new("Content", "Peaceful", 2, "#8ac926"),
            new("Satisfied", "Peaceful", 2, "#8ac926"),
            new("Powerful", "Joy", 1, "#8ac926"),
            new("Courageous", "Powerful", 2, "#8ac926"),
            new("Creative", "Powerful", 2, "#8ac926"),
            new("Accepted", "Joy", 1, "#8ac926"),
            new("Respected", "Accepted", 2, "#8ac926"),
            new("Valued", "Accepted", 2, "#8ac926"),
            new("Intimate", "Joy", 1, "#8ac926"),
            new("Connected", "Intimate", 2, "#8ac926"),
            new("Authentic", "Intimate", 2, "#8ac926"),
            new("Playful", "Joy", 1, "#8ac926"),
            new("Aroused", "Playful", 2, "#8ac926"),
            new("Free", "Joy", 1, "#8ac926"),
            new("Spontaneous", "Free", 2, "#8ac926"),
            new("Liberated", "Free", 2, "#8ac926"),
            new("Love", null, 0, "#ffca3a"),
            new("Romantic", "Love", 1, "#ffca3a"),
            new("Enchanted", "Romantic", 2, "#ffca3a"),
            new("Rapturous", "Romantic", 2, "#ffca3a"),
            new("Sentimental", "Love", 1, "#ffca3a"),
            new("Nostalgic", "Sentimental", 2, "#ffca3a"),
            new("Appreciative", "Sentimental", 2, "#ffca3a"),
            new("Tender", "Love", 1, "#ffca3a"),
            new("Affectionate", "Tender", 2, "#ffca3a"),
            new("Compassionate", "Tender", 2, "#ffca3a"),
            new("Attracted", "Love", 1, "#ffca3a"),
            new("Infatuated", "Attracted", 2, "#ffca3a"),
            new("Passionate", "Attracted", 2, "#ffca3a"),
            new("Admiring", "Love", 1, "#ffca3a"),
            new("Adoring", "Admiring", 2, "#ffca3a"),
            new("Desired", "Admiring", 2, "#ffca3a"),
            new("Caring", "Love", 1, "#ffca3a"),
            new("Warm", "Caring", 2, "#ffca3a"),
            new("Trusting", "Caring", 2, "#ffca3a"),
            new("Fear", null, 0, "#6a4c93"),
            new("Scared", "Fear", 1, "#6a4c93"),
            new("Helpless", "Scared", 2, "#6a4c93"),
            new("Frightened", "Scared", 2, "#6a4c93"),
            new("Anxious", "Fear", 1, "#6a4c93"),
            new("Overwhelmed", "Anxious", 2, "#6a4c93"),
            new("Worried", "Anxious", 2, "#6a4c93"),
            new("Insecure", "Fear", 1, "#6a4c93"),
            new("Inferior", "Insecure", 2, "#6a4c93"),
            new("Submissive", "Fear", 1, "#6a4c93"),
            new("Insignificant", "Submissive", 2, "#6a4c93"),
            new("Worthless", "Submissive", 2, "#6a4c93"),
            new("Rejected", "Fear", 1, "#6a4c93"),
            new("Lonely", "Rejected", 2, "#6a4c93"),
            new("Humiliated", "Fear", 1, "#6a4c93"),
            new("Disrespected", "Humiliated", 2, "#6a4c93"),
            new("Ridiculed", "Humiliated", 2, "#6a4c93"),
            new("Weak", "Fear", 1, "#6a4c93"),
            new("Vulnerable", "Weak", 2, "#6a4c93"),
            new("Victimized", "Weak", 2, "#6a4c93"),
            new("Threatened", "Fear", 1, "#6a4c93"),
            new("Nervous", "Threatened", 2, "#6a4c93"),
            new("Exposed", "Threatened", 2, "#6a4c93"),
            new("Anger", null, 0, "#ff595e"),
            new("Jealous", "Anger", 1, "#ff595e"),
            new("Resentful", "Jealous", 2, "#ff595e"),
            new("Envious", "Jealous", 2, "#ff595e"),
            new("Frustrated", "Anger", 1, "#ff595e"),
            new("Annoyed", "Frustrated", 2, "#ff595e"),
            new("Irritated", "Frustrated", 2, "#ff595e"),
            new("Critical", "Anger", 1, "#ff595e"),
            new("Skeptical", "Critical", 2, "#ff595e"),
            new("Sarcastic", "Critical", 2, "#ff595e"),
            new("Distant", "Anger", 1, "#ff595e"),
            new("Withdrawn", "Distant", 2, "#ff595e"),
            new("Suspicious", "Distant", 2, "#ff595e"),
            new("Hurt", "Anger", 1, "#ff595e"),
            new("Embarrassed", "Hurt", 2, "#ff595e"),
            new("Disappointed", "Hurt", 2, "#ff595e"),
            new("Hostile", "Anger", 1, "#ff595e"),
            new("Hateful", "Hostile", 2, "#ff595e"),
            new("Violent", "Hostile", 2, "#ff595e"),
            new("Infuriated", "Anger", 1, "#ff595e"),
            new("Enraged", "Infuriated", 2, "#ff595e"),
            new("Furious", "Infuriated", 2, "#ff595e"),
            new("Aggressive", "Anger", 1, "#ff595e"),
            new("Provoked", "Aggressive", 2, "#ff595e"),
            new("Mad", "Aggressive", 2, "#ff595e"),
            new("Sadness", null, 0, "#1982c4"),
            new("Guilty", "Sadness", 1, "#1982c4"),
            new("Regretful", "Guilty", 2, "#1982c4"),
            new("Remorseful", "Guilty", 2, "#1982c4"),
            new("Abandoned", "Sadness", 1, "#1982c4"),
            new("Ignored", "Abandoned", 2, "#1982c4"),
            new("Excluded", "Abandoned", 2, "#1982c4"),
            new("Despair", "Sadness", 1, "#1982c4"),
            new("Grief", "Despair", 2, "#1982c4"),
            new("Powerless", "Despair", 2, "#1982c4"),
            new("Depressed", "Sadness", 1, "#1982c4"),
            new("Hopeless", "Depressed", 2, "#1982c4"),
            new("Empty", "Depressed", 2, "#1982c4"),
            new("Lonely", "Sadness", 1, "#1982c4"),
            new("Isolated", "Lonely", 2, "#1982c4"),
            new("Abandoned", "Lonely", 2, "#1982c4"),
            new("Bored", "Sadness", 1, "#1982c4"),
            new("Apathetic", "Bored", 2, "#1982c4"),
            new("Indifferent", "Bored", 2, "#1982c4"),
            new("Tired", "Sadness", 1, "#1982c4"),
            new("Sleepy", "Tired", 2, "#1982c4"),
            new("Unfocused", "Tired", 2, "#1982c4"),
            new("Ashamed", "Sadness", 1, "#1982c4"),
            new("Mortified", "Ashamed", 2, "#1982c4"),
            new("Humiliated", "Ashamed", 2, "#1982c4"),
            new("Inferior", "Sadness", 1, "#1982c4"),
            new("Inadequate", "Inferior", 2, "#1982c4"),
            new("Insecure", "Inferior", 2, "#1982c4"),
            new("Disappointed", "Sadness", 1, "#1982c4"),
            new("Let down", "Disappointed", 2, "#1982c4"),
            new("Surprise", null, 0, "#ffca3a"),
            new("Amazed", "Surprise", 1, "#ffca3a"),
            new("Awe", "Amazed", 2, "#ffca3a"),
            new("Astonished", "Amazed", 2, "#ffca3a"),
            new("Confused", "Surprise", 1, "#ffca3a"),
            new("Disillusioned", "Confused", 2, "#ffca3a"),
            new("Perplexed", "Confused", 2, "#ffca3a"),
            new("Excited", "Surprise", 1, "#ffca3a"),
            new("Eager", "Excited", 2, "#ffca3a"),
            new("Energetic", "Excited", 2, "#ffca3a"),
            new("Startled", "Surprise", 1, "#ffca3a"),
            new("Shocked", "Startled", 2, "#ffca3a"),
            new("Dismayed", "Startled", 2, "#ffca3a"),
            new("Stimulated", "Surprise", 1, "#ffca3a"),
            new("Curious", "Stimulated", 2, "#ffca3a"),
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
