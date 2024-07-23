using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace Sharpist.Server.Service.IServices;
public interface IPdfToTextService
{
    Task<string> PdfToTextAsync(PdfDocument pdf);
}
