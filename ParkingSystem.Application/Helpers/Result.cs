namespace ParkingSystem.Application.Helpers;

public class Result<T>
{
    public T? Value { get; private set; }
    public string Message { get; private set; }

    public Result(T? value, string message)
    {
        Value = value;
        Message = message;
    }
}