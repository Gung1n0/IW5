namespace IW5_project.API.BL
{
    public class NotFoundException(string message) : Exception(message);
    public class ConflictException(string message) : Exception(message);
}
