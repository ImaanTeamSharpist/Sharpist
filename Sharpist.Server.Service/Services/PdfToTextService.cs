namespace Sharpist.Server.Service.Services;

using Sharpist.Server.Service.IServices;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

public class PdfToTextService : IPdfToTextService
{
    public async Task<string> PdfToTextAsync(PdfDocument pdf)
    {
        StringBuilder pageText = new StringBuilder();

        foreach (Page page in pdf.GetPages())
        {
            foreach (Word word in page.GetWords())
            {
                pageText.Append(word.Text + " ");
            }
        }

        return await Task.FromResult(pageText.ToString());
    }
}

