namespace QuarkLang.Core.Interpreter.DataTypes;

public class TypeValue : RuntimeValue
{
    public readonly string type;

    public TypeValue(RuntimeValue obj)
    {
        type = obj.GetValueType();
    }

    public TypeValue(string type)
    {
        this.type = type;
    }

    public static bool operator ==(TypeValue left, TypeValue right) => left.type == right.type;
    public static bool operator !=(TypeValue left, TypeValue right) => left.type != right.type;

    public override bool Equals(object? obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }
}