namespace QuarkLang.Core.Runtime;

public class Module : RuntimeValue
{
    public new string _name;
    public Environment _env;

    public Module(Environment env, string name)
    {
        _env = env;
        _name = name;
    }

    public override TypeValue Type()
    {
        return new TypeValue("module");
    }
}