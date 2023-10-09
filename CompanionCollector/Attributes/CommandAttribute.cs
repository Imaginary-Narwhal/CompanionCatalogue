using System;

namespace CompanionCollector.Attributes;
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Command { get; }

    public CommandAttribute(string command)
    {
        Command = command;
    }
}