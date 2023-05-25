namespace Quark.Core.Interpreter.DataTypes;

public class StringValue : RuntimeValue
{
    public string value;

    public StringValue(string value)
    {
        this.value = value;
    }

    public override RuntimeValue AdditionOperator(RuntimeValue right)
    {
        if (right is StringValue str) 
            return new StringValue(value + str.value);

        return base.AdditionOperator(right);
    }

    public override RuntimeValue MultiplicationOperator(RuntimeValue right)
    {
        if (right is Int32Value int32) 
            return new StringValue(string.Concat(System.Linq.Enumerable.Repeat(value, int32.value)));

        return base.MultiplicationOperator(right);
    }
}