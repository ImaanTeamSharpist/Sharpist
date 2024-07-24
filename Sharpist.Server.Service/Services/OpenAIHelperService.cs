using OpenAI.API;
using OpenAI.API.Completions;
using Sharpist.Server.Service.IServices;

namespace Sharpist.Server.Service.Services;

public class OpenAIHelperService : IOpenAIHelperService
{
    private readonly OpenAIAPI _api;

    public OpenAIHelperService()
    {
        _api = new OpenAIAPI("sk - proj - Ysm9ladXYIAWjCoLuDjBT3BlbkFJoyrPzaAcvTPDBWnAcTyO");
    }

    public async Task<string> CheckResumeCompatibility(string resumeText, string requirements)
    {
        var prompt = $"Given the resume: {resumeText}\nAnd the job requirements: {requirements}\nCheck the compatibility between them. Tell only percentages that this candidite matches with our vacancy. If percentage high than 60%, then send me percentage only, if less than 60% just do not send nothing send the object null";
        var completionRequest = new CompletionRequest()
        {
            Prompt = prompt,
            MaxTokens = 500
        };

        var result = await _api.Completions.CreateCompletionAsync(completionRequest);
        return result.Completions.FirstOrDefault()?.Text;
    }
}
