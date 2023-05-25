using Quark.Core.Interpreter.DataTypes;
using Quark.Core.Parser.AST;
using Quark.ErrorHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quark.Core.Interpreter;

public class Interpreter
{
    private readonly string path;
    private static Dictionary<string, Module> imported = new();

    public int ExitCode { get; }
    public RuntimeValue? LastEvaluated { get; }

    public Interpreter(ProgramUnit program, string path, Environment? env = null)
    {
        this.path = path;
        env ??= new Environment();

        var module = new Module(env, path.Split("\\").Last().Split('.')[0]);
        imported.Add(path, module);
        foreach (var stmt in program.body)
        {
            LastEvaluated = EvaluateStatement(stmt, env);

            if (LastEvaluated is ExceptionValue exception)
            {
                ExitCode = -1;
                ErrorLoger.Log(new RuntimeError(exception));
                return;
            }
        }

        ExitCode = 0;
    }

    private RuntimeValue? EvaluateStatement(Statement stmt, Environment env)
    {
        if (stmt is ImportStatement importStatement) return EvaluateImportStatement(importStatement, env);
        else if (stmt is VariableDeclaration variableDeclaration) return EvaluateVariableDeclaration(variableDeclaration, env);
        else if (stmt is VariableAssignment variableAssignment) return EvaluateVariableAssignment(variableAssignment, env);
        else if (stmt is FunctionDeclaration functionDeclaration) return EvaluateFunctionDeclaration(functionDeclaration, env);
        else if (stmt is ReturnStatement returnStmt) return EvaluateReturnStmt(returnStmt, env);
        else if (stmt is ConditionalStatement ifElseStmt) return EvaluateIfElseStmt(ifElseStmt, env);
        else if (stmt is WhileLoop whileLoop) return EvaluateWhileLoop(whileLoop, env);
        else if (stmt is CallExpression functionCall) return EvaluateCallExpr(functionCall, env);

        else if (stmt is Expression expr) return EvaluateExpression(expr, env);

        Console.WriteLine("This statement has not yet been setup for interpretation. " + stmt.GetType());
        throw new NotImplementedException();
    }

    private RuntimeValue? EvaluateImportStatement(ImportStatement stmt, Environment env)
    {
        if (path == "console") 
            return new ExceptionValue("Cannot use import statement when using console");

        string modulePath = Path.GetDirectoryName(path) ?? "";
        string extension = Path.GetExtension(path);

        modulePath += "\\" + stmt.module;

        modulePath += extension;

        if (!File.Exists(modulePath))
            return new ExceptionValue($"Cannot find {modulePath}");

        //Console.WriteLine($"Importing: {stmt.module} from {modulePath}");

        if (imported.TryGetValue(modulePath, out Module value))
        {
            //Console.WriteLine($"Already imported {stmt.module} from {modulePath}");
            return env.DeclareVariable(stmt.module, value, true);
        }
        

        string code = File.ReadAllText(modulePath);
        var lexer = new Lexer.Lexer(code, modulePath);
        var parser = new Parser.Parser(lexer.Tokens, modulePath);
        if (parser.ErrorEncountered)
            return new ExceptionValue($"Cannot import {modulePath}");

        var newEnv = new Environment();

        var interpreter = new Interpreter(parser.Program, modulePath, newEnv);
        if (interpreter.ExitCode != 0) 
            return new ExceptionValue($"Cannot import {modulePath}");

        var module = new Module(newEnv, stmt.module);
        return env.DeclareVariable(stmt.module, module, true);
    }

    private RuntimeValue? EvaluateFunctionDeclaration(FunctionDeclaration stmt, Environment env)
    {
        var fun = new Function(stmt.name, stmt.body, stmt.args, stmt.returnType);

        var result = env.DeclareVariable(stmt.name, fun, true);

        if (result is ExceptionValue exception)
            return new ExceptionValue(exception);

        return null;
    }

    private RuntimeValue? EvaluateVariableDeclaration(VariableDeclaration varDeclaration, Environment env)
    {
        var value = EvaluateExpression(varDeclaration.value, env);
        if (value is ExceptionValue exception1)
            return new ExceptionValue(exception1);

        var result = env.DeclareVariable(varDeclaration.name, value, varDeclaration.constant);
        if (result is ExceptionValue exception2)
            return new ExceptionValue(exception2);

        return null;
    }

