using API.Model;
using System.Threading.Tasks;

public interface IHttpStreamWriter
{
    Task WriteAsync(Camera base64String);
}
