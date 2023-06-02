using System;

namespace QuarkLang.Core.Interpreter.DataTypes;

public abstract class RuntimeValue
{
    public Environment? ptr;
    public string? name;

    public override string ToString()
    {
        if (this is NullValue) return "null";
        else if (this is Int32Value intValue) return intValue.value.ToString().Replace(',', '.');
        else if (this is Float32Value floatValue) return floatValue.value.ToString().Replace(',', '.');
        else if (this is BooleanValue booleaValue) return booleaValue.value.ToString().ToLower();
        else if (this is StringValue stringValue) return $"\"{stringValue.value}\"";
        else if (this is Function fun) return fun.name;
        else if (this is VoidValue) return "void";
        else if (this is ExceptionValue exception) return exception.error;
        else if (this is TypeValue type) return type.type;
        else if (this is Module module) return module.name;
        else if (this is ReferenceValue) return "reference";
        throw new NotImplementedException();
    }
    public string GetValueType()
    {
        if (this is NullValue) return "null";
        else if (this is Int32Value) return "int32";
        else if (this is Float32Value) return "float32";
        else if (this is BooleanValue) return "boolean";
        else if (this is StringValue) return "string";
        else if (this is Function) return "function";
        else if (this is VoidValue) return "void";
        else if (this is ExceptionValue) return "exception";
        else if (this is TypeValue) return "type";
        else if (this is Module) return "module";
        else if (this is ReferenceValue) return "reference";
        throw new NotImplementedException();
    }

    public virtual RuntimeValue AdditionOperator(RuntimeValue right)
        => OperatorExceptionGenerator("+", right);
    public virtual RuntimeValue SubtractionOperator(RuntimeValue right)
        => OperatorExceptionGenerator("-", right);
    public virtual RuntimeValue MultiplicationOperator(RuntimeValue right)
        => OperatorExceptionGenerator("*", right);
    public virtual RuntimeValue DivisionOperator(RuntimeValue right)
        => OperatorExceptionGenerator("/", right);
    public virtual RuntimeValue ModuloOperator(RuntimeValue right)
        => OperatorExceptionGenerator("%", right);

    protected ExceptionValue OperatorExceptionGenerator(string op, RuntimeValue right)
    => new($"Cannot use '{op}' operator with '{GetValueType()}' and '{right.GetValueType()}'");
}