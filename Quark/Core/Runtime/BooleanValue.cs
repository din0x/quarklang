namespace QuarkLang.Core.Runtime;

public class BooleanValue : RuntimeValue
{
    public bool _value;

    public BooleanValue(bool value)
    {
        _value = value;
    }

    public override TypeValue Type()
    {
        return new TypeValue("boolean");
    }

    public override RuntimeValue Equal(RuntimeValue right)
    {
        if (right is BooleanValue boolean)
            return new BooleanValue(_value == boolean._value);

        return base.Equal(right);
    }

    public override RuntimeValue NotEqual(RuntimeValue right)
    {
        if (right is BooleanValue boolean)
            return new BooleanValue(_value != boolean._value);

        return base.NotEqual(right);
    }

    public override RuntimeValue Not()
    {
        return new BooleanValue(!_value);
    }
}