    private RuntimeValue? EvaluateVariableAssignment(VariableAssignment assignment, Environment env)
    {
        var variable = EvaluateExpression(assignment.variable, env);
        if (variable is ExceptionValue exception1)
            return new ExceptionValue(exception1);
        //Console.WriteLine("var name: " + variable.name);
        string name = variable.name; // i dont know why, but without this it doesn't work

        var value = EvaluateExpression(assignment.value, env);
        if (value is ExceptionValue exception2)
            return new ExceptionValue(exception2);
        //Console.WriteLine("val name: " + variable.name);

        if (variable.ptr == null || name == null)
            return new ExceptionValue("Cannot assign");
        //Console.WriteLine(variable.GetHashCode());

        var varEnv = variable.ptr;
        var result = varEnv.AssignVariable(name, value);
        if (result is ExceptionValue exception3)
            return new ExceptionValue(exception3);

        //Console.WriteLine(value.ToString());
        //Console.WriteLine(varEnv.LookupVariable(name));
        //Console.WriteLine(value.ptr.LookupVariable(name));
        //Console.WriteLine(name);

        return null;
    }

    private RuntimeValue? EvaluateReturnStmt(ReturnStatement stmt, Environment env)
    {
        RuntimeValue result = new VoidValue();

        if (stmt.expr != null)
            result = EvaluateExpression(stmt.expr, env);

        if (result is ExceptionValue exception)
            return new ExceptionValue(exception);

        result.ptr = null;
        result.name = null;

        return result;
    }

    private RuntimeValue? EvaluateIfElseStmt(ConditionalStatement stmt, Environment env)
    {
        for (int i = 0; i < stmt.conditions.Length && i < stmt.blocks.Count; i++)
        {
            var result = EvaluateExpression(stmt.conditions[i], env);

            if (result is ExceptionValue exception)
                return new ExceptionValue(exception);

            if (result is not BooleanValue boolean)
                return new ExceptionValue($"Cannot convert '{result.GetValueType()}' to boolean");
            
            if (boolean.value)
            {
                var newEnv = new Environment(env);
                foreach (var s in stmt.blocks[i])
                {
                    var v = EvaluateStatement(s, newEnv);

                    if (v is ExceptionValue exception1)
                        return new ExceptionValue(exception1);

                    if (v is not null)
                    {
                        return v;
                    }
                }
                return null;
            }
        }
        if (stmt.falseBlock != null)
        {
            var newEnv = new Environment(env);
            foreach (var s in stmt.falseBlock)
            {
                var v = EvaluateStatement(s, newEnv);
                if (v is not null)
                {
                    return v;
                }
            }
        }

        return null;
    }

    private RuntimeValue? EvaluateWhileLoop(WhileLoop whileLoop, Environment env)
    {
        while (true)
        {
            var result = EvaluateExpression(whileLoop.condition, env);

            if (result is ExceptionValue exception)
                return new ExceptionValue(exception);

            if (result is not BooleanValue boolean)
                return new ExceptionValue($"Cannot convert '{result.GetValueType()}' to boolean");
            
            if (!boolean.value) 
                return null;

            var newEnv = new Environment(env);
            foreach (var s in whileLoop.body)
            {
                var v = EvaluateStatement(s, newEnv);
                if (v is not null) return v;
            }
        }
    }

    private RuntimeValue EvaluateExpression(Expression expr, Environment env)
    {
        if (expr is UnaryExpression unaryExpr) return EvaluateUnaryExpr(unaryExpr, env);
        else if (expr is Identifier iden) return EvaluateIdentifier(iden, env);
        else if (expr is CallExpression functionCall) return EvaluateCallExpr(functionCall, env) ?? new VoidValue();
        else if (expr is MemberExpression member) return EvaluateMemberExpr(member, env);
        else if (expr is NullLiteral) return new NullValue();
        else if (expr is Int32Literal int32Literal) return new Int32Value(int32Literal.value);
        else if (expr is Float32Literal float32Literal) return new Float32Value(float32Literal.value);
        else if (expr is BooleanLiteral booleanLiteral) return new BooleanValue(booleanLiteral.value);
        else if (expr is StringLiteral stringLiteral) return new StringValue(stringLiteral.value);
        else if (expr is ArithmeticExpression arithmeticExpr) return EvaluateArithmeticExpr(arithmeticExpr, env);
        else if (expr is RelationalExpression relationalExpr) return EvaluateRelationalExpr(relationalExpr, env);
        else if (expr is LogicalExpression logicalExpr) return EvaluateLogicalExpr(logicalExpr, env);

        Console.WriteLine("This expression has not yet been setup for interpretation. " + expr.GetType());
        throw new NotImplementedException();
    }

