namespace QuarkLang.Core.Runtime;

public class VoidValue : RuntimeValue
{
    public override TypeValue Type()
    {
        return new TypeValue("void");
    }
}