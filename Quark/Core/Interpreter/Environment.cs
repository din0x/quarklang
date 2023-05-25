using System.Collections.Generic;
using Quark.Core.Interpreter.DataTypes;

namespace Quark.Core.Interpreter;

public class Environment
{
    private readonly Environment? parent;
    private readonly Dictionary<string, RuntimeValue> variables;
    private readonly Dictionary<string, TypeValue> types;
    private readonly Dictionary<string, bool> constants;

    public Environment(Environment? parentENV = null)
    {
        parent = parentENV;
        variables = new Dictionary<string, RuntimeValue>();
        types = new Dictionary<string, TypeValue>();
        constants = new Dictionary<string, bool>();
    }

    public RuntimeValue? DeclareVariable(string varname, RuntimeValue value, bool constant)
    {
        if (variables.ContainsKey(varname))
        {
            return new ExceptionValue($"Cannot declare '{varname}' as it is already defined");
        }
        constants[varname] = constant;
        value.ptr = this;
        value.name = varname;
        variables[varname] = value;
        types[varname] = new TypeValue(value);

        return null;
    }

    public RuntimeValue? AssignVariable(string varname, RuntimeValue value)
    {
        var env = Resolve(varname);

        if (env == null) 
            return new ExceptionValue($"Cannot resolve '{varname}' as it is undefined");

        if (env.constants[varname])
            return new ExceptionValue($"Cannot assign '{varname}' as it is constant");
        

        if (env.types[varname] != new TypeValue(value))
            return new ExceptionValue($"Cannot convert '{value.GetValueType()}' to '{types[varname].type}'");
        

        env.variables[varname] = value;
        return null;
    }

    public RuntimeValue LookupVariable(string varname)
    {
        var env = Resolve(varname);

        if (env  == null)
        {
            return new ExceptionValue($"Cannot resolve '{varname}' as it is undefined");
        }

        var value = env.variables[varname];

        value.ptr = env;
        value.name = varname;

        return value;
    }

    public Environment? Resolve(string varname)
    {
        if (variables.ContainsKey(varname)) return this;
        else if (parent == null)
        {
            return null;
        }

        return parent.Resolve(varname);
    }
}