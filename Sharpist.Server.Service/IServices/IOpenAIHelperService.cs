namespace Sharpist.Server.Service.IServices;

public interface IOpenAIHelperService
{
    Task<string> CheckResumeCompatibility(string resumeText, string requirements);
}
