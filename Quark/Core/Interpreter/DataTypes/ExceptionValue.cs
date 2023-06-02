namespace QuarkLang.Core.Interpreter.DataTypes;

public class ExceptionValue : RuntimeValue
{
    public string error;
    public ExceptionValue? cause;

    public ExceptionValue(string error)
    {
        this.error = error;
    }

    public ExceptionValue(ExceptionValue couse)
    {
        error = "";
        cause = couse;
    }
}
