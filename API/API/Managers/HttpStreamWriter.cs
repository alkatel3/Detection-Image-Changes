using API.Model;
using System.Text;

public class HttpStreamWriter : IHttpStreamWriter
{
    private readonly HttpContext _httpContext;

    public HttpStreamWriter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task WriteAsync(Camera base64String)
    {

        _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        _httpContext.Response.Body.SetLength(0);

        var bytes = Encoding.UTF8.GetBytes(base64String.ToString());
        await _httpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        await _httpContext.Response.Body.FlushAsync();
    }
}
