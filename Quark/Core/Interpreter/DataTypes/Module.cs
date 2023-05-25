namespace Quark.Core.Interpreter.DataTypes;

public class Module : RuntimeValue
{
    public new string name;
    public Environment env;

    public Module(Environment env, string name)
    {
        this.env = env;
        this.name = name;
    }
}