namespace QuarkLang.Core.Runtime;

public class ExceptionValue : RuntimeValue
{
    public readonly string error;
    public readonly ExceptionValue? cause;

    public ExceptionValue(string error)
    {
        this.error = error;
    }

    public ExceptionValue(ExceptionValue couse)
    {
        error = "";
        cause = couse;
    }

    public override TypeValue Type()
    {
        return new TypeValue("exception");
    }
}
