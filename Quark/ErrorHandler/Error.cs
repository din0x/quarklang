namespace Quark.ErrorHandler;

public abstract class Error
{
    protected string error;

    public Error(string error)
    {
        this.error = error;
    }

    public abstract override string ToString();
}