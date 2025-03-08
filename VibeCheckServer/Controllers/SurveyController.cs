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
        _context.Responses.Add(new Response(response.Feeling.Path, code));
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("responses/{code}")]
    public async Task<ActionResult<List<Response>>> GetResponses(string code)
    {
        var colorLookup = GetAllFeelings().ToDictionary(x => x.Path, x => x.Color);
        var responses = await _context
            .Responses.Where(r => r.SurveyCode == code)
            .Select(r => r.ToViewModel(colorLookup[r.FeelingPath]))
            .ToListAsync();

        return Ok(responses);
    }

    [HttpGet("feelings")]
    public ActionResult<List<Feeling>> GetFeelings()
    {
        return Ok(GetAllFeelings());
    }

    private string GenerateCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private List<Feeling> GetAllFeelings() =>
        [
            new("Joy", "#8ac926"),
            new("Joy/Optimistic", "#8ac926"),
            new("Joy/Optimistic/Inspired", "#8ac926"),
            new("Joy/Optimistic/Hopeful", "#8ac926"),
            new("Joy/Proud", "#8ac926"),
            new("Joy/Proud/Confident", "#8ac926"),
            new("Joy/Proud/Successful", "#8ac926"),
            new("Joy/Cheerful", "#8ac926"),
            new("Joy/Cheerful/Happy", "#8ac926"),
            new("Joy/Cheerful/Delighted", "#8ac926"),
            new("Joy/Peaceful", "#8ac926"),
            new("Joy/Peaceful/Content", "#8ac926"),
            new("Joy/Peaceful/Satisfied", "#8ac926"),
            new("Joy/Powerful", "#8ac926"),
            new("Joy/Powerful/Courageous", "#8ac926"),
            new("Joy/Powerful/Creative", "#8ac926"),
            new("Joy/Accepted", "#8ac926"),
            new("Joy/Accepted/Respected", "#8ac926"),
            new("Joy/Accepted/Valued", "#8ac926"),
            new("Joy/Intimate", "#8ac926"),
            new("Joy/Intimate/Connected", "#8ac926"),
            new("Joy/Intimate/Authentic", "#8ac926"),
            new("Joy/Playful", "#8ac926"),
            new("Joy/Playful/Aroused", "#8ac926"),
            new("Joy/Playful/Energetic", "#8ac926"),
            new("Joy/Free", "#8ac926"),
            new("Joy/Free/Spontaneous", "#8ac926"),
            new("Joy/Free/Liberated", "#8ac926"),
            new("Love", "#ffca3a"),
            new("Love/Romantic", "#ffca3a"),
            new("Love/Romantic/Enchanted", "#ffca3a"),
            new("Love/Romantic/Rapturous", "#ffca3a"),
            new("Love/Sentimental", "#ffca3a"),
            new("Love/Sentimental/Nostalgic", "#ffca3a"),
            new("Love/Sentimental/Appreciative", "#ffca3a"),
            new("Love/Tender", "#ffca3a"),
            new("Love/Tender/Affectionate", "#ffca3a"),
            new("Love/Tender/Compassionate", "#ffca3a"),
            new("Love/Attracted", "#ffca3a"),
            new("Love/Attracted/Infatuated", "#ffca3a"),
            new("Love/Attracted/Passionate", "#ffca3a"),
            new("Love/Admiring", "#ffca3a"),
            new("Love/Admiring/Adoring", "#ffca3a"),
            new("Love/Admiring/Desired", "#ffca3a"),
            new("Love/Caring", "#ffca3a"),
            new("Love/Caring/Warm", "#ffca3a"),
            new("Love/Caring/Trusting", "#ffca3a"),
            new("Fear", "#6a4c93"),
            new("Fear/Scared", "#6a4c93"),
            new("Fear/Scared/Helpless", "#6a4c93"),
            new("Fear/Scared/Frightened", "#6a4c93"),
            new("Fear/Anxious", "#6a4c93"),
            new("Fear/Anxious/Overwhelmed", "#6a4c93"),
            new("Fear/Anxious/Worried", "#6a4c93"),
            new("Fear/Insecure", "#6a4c93"),
            new("Fear/Insecure/Inadequate", "#6a4c93"),
            new("Fear/Insecure/Inferior", "#6a4c93"),
            new("Fear/Submissive", "#6a4c93"),
            new("Fear/Submissive/Insignificant", "#6a4c93"),
            new("Fear/Submissive/Worthless", "#6a4c93"),
            new("Fear/Rejected", "#6a4c93"),
            new("Fear/Rejected/Isolated", "#6a4c93"),
            new("Fear/Rejected/Lonely", "#6a4c93"),
            new("Fear/Humiliated", "#6a4c93"),
            new("Fear/Humiliated/Disrespected", "#6a4c93"),
            new("Fear/Humiliated/Ridiculed", "#6a4c93"),
            new("Fear/Weak", "#6a4c93"),
            new("Fear/Weak/Vulnerable", "#6a4c93"),
            new("Fear/Weak/Victimized", "#6a4c93"),
            new("Fear/Threatened", "#6a4c93"),
            new("Fear/Threatened/Nervous", "#6a4c93"),
            new("Fear/Threatened/Exposed", "#6a4c93"),
            new("Anger", "#ff595e"),
            new("Anger/Jealous", "#ff595e"),
            new("Anger/Jealous/Resentful", "#ff595e"),
            new("Anger/Jealous/Envious", "#ff595e"),
            new("Anger/Frustrated", "#ff595e"),
            new("Anger/Frustrated/Annoyed", "#ff595e"),
            new("Anger/Frustrated/Irritated", "#ff595e"),
            new("Anger/Critical", "#ff595e"),
            new("Anger/Critical/Skeptical", "#ff595e"),
            new("Anger/Critical/Sarcastic", "#ff595e"),
            new("Anger/Distant", "#ff595e"),
            new("Anger/Distant/Withdrawn", "#ff595e"),
            new("Anger/Distant/Suspicious", "#ff595e"),
            new("Anger/Hurt", "#ff595e"),
            new("Anger/Hurt/Embarrassed", "#ff595e"),
            new("Anger/Hurt/Disappointed", "#ff595e"),
            new("Anger/Hostile", "#ff595e"),
            new("Anger/Hostile/Hateful", "#ff595e"),
            new("Anger/Hostile/Violent", "#ff595e"),
            new("Anger/Infuriated", "#ff595e"),
            new("Anger/Infuriated/Enraged", "#ff595e"),
            new("Anger/Infuriated/Furious", "#ff595e"),
            new("Anger/Aggressive", "#ff595e"),
            new("Anger/Aggressive/Provoked", "#ff595e"),
            new("Anger/Aggressive/Mad", "#ff595e"),
            new("Sadness", "#1982c4"),
            new("Sadness/Guilty", "#1982c4"),
            new("Sadness/Guilty/Regretful", "#1982c4"),
            new("Sadness/Guilty/Remorseful", "#1982c4"),
            new("Sadness/Abandoned", "#1982c4"),
            new("Sadness/Abandoned/Ignored", "#1982c4"),
            new("Sadness/Abandoned/Excluded", "#1982c4"),
            new("Sadness/Despair", "#1982c4"),
            new("Sadness/Despair/Grief", "#1982c4"),
            new("Sadness/Despair/Powerless", "#1982c4"),
            new("Sadness/Depressed", "#1982c4"),
            new("Sadness/Depressed/Hopeless", "#1982c4"),
            new("Sadness/Depressed/Empty", "#1982c4"),
            new("Sadness/Lonely", "#1982c4"),
            new("Sadness/Lonely/Isolated", "#1982c4"),
            new("Sadness/Bored", "#1982c4"),
            new("Sadness/Bored/Apathetic", "#1982c4"),
            new("Sadness/Bored/Indifferent", "#1982c4"),
            new("Sadness/Tired", "#1982c4"),
            new("Sadness/Tired/Sleepy", "#1982c4"),
            new("Sadness/Tired/Unfocused", "#1982c4"),
            new("Sadness/Ashamed", "#1982c4"),
            new("Sadness/Ashamed/Mortified", "#1982c4"),
            new("Sadness/Ashamed/Humiliated", "#1982c4"),
            new("Sadness/Inferior", "#1982c4"),
            new("Sadness/Inferior/Inadequate", "#1982c4"),
            new("Sadness/Inferior/Insecure", "#1982c4"),
            new("Sadness/Disappointed", "#1982c4"),
            new("Sadness/Disappointed/Let down", "#1982c4"),
            new("Sadness/Disappointed/Disillusioned", "#1982c4"),
            new("Surprise", "#ffca3a"),
            new("Surprise/Amazed", "#ffca3a"),
            new("Surprise/Amazed/Awe", "#ffca3a"),
            new("Surprise/Amazed/Astonished", "#ffca3a"),
            new("Surprise/Confused", "#ffca3a"),
            new("Surprise/Confused/Disillusioned", "#ffca3a"),
            new("Surprise/Confused/Perplexed", "#ffca3a"),
            new("Surprise/Excited", "#ffca3a"),
            new("Surprise/Excited/Eager", "#ffca3a"),
            new("Surprise/Excited/Energetic", "#ffca3a"),
            new("Surprise/Startled", "#ffca3a"),
            new("Surprise/Startled/Shocked", "#ffca3a"),
            new("Surprise/Startled/Dismayed", "#ffca3a"),
            new("Surprise/Stimulated", "#ffca3a"),
            new("Surprise/Stimulated/Inspired", "#ffca3a"),
            new("Surprise/Stimulated/Curious", "#ffca3a"),
        ];
}
