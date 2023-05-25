namespace Quark.Core.Interpreter.DataTypes;

public class ReferenceValue : RuntimeValue
{
    public RuntimeValue value;

    public ReferenceValue(RuntimeValue value)
    {
        this.value = value;
    }
}