namespace Quark.Core.Interpreter.DataTypes;

public class Int32Value : RuntimeValue
{
    public int value;

    public Int32Value(int value)
    {
        this.value = value;
    }

    public override RuntimeValue AdditionOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value + float32.value);
        else if (right is Int32Value int32) return new Int32Value(value + int32.value);

        return base.AdditionOperator(right);
    }

    public override RuntimeValue SubtractionOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value - float32.value);
        else if (right is Int32Value int32) return new Int32Value(value - int32.value);

        return base.SubtractionOperator(right);
    }

    public override RuntimeValue MultiplicationOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value * float32.value);
        else if (right is Int32Value int32) return new Int32Value(value * int32.value);
        else if (right is StringValue str) 
            return new StringValue(string.Concat(System.Linq.Enumerable.Repeat(str.value, value)));

        return base.MultiplicationOperator(right);
    }

    public override RuntimeValue DivisionOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value / float32.value);
        else if (right is Int32Value int32) return new Int32Value(value / int32.value);

        return base.DivisionOperator(right);
    }

    public override RuntimeValue ModuloOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value % float32.value);
        else if (right is Int32Value int32) return new Int32Value(value % int32.value);

        return base.ModuloOperator(right);
    }
}