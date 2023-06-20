namespace QuarkLang.Core.Runtime;

public class Float32Value : RuntimeValue
{
    public float _value;

    public Float32Value(float value)
    {
        _value = value;
    }

    public override TypeValue Type()
    {
        return new TypeValue("float32");
    }

    public override RuntimeValue Add(RuntimeValue right)
    {
        if (right is  Float32Value float32) return new Float32Value(_value + float32._value);
        else if (right is  Int32Value int32) return new Float32Value(_value + int32._value);

        return base.Add(right);
    }

    public override RuntimeValue Subtract(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value - float32._value);
        else if (right is Int32Value int32) return new Float32Value(_value - int32._value);

        return base.Subtract(right);
    }

    public override RuntimeValue Divide(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value / float32._value);
        else if (right is Int32Value int32) return new Float32Value(_value / int32._value);

        return base.Divide(right);
    }

    public override RuntimeValue Multiply(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value * float32._value);
        else if (right is Int32Value int32) return new Float32Value(_value * int32._value);

        return base.Multiply(right);
    }

    public override RuntimeValue Modulo(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value % float32._value);
        else if (right is Int32Value int32) return new Float32Value(_value % int32._value);

        return base.Modulo(right);
    }

    public override RuntimeValue Plus()
    {
        return new Float32Value(_value);
    }

    public override RuntimeValue Minus()
    {
        return new Float32Value(-_value);
    }

    public override RuntimeValue Equal(RuntimeValue right)
    {
        if (right is Float32Value float32)
            return new BooleanValue(_value == float32._value);
        if (right is Float32Value int32)
            return new BooleanValue(_value == int32._value);

        return base.Equal(right);
    }

    public override RuntimeValue NotEqual(RuntimeValue right)
    {
        if (right is Float32Value float32)
            return new BooleanValue(_value != float32._value);
        if (right is Int32Value int32)
            return new BooleanValue(_value != int32._value);

        return base.NotEqual(right);
    }

    public override RuntimeValue Greater(RuntimeValue right)
    {
        if (right is Float32Value float32)
            return new BooleanValue(_value > float32._value);
        if (right is Int32Value int32)
            return new BooleanValue(_value > int32._value);

        return base.Greater(right);
    }

    public override RuntimeValue Less(RuntimeValue right)
    {
        if (right is Float32Value float32)
            return new BooleanValue(_value < float32._value);
        if (right is Int32Value int32)
            return new BooleanValue(_value < int32._value);

        return base.Less(right);
    }

    public override RuntimeValue GreaterOrEqual(RuntimeValue right)
    {
        if (right is Float32Value float32)
            return new BooleanValue(_value >= float32._value);
        if (right is Int32Value int32)
            return new BooleanValue(_value >= int32._value);

        return base.GreaterOrEqual(right);
    }

    public override RuntimeValue LessOrEqual(RuntimeValue right)
    {
        if (right is Float32Value float32)
            return new BooleanValue(_value <= float32._value);
        if (right is Int32Value int32)
            return new BooleanValue(_value <= int32._value);

        return base.LessOrEqual(right);
    }
}
