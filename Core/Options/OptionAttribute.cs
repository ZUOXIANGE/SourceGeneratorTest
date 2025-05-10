namespace Core.Options;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class OptionAttribute(string? section = null) : Attribute
{
    public string? Section { get; } = section;
}