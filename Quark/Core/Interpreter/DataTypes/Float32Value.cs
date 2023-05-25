namespace Quark.Core.Interpreter.DataTypes;

public class Float32Value : RuntimeValue
{
    public float value;

    public Float32Value(float value)
    {
        this.value = value;
    }

    public override RuntimeValue AdditionOperator(RuntimeValue right)
    {
        if (right is  Float32Value float32) return new Float32Value(value + float32.value);
        else if (right is  Int32Value int32) return new Float32Value(value + int32.value);

        return base.AdditionOperator(right);
    }

    public override RuntimeValue SubtractionOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value - float32.value);
        else if (right is Int32Value int32) return new Float32Value(value - int32.value);

        return base.SubtractionOperator(right);
    }

    public override RuntimeValue DivisionOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value / float32.value);
        else if (right is Int32Value int32) return new Float32Value(value / int32.value);

        return base.DivisionOperator(right);
    }

    public override RuntimeValue MultiplicationOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value * float32.value);
        else if (right is Int32Value int32) return new Float32Value(value * int32.value);

        return base.MultiplicationOperator(right);
    }

    public override RuntimeValue ModuloOperator(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(value % float32.value);
        else if (right is Int32Value int32) return new Float32Value(value % int32.value);

        return base.ModuloOperator(right);
    }
}
