namespace WebApplication3;

[AttributeUsage(AttributeTargets.Property)]
public class AesAttribute : Attribute
{
    public string key;

    public AesAttribute(string key)
    {
        this.key = key;
    }
}