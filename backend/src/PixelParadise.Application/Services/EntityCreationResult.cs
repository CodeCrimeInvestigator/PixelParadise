namespace PixelParadise.Application.Services;

public class EntityCreationResult<T> where T : class
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Entity { get; set; }
}