    private RuntimeValue EvaluateLogicalExpr(LogicalExpression expr, Environment env)
    {
        var left = EvaluateExpression(expr.left, env);

        if (left is ExceptionValue _exception)
            return new ExceptionValue(_exception);

        var right = EvaluateExpression(expr.right, env);

        if (right is ExceptionValue exception)
            return new ExceptionValue(exception);

        var op = expr.type;

        if (left is BooleanValue b1 && right is BooleanValue b2)
        {
            if (op == LogicalExpression.Type.And) return new BooleanValue(b1.value && b2.value);
            if (op == LogicalExpression.Type.Or) return new BooleanValue(b1.value || b2.value);
        }
        return new ExceptionValue($"Cannot use '{(op.ToString() == "And" ? "&&" : "||")}' as both sides must be boolean");
    }

    private RuntimeValue EvaluateRelationalExpr(RelationalExpression expr, Environment env)
    {
        var left = EvaluateExpression(expr.left, env);

        if (left is ExceptionValue _exception)
            return new ExceptionValue(_exception);

        var right = EvaluateExpression(expr.right, env);

        if (right is ExceptionValue exception)
            return new ExceptionValue(exception);

        var op = expr.op;

        if (left is Int32Value _int32)
        {
            if (right is Int32Value int32)
            {
                if (op == "==") return new BooleanValue(_int32.value == int32.value);
                if (op == "!=") return new BooleanValue(_int32.value != int32.value);
                if (op == ">") return new BooleanValue(_int32.value > int32.value);
                if (op == ">=") return new BooleanValue(_int32.value >= int32.value);
                if (op == "<") return new BooleanValue(_int32.value < int32.value);
                if (op == "<=") return new BooleanValue(_int32.value <= int32.value);
            }
            if (right is NullValue)
            {
                return new BooleanValue(op == "!=");
            }
        }
        if (left is Float32Value _float32)
        {
            if (right is Float32Value float32)
            {
                if (op == "==") return new BooleanValue(_float32.value == float32.value);
                if (op == "!=") return new BooleanValue(_float32.value != float32.value);
                if (op == ">") return new BooleanValue(_float32.value > float32.value);
                if (op == ">=") return new BooleanValue(_float32.value >= float32.value);
                if (op == "<") return new BooleanValue(_float32.value < float32.value);
                if (op == "<=") return new BooleanValue(_float32.value <= float32.value);
            }
            if (right is NullValue)
            {
                return new BooleanValue(op == "!=");
            }
        }
        else if (left is StringValue _str)
        {
            if (right is StringValue str)
            {
                if (op == "==") return new BooleanValue(_str.value == str.value);
                if (op == "!=") return new BooleanValue(_str.value != str.value);
            }
            if (right is NullValue)
            {
                return new BooleanValue(op == "!=");
            }
        }
        else if (left is BooleanValue _boolean)
        {
            if (right is BooleanValue boolean)
            {
                if (op == "==") return new BooleanValue(_boolean.value == boolean.value);
                if (op == "!=") return new BooleanValue(_boolean.value != boolean.value);
            }
        }
        else if (left is NullValue)
        {
            if (right is NullValue)
                return new BooleanValue(op == "==");
            else
                return new BooleanValue(op == "!=");
        }
        return new ExceptionValue($"Cannot compare '{left.GetValueType()}' and '{right.GetValueType()}' using '{expr.op}'");
    }

    private RuntimeValue EvaluateUnaryExpr(UnaryExpression expr, Environment env)
    {
        var value = EvaluateExpression(expr.value, env);

        if (value is ExceptionValue exception)
            return new ExceptionValue(exception);

        if (expr.op == "!" && value is BooleanValue boolean) return new BooleanValue(!boolean.value);
        if (expr.op == "+" && value is Int32Value) return value;
        if (expr.op == "-" && value is Int32Value int32) return new Float32Value(-int32.value);
        if (expr.op == "+" && value is Float32Value) return value;
        if (expr.op == "-" && value is Float32Value float32) return new Float32Value(-float32.value);

        return new ExceptionValue($"Cannot use '{expr.op}' with '{value.GetValueType()}'");
    }

