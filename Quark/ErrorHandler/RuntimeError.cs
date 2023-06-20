using QuarkLang.Core.Runtime;

namespace QuarkLang.ErrorHandler;

internal class RuntimeError : Error
{
    readonly ExceptionValue exception;

    public RuntimeError(ExceptionValue exception) : base("")
    {
        this.exception = exception;
    }

    public override string ToString()
    {
        ExceptionValue current = exception;
        string str = "";

        while (true)
        {
            if (current.cause is null)
            {
                break;
            }

            current = current.cause;
        }

        str += $"""
                Coused by:
                   {current.error}
                """;


        return str;
    }
}