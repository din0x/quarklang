namespace QuarkLang.Core.Runtime;

public abstract class RuntimeValue
{
    public Environment? _ptr;
    public string? _name;

    public abstract TypeValue Type();

    public virtual RuntimeValue Add(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("+", right);
    public virtual RuntimeValue Subtract(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("-", right);
    public virtual RuntimeValue Multiply(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("*", right);
    public virtual RuntimeValue Divide(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("/", right);
    public virtual RuntimeValue Modulo(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("%", right);
    public virtual RuntimeValue Equal(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("==", right);
    public virtual RuntimeValue NotEqual(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("!=", right);
    public virtual RuntimeValue Greater(RuntimeValue right)
        => BinaryOperatorExceptionGenerator(">", right);
    public virtual RuntimeValue Less(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("<", right);
    public virtual RuntimeValue GreaterOrEqual(RuntimeValue right)
        => BinaryOperatorExceptionGenerator(">=", right);
    public virtual RuntimeValue LessOrEqual(RuntimeValue right)
        => BinaryOperatorExceptionGenerator("<=", right);

    private ExceptionValue BinaryOperatorExceptionGenerator(string op, RuntimeValue right)
        => new($"Cannot use '{op}' operator with '{Type()}' and '{right.Type()}'");

    public virtual RuntimeValue Not()
        => UnaryOperatorExceptionGenerator("!");
    public virtual RuntimeValue Plus()
        => UnaryOperatorExceptionGenerator("+");
    public virtual RuntimeValue Minus()
        => UnaryOperatorExceptionGenerator("-");

    private ExceptionValue UnaryOperatorExceptionGenerator(string op)
        => new($"Cannot use '{op}' operator with '{Type()}'");
}