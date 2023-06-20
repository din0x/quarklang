namespace QuarkLang.Core.Runtime;

public class Int32Value : RuntimeValue
{
    public int _value;

    public Int32Value(int value)
    {
        _value = value;
    }

    public override TypeValue Type()
    {
        return new TypeValue("int32");
    }

    public override RuntimeValue Add(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value + float32._value);
        else if (right is Int32Value int32) return new Int32Value(_value + int32._value);

        return base.Add(right);
    }

    public override RuntimeValue Subtract(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value - float32._value);
        else if (right is Int32Value int32) return new Int32Value(_value - int32._value);

        return base.Subtract(right);
    }

    public override RuntimeValue Multiply(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value * float32._value);
        else if (right is Int32Value int32) return new Int32Value(_value * int32._value);
        else if (right is StringValue str) 
            return new StringValue(string.Concat(System.Linq.Enumerable.Repeat(str._value, _value)));

        return base.Multiply(right);
    }

    public override RuntimeValue Divide(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value / float32._value);
        else if (right is Int32Value int32) return new Int32Value(_value / int32._value);

        return base.Divide(right);
    }

    public override RuntimeValue Modulo(RuntimeValue right)
    {
        if (right is Float32Value float32) return new Float32Value(_value % float32._value);
        else if (right is Int32Value int32) return new Int32Value(_value % int32._value);

        return base.Modulo(right);
    }

    public override RuntimeValue Plus()
    {
        return new Int32Value(_value);
    }

    public override RuntimeValue Minus()
    {
        return new Int32Value(-_value);
    }
}