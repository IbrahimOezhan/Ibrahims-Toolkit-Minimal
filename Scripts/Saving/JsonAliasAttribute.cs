using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class JsonAliasAttribute : Attribute
{
    public string[] Aliases { get; }

    public JsonAliasAttribute(params string[] aliases)
    {
        Aliases = aliases;
    }
}
