using System;

public class Savable
{
    public string fullName;

    public Savable()
    {
        Type ty = this.GetType();

        string assemblyName = ty.Assembly.GetName().Name;

        string qualifiedName = $"{ty.FullName}, {assemblyName}";

        fullName = qualifiedName;
    }
}
