namespace Core.Options;

[Option(nameof(TestOptions))]
public class TestOptions
{
    public string? Name { get; set; }
}