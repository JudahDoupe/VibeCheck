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
        _context.Responses.Add(new Response { 
            FeelingPath = response.Feeling.FullPath, 
            FeelingName = response.Feeling.Name, 
            Color = response.Feeling.Color, 
            Code = code 
        });
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
            new("Joy", "Joy", "#8ac926"),
            new("Optimistic", "Joy/Optimistic", "#8ac926"),
            new("Inspired", "Joy/Optimistic/Inspired", "#8ac926"),
            new("Hopeful", "Joy/Optimistic/Hopeful", "#8ac926"),
            new("Proud", "Joy/Proud", "#8ac926"),
            new("Confident", "Joy/Proud/Confident", "#8ac926"),
            new("Successful", "Joy/Proud/Successful", "#8ac926"),
            new("Cheerful", "Joy/Cheerful", "#8ac926"),
            new("Happy", "Joy/Cheerful/Happy", "#8ac926"),
            new("Delighted", "Joy/Cheerful/Delighted", "#8ac926"),
            new("Peaceful", "Joy/Peaceful", "#8ac926"),
            new("Content", "Joy/Peaceful/Content", "#8ac926"),
            new("Satisfied", "Joy/Peaceful/Satisfied", "#8ac926"),
            new("Powerful", "Joy/Powerful", "#8ac926"),
            new("Courageous", "Joy/Powerful/Courageous", "#8ac926"),
            new("Creative", "Joy/Powerful/Creative", "#8ac926"),
            new("Accepted", "Joy/Accepted", "#8ac926"),
            new("Respected", "Joy/Accepted/Respected", "#8ac926"),
            new("Valued", "Joy/Accepted/Valued", "#8ac926"),
            new("Intimate", "Joy/Intimate", "#8ac926"),
            new("Connected", "Joy/Intimate/Connected", "#8ac926"),
            new("Authentic", "Joy/Intimate/Authentic", "#8ac926"),
            new("Playful", "Joy/Playful", "#8ac926"),
            new("Aroused", "Joy/Playful/Aroused", "#8ac926"),
            new("Energetic", "Joy/Playful/Energetic", "#8ac926"),
            new("Free", "Joy/Free", "#8ac926"),
            new("Spontaneous", "Joy/Free/Spontaneous", "#8ac926"),
            new("Liberated", "Joy/Free/Liberated", "#8ac926"),
            new("Love", "Love", "#ffca3a"),
            new("Romantic", "Love/Romantic", "#ffca3a"),
            new("Enchanted", "Love/Romantic/Enchanted", "#ffca3a"),
            new("Rapturous", "Love/Romantic/Rapturous", "#ffca3a"),
            new("Sentimental", "Love/Sentimental", "#ffca3a"),
            new("Nostalgic", "Love/Sentimental/Nostalgic", "#ffca3a"),
            new("Appreciative", "Love/Sentimental/Appreciative", "#ffca3a"),
            new("Tender", "Love/Tender", "#ffca3a"),
            new("Affectionate", "Love/Tender/Affectionate", "#ffca3a"),
            new("Compassionate", "Love/Tender/Compassionate", "#ffca3a"),
            new("Attracted", "Love/Attracted", "#ffca3a"),
            new("Infatuated", "Love/Attracted/Infatuated", "#ffca3a"),
            new("Passionate", "Love/Attracted/Passionate", "#ffca3a"),
            new("Admiring", "Love/Admiring", "#ffca3a"),
            new("Adoring", "Love/Admiring/Adoring", "#ffca3a"),
            new("Desired", "Love/Admiring/Desired", "#ffca3a"),
            new("Caring", "Love/Caring", "#ffca3a"),
            new("Warm", "Love/Caring/Warm", "#ffca3a"),
            new("Trusting", "Love/Caring/Trusting", "#ffca3a"),
            new("Fear", "Fear", "#6a4c93"),
            new("Scared", "Fear/Scared", "#6a4c93"),
            new("Helpless", "Fear/Scared/Helpless", "#6a4c93"),
            new("Frightened", "Fear/Scared/Frightened", "#6a4c93"),
            new("Anxious", "Fear/Anxious", "#6a4c93"),
            new("Overwhelmed", "Fear/Anxious/Overwhelmed", "#6a4c93"),
            new("Worried", "Fear/Anxious/Worried", "#6a4c93"),
            new("Insecure", "Fear/Insecure", "#6a4c93"),
            new("Inadequate", "Fear/Insecure/Inadequate", "#6a4c93"),
            new("Inferior", "Fear/Insecure/Inferior", "#6a4c93"),
            new("Submissive", "Fear/Submissive", "#6a4c93"),
            new("Insignificant", "Fear/Submissive/Insignificant", "#6a4c93"),
            new("Worthless", "Fear/Submissive/Worthless", "#6a4c93"),
            new("Rejected", "Fear/Rejected", "#6a4c93"),
            new("Isolated", "Fear/Rejected/Isolated", "#6a4c93"),
            new("Lonely", "Fear/Rejected/Lonely", "#6a4c93"),
            new("Humiliated", "Fear/Humiliated", "#6a4c93"),
            new("Disrespected", "Fear/Humiliated/Disrespected", "#6a4c93"),
            new("Ridiculed", "Fear/Humiliated/Ridiculed", "#6a4c93"),
            new("Weak", "Fear/Weak", "#6a4c93"),
            new("Vulnerable", "Fear/Weak/Vulnerable", "#6a4c93"),
            new("Victimized", "Fear/Weak/Victimized", "#6a4c93"),
            new("Threatened", "Fear/Threatened", "#6a4c93"),
            new("Nervous", "Fear/Threatened/Nervous", "#6a4c93"),
            new("Exposed", "Fear/Threatened/Exposed", "#6a4c93"),
            new("Anger", "Anger", "#ff595e"),
            new("Jealous", "Anger/Jealous", "#ff595e"),
            new("Resentful", "Anger/Jealous/Resentful", "#ff595e"),
            new("Envious", "Anger/Jealous/Envious", "#ff595e"),
            new("Frustrated", "Anger/Frustrated", "#ff595e"),
            new("Annoyed", "Anger/Frustrated/Annoyed", "#ff595e"),
            new("Irritated", "Anger/Frustrated/Irritated", "#ff595e"),
            new("Critical", "Anger/Critical", "#ff595e"),
            new("Skeptical", "Anger/Critical/Skeptical", "#ff595e"),
            new("Sarcastic", "Anger/Critical/Sarcastic", "#ff595e"),
            new("Distant", "Anger/Distant", "#ff595e"),
            new("Withdrawn", "Anger/Distant/Withdrawn", "#ff595e"),
            new("Suspicious", "Anger/Distant/Suspicious", "#ff595e"),
            new("Hurt", "Anger/Hurt", "#ff595e"),
            new("Embarrassed", "Anger/Hurt/Embarrassed", "#ff595e"),
            new("Disappointed", "Anger/Hurt/Disappointed", "#ff595e"),
            new("Hostile", "Anger/Hostile", "#ff595e"),
            new("Hateful", "Anger/Hostile/Hateful", "#ff595e"),
            new("Violent", "Anger/Hostile/Violent", "#ff595e"),
            new("Infuriated", "Anger/Infuriated", "#ff595e"),
            new("Enraged", "Anger/Infuriated/Enraged", "#ff595e"),
            new("Furious", "Anger/Infuriated/Furious", "#ff595e"),
            new("Aggressive", "Anger/Aggressive", "#ff595e"),
            new("Provoked", "Anger/Aggressive/Provoked", "#ff595e"),
            new("Mad", "Anger/Aggressive/Mad", "#ff595e"),
            new("Sadness", "Sadness", "#1982c4"),
            new("Guilty", "Sadness/Guilty", "#1982c4"),
            new("Regretful", "Sadness/Guilty/Regretful", "#1982c4"),
            new("Remorseful", "Sadness/Guilty/Remorseful", "#1982c4"),
            new("Abandoned", "Sadness/Abandoned", "#1982c4"),
            new("Ignored", "Sadness/Abandoned/Ignored", "#1982c4"),
            new("Excluded", "Sadness/Abandoned/Excluded", "#1982c4"),
            new("Despair", "Sadness/Despair", "#1982c4"),
            new("Grief", "Sadness/Despair/Grief", "#1982c4"),
            new("Powerless", "Sadness/Despair/Powerless", "#1982c4"),
            new("Depressed", "Sadness/Depressed", "#1982c4"),
            new("Hopeless", "Sadness/Depressed/Hopeless", "#1982c4"),
            new("Empty", "Sadness/Depressed/Empty", "#1982c4"),
            new("Lonely", "Sadness/Lonely", "#1982c4"),
            new("Isolated", "Sadness/Lonely/Isolated", "#1982c4"),
            new("Bored", "Sadness/Bored", "#1982c4"),
            new("Apathetic", "Sadness/Bored/Apathetic", "#1982c4"),
            new("Indifferent", "Sadness/Bored/Indifferent", "#1982c4"),
            new("Tired", "Sadness/Tired", "#1982c4"),
            new("Sleepy", "Sadness/Tired/Sleepy", "#1982c4"),
            new("Unfocused", "Sadness/Tired/Unfocused", "#1982c4"),
            new("Ashamed", "Sadness/Ashamed", "#1982c4"),
            new("Mortified", "Sadness/Ashamed/Mortified", "#1982c4"),
            new("Humiliated", "Sadness/Ashamed/Humiliated", "#1982c4"),
            new("Inferior", "Sadness/Inferior", "#1982c4"),
            new("Inadequate", "Sadness/Inferior/Inadequate", "#1982c4"),
            new("Insecure", "Sadness/Inferior/Insecure", "#1982c4"),
            new("Disappointed", "Sadness/Disappointed", "#1982c4"),
            new("Let down", "Sadness/Disappointed/Let down", "#1982c4"),
            new("Disillusioned", "Sadness/Disappointed/Disillusioned", "#1982c4"),
            new("Surprise", "Surprise", "#ffca3a"),
            new("Amazed", "Surprise/Amazed", "#ffca3a"),
            new("Awe", "Surprise/Amazed/Awe", "#ffca3a"),
            new("Astonished", "Surprise/Amazed/Astonished", "#ffca3a"),
            new("Confused", "Surprise/Confused", "#ffca3a"),
            new("Disillusioned", "Surprise/Confused/Disillusioned", "#ffca3a"),
            new("Perplexed", "Surprise/Confused/Perplexed", "#ffca3a"),
            new("Excited", "Surprise/Excited", "#ffca3a"),
            new("Eager", "Surprise/Excited/Eager", "#ffca3a"),
            new("Energetic", "Surprise/Excited/Energetic", "#ffca3a"),
            new("Startled", "Surprise/Startled", "#ffca3a"),
            new("Shocked", "Surprise/Startled/Shocked", "#ffca3a"),
            new("Dismayed", "Surprise/Startled/Dismayed", "#ffca3a"),
            new("Stimulated", "Surprise/Stimulated", "#ffca3a"),
            new("Inspired", "Surprise/Stimulated/Inspired", "#ffca3a"),
            new("Curious", "Surprise/Stimulated/Curious", "#ffca3a"),
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
