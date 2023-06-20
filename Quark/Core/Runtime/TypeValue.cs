namespace QuarkLang.Core.Runtime;

public class TypeValue : RuntimeValue
{
    public readonly string _type;

    public TypeValue(string type)
    {
        _type = type;
    }

    public TypeValue(RuntimeValue value)
    {
        _type = value.Type()._type;
    }

    public override TypeValue Type()
    {
        return new TypeValue("type");
    }
}