namespace IW5_project.IW5_project.Api.BL
{
    public class NotFoundException(string message) : Exception(message);
    public class ConflictException(string message) : Exception(message);
}
