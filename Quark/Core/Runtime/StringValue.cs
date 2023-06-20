namespace QuarkLang.Core.Runtime;

public class StringValue : RuntimeValue
{
    public string _value;

    public StringValue(string value)
    {
        _value = value;
    }

    public override TypeValue Type()
    {
        return new TypeValue("string");
    }

    public override RuntimeValue Add(RuntimeValue right)
    {
        if (right is StringValue str) 
            return new StringValue(_value + str._value);

        return base.Add(right);
    }

    public override RuntimeValue Multiply(RuntimeValue right)
    {
        if (right is Int32Value int32) 
            return new StringValue(string.Concat(System.Linq.Enumerable.Repeat(_value, int32._value)));

        return base.Multiply(right);
    }

    public override RuntimeValue Equal(RuntimeValue right)
    {
        if (right is StringValue str)
            return new BooleanValue(_value == str._value);

        return base.Equal(right);
    }

    public override RuntimeValue NotEqual(RuntimeValue right)
    {
        if (right is StringValue str)
            return new BooleanValue(_value != str._value);

        return base.NotEqual(right);
    }
}