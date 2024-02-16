using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageSender, EmailSender>();
builder.Services.AddScoped<IMessageSender, FacebookSender>();
builder.Services.AddScoped<IMessageSender, SnapchatSender>();
builder.Services.TryAddScoped<IMessageSender, UnregisteredSender>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/single-message/{username}", SendSingleMessage);
app.MapGet("/multi-message/{username}", SendMultiMessage);

app.Run();

string SendSingleMessage(string username, IMessageSender sender)
{
    sender.SendMessage($"Welcome to GroobHouse {username}");
    return "Check the application logs to check what implementation ran!";
}

string SendMultiMessage(string username, IEnumerable<IMessageSender> senders)
{
    foreach (var sender in senders)
    {
        sender.SendMessage($"Welcome to GroobHouse {username}");
    }
    return "Check the application logs to check what implementations ran!";
}

/*
 * An interface is a blueprint for classes to implement the methods, properties and members of a class such that multiple classes can be made for different scenarios
 * Declare interface (used by .Add* to fulfill TService required field)
*/
public interface IMessageSender
{
    public void SendMessage(string message);
}

//write implementations used by .Add* to fulfill TImplementation field)

public class EmailSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"This message is from email: {message}");
    }
}

public class FacebookSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"This message is from Facebook: {message}");
    }
}

public class SnapchatSender : IMessageSender
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"This message is from Snapchat: {message}");
    }
}

public class UnregisteredSender : IMessageSender
{
    public void SendMessage(string message)
    {
        throw new Exception("I'm never registered so shouldn't be called");
    }
}