    private RuntimeValue? EvaluateCallExpr(CallExpression expr, Environment env)
    {
        if (expr.expr is Identifier iden)
        {
            if (iden.symbol == "print")
            {
                var v = EvaluateExpression(expr.args[0], env);
                if (v is ExceptionValue exception)
                    return new ExceptionValue(exception);

                if (v is StringValue str)
                {
                    Console.WriteLine(str.value);
                    return null;
                }

                Console.WriteLine(v);
                return null;
            }
            else if (iden.symbol == "type")
            {
                var v = EvaluateExpression(expr.args[0], env);
                if (v is ExceptionValue exception)
                    return new ExceptionValue(exception);

                return expr.useAsStmt ? null : new TypeValue(v);
            }
            else if (iden.symbol == ".len")
            {
                var v = EvaluateExpression(expr.args[0], env);

                if (v is StringValue str)
                    return expr.useAsStmt ? null : new Int32Value(str.value.Length);

                Console.WriteLine(".len");
                return null;
            }
        }

        RuntimeValue val = EvaluateExpression(expr.expr, env);

        if (val is ExceptionValue exception1)
            return new ExceptionValue(exception1);

        else if (val is Function function)
        {
            if (expr.args.Length != function.args.Length)
                return new ExceptionValue($"Function '{function.name}' does not take {expr.args.Length} arguments");

            Environment newEnv = new(env);

            for (int i = 0; i < function.args.Length; i++)
            {
                var value = EvaluateExpression(expr.args[i], env);

                if (value is ExceptionValue exception2)
                    return new ExceptionValue(exception2);

                var argtype = function.args[i].dataType;

                if (value.GetValueType() != argtype.type)
                    return new ExceptionValue($"'{function.name}' does not take '{value.GetValueType()}' as paremether {i}, expected '{argtype.type}'");


                else if (newEnv.DeclareVariable(function.args[i].name, value, false) is ExceptionValue exception3)
                    return new ExceptionValue(exception3);
            }

            foreach (Statement stmt in function.body)
            {
                var v = EvaluateStatement(stmt, newEnv);
                if (v is not null)
                {
                    if (v is ExceptionValue exception)
                        return new ExceptionValue(exception);
                    

                    else if (v.GetValueType() != function.returnType.type)
                    {
                        return new ExceptionValue($"Cannot convert '{v.GetValueType()} to '{function.returnType.type}'");
                    }
                    //Console.WriteLine("Result of call: " + v.ToString());
                    return expr.useAsStmt ? null : v;
                }
            }
            if (function.returnType.type != new VoidValue().GetValueType())
            {
                return new ExceptionValue($"'{function.name}': not all code paths return a value");
            } 
            else
            {
                return expr.useAsStmt ? null : new VoidValue();
            }
        }
        return new ExceptionValue($"Cannot call {val}");
    }

    private RuntimeValue EvaluateIdentifier(Identifier iden, Environment env)
    {
        var val = env.LookupVariable(iden.symbol);
        //Console.WriteLine($"{iden.symbol} : {val.GetHashCode()} : {val.ToString()}");
        if (val is ExceptionValue exception)
            return new ExceptionValue(exception);

        return val;
    }

    private RuntimeValue EvaluateMemberExpr(MemberExpression expr, Environment env)
    {
        var parent = EvaluateExpression(expr.parent, env);

        if (parent is ExceptionValue exception)
            return new ExceptionValue(exception);

        else if (parent is Module module)
            return module.env.LookupVariable(expr.member);

        else if (parent is StringValue str)
        {
            if (expr.member == "len") 
            {
                var body = new Statement[] { new CallExpression(new Identifier(".len"), new Expression[] {new StringLiteral(str.value)}, false) };
                return new Function(".len", body, Array.Empty<FunctionDeclaration.Argument>(), new TypeValue(new Int32Value(0)));
            }
        }

        return new ExceptionValue($"'{parent.GetValueType()}' does not contain definition for {expr.member}");
    }

    private RuntimeValue EvaluateArithmeticExpr(ArithmeticExpression expr, Environment env)
    {
        var left = EvaluateExpression(expr.left, env);
        if (left is ExceptionValue _exception) 
            return new ExceptionValue(_exception);

        var right = EvaluateExpression(expr.right, env);
        if (right is ExceptionValue exception)
            return new ExceptionValue(exception);

        var op = expr.op;

        if (op == "+") return left.AdditionOperator(right);
        else if (op == "-") return left.SubtractionOperator(right);
        else if (op == "*") return left.MultiplicationOperator(right);
        else if (op == "/") return left.DivisionOperator(right);
        else if (op == "%") return left.ModuloOperator(right);
        
        return new ExceptionValue($"Cannot use '{expr.op}' operator with '{left.GetValueType()}' and '{right.GetValueType()}'");
    }
}