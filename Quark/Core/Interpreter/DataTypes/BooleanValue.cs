namespace Quark.Core.Interpreter.DataTypes;

public class BooleanValue : RuntimeValue
{
    public bool value;

    public BooleanValue(bool value)
    {
        this.value = value;
    }
}