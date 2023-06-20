using System.IO;
using System.Linq;

namespace QuarkLang.Core.ErrorHandler;

public class SyntaxError : Error
{
    private readonly uint _character;
    private readonly string _file;

    public SyntaxError(string error, uint character, string file) : base(error)
    {
        base.error = error;
        _character = character;
        _file = file;
    }

    public override string ToString()
    {
        return "";
    }
}