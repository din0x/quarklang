using QuarkLang.Core.AST;
using QuarkLang.Core.AST.Utils;
using QuarkLang.Core.CompileTime.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace QuarkLang.Core.CompileTime;

public class Parser
{
    public static Unit Parse(Token[] tokens)
    {
        Parser parser = new(tokens);
        parser.Parse();

        return parser._program;
    }

    private Parser(Token[] tokens)
    {
        _tokens = new List<Token>(tokens);
        eof = _tokens.Last();
        _program = Parse();
    }

    private Unit _program;

    private readonly List<Token> _tokens;

    private readonly Token eof;

    private bool NotEOF => _tokens.Count > 0 && _tokens[0]._type != TokenType.EOF;

    private Token At()
    {
        if (NotEOF)
            return _tokens[0];

        return eof;
    }

    private bool At(TokenType tokentype)
    {
        return At()._type == tokentype;
    }

    private bool Expect(TokenType tokentype)
    {
        if (At(tokentype))
            return true;

        Console.WriteLine($"""
            Syntax Error:
                Unexpected token: {At()}
                Expected: {tokentype}
            """);

        return false;
    }

    private Token Eat()
    {
        if (NotEOF)
        {
            var at = At();
            _tokens.RemoveAt(0);
            return at;
        }
        return eof;
    }

    private Unit Parse()
    {
        var statements = new List<Statement>();
        while (NotEOF)
        {
            statements.Add(ParseStatement());
        }
        return new Unit(statements.ToArray());
    }

    private Statement ParseStatement()
    {
        return ParseClassDeclaration();
    }

    private Statement ParseClassDeclaration()
    {
        if (!At(TokenType.Class))
            return ParseFunctionDeclaration();

        /*
        class Person {
            _name: string,
            _age: int32,
            
            fn say_hello() {
                
            }
        }
        */

        Eat();
        Expect(TokenType.Identifier);
        var name = Eat()._value;

        if (Expect(TokenType.OpenBrace))
            Eat();

        var methods = new List<Function>();
        var fields = new List<Field>();
        while (NotEOF && !At(TokenType.CloseBrace))
        {
            if (At(TokenType.Fn))
            {
                var function = ParseFunction();
                methods.Add(function);
            }
            else
            {
                /*
                name: string = "name",
                */

                Expect(TokenType.Identifier);
                var fieldName = Eat()._value;

                Expect(TokenType.Colon);
                Eat();

                var type = ParseType();

                Expression? defaultValue = null;
                if (At(TokenType.EqualSign))
                {
                    Eat();
                    defaultValue = ParseExpression();
                }

                fields.Add(new(fieldName, type, defaultValue));

                if (!At(TokenType.CloseBrace))
                {
                    if (Expect(TokenType.Comma))
                        Eat();
                }
            }

            if (At(TokenType.CloseBrace))
                Eat();
        }

        return new ClassDeclaration(name, fields.ToArray(), methods.ToArray());
    }

    private Statement ParseFunctionDeclaration()
    {
        if (!At(TokenType.Fn))
            return ParseVariableDeclaration();

        var function = ParseFunction();

        return new FunctionDeclaration(function);
    }

    private Statement ParseVariableDeclaration()
    {
        if (!(At()._type is TokenType.Var or TokenType.Const))
            return ParseExpressionStatement();

        /*
        var x: int32 = 1;
        const pi: float32 = 3.14;

        note: type is not necessary
        */

        bool @const = false;
        if (At(TokenType.Const))
            @const = true;
        Eat();

        Expect(TokenType.Identifier);
        var name = Eat()._value;

        Expression? type = null;
        if (At(TokenType.Colon))
        {
            Eat();
            type = ParseType();
        }

        Expect(TokenType.EqualSign);
        Eat();
        var value = ParseExpression();

        if (Expect(TokenType.Semicolon))
            Eat();

        return new VariableDeclaration(name, type, value);
    }

    private Statement ParseExpressionStatement()
    {
        var expr = ParseExpression();

        if (Expect(TokenType.Semicolon))
            Eat();

        return new ExpressionStatement(expr);
    }

    private Expression ParseExpression()
    {
        return ParseLogicalExpression();
    }

    private Expression ParseLogicalExpression()
    {
        var left = ParseRelationalExpression();

        while (NotEOF && At()._value is "&&" or "or")
        {
            var op = Eat()._value == "&&" ? BinaryOperator.And : BinaryOperator.Or;
            var right = ParseRelationalExpression();
            left = new BinaryExpression(op, left, right);
        }

        return left;
    }

    private Expression ParseRelationalExpression()
    {
        var left = ParseAdditiveExpression();

        while (NotEOF && At()._value is "==" or "!=" or ">" or ">=" or "<" or "<=")
        {
            var ops = new Dictionary<string, BinaryOperator>
            {
                { "==", BinaryOperator.Equal },
                { "!=", BinaryOperator.NotEqual },
                { ">", BinaryOperator.More },
                { ">=", BinaryOperator.MoreOrEqual },
                { "<", BinaryOperator.Less },
                { "<=", BinaryOperator.LessOrEqual },
            };
            var op = ops[Eat()._value];

            var right = ParseAdditiveExpression();
            left = new BinaryExpression(op, left, right);
        }

        return left;
    }

    private Expression ParseAdditiveExpression()
    {
        var left = ParseMultiplicativeExpression();

        while (NotEOF && At()._value is "+" or "-")
        {
            var op = Eat()._value == "+" ? BinaryOperator.Add : BinaryOperator.Subtract;
            var right = ParseMultiplicativeExpression();
            left = new BinaryExpression(op, left, right);
        }

        return left;
    }

    private Expression ParseMultiplicativeExpression()
    {
        var left = ParseUnaryExpression();

        while (NotEOF && At()._value is "*" or "/" or "%")
        {
            var ops = new Dictionary<string, BinaryOperator>
            {
                { "*", BinaryOperator.Multiply },
                { "/", BinaryOperator.Divide },
                { "%", BinaryOperator.Modulo },
            };
            var op = ops[Eat()._value];

            var right = ParseUnaryExpression();
            left = new BinaryExpression(op, left, right);
        }
        return left;
    }

    private Expression ParseUnaryExpression()
    {
        if (At()._value is "+" or "-" or "!")
        {
            var ops = new Dictionary<string, UnaryOperator>
            {
                { "+", UnaryOperator.Plus },
                { "-", UnaryOperator.Minus },
                { "!", UnaryOperator.Not },
            };
            var op = ops[Eat()._value];

            var expr = ParseAccessAndInvocationExpression();
            return new UnaryExpression(op, expr);
        }
        return ParseAccessAndInvocationExpression();
    }

    private Expression ParseAccessAndInvocationExpression()
    {
        var expr = ParsePrimaryExpression();

        /*
        a(1, 2).b.c(1, 2, 3)
        */


        while (NotEOF && At()._type is TokenType.OpenParen or TokenType.Dot)
        {
            if (At(TokenType.OpenParen))
            {
                Eat();

                List<Expression> args = new();
                do
                {
                    if (At(TokenType.CloseParen))
                    {
                        break;
                    }


                    if (args.Count != 0)
                    {
                        if (Expect(TokenType.Comma))
                            Eat();
                    }
                    args.Add(ParseExpression());
                } while (NotEOF && At(TokenType.Comma));

                if (Expect(TokenType.CloseParen))
                    Eat();

                expr = new InvocationExpression(expr, args.ToArray());
            }
            if (At(TokenType.Dot))
            {
                Eat();

                Expect(TokenType.Identifier);

                expr = new MemberAccessExpression(expr, Eat()._value);
            }
        }

        return expr;
    }

    private Expression ParsePrimaryExpression()
    {
        if (At(TokenType.Identifier))
            return new Identidier(Eat()._value);
        else if (At(TokenType.Int32))
            return new Int32Literal(int.Parse(Eat()._value));
        else if (At(TokenType.Float32))
            return new Float32Literal(float.Parse(Eat()._value));
        else if (At(TokenType.Boolean))
            return new BooleanLiteral(Eat()._value == "true");
        else if (At(TokenType.String))
            return new StringLiteral(Eat()._value);
        else if (At(TokenType.OpenParen))
        {
            Eat();

            var value = ParseExpression();
            if (Expect(TokenType.CloseParen))
                Eat();
            return value;
        }

        Eat();
        return new InvalidExpression();
    }

    private Expression ParseType()
    {
        Expect(TokenType.Identifier);
        Expression expr = new Identidier(Eat()._value);

        while (At(TokenType.Dot))
        {
            Eat();

            Expect(TokenType.Identifier);
            expr = new MemberAccessExpression(expr, Eat()._value);
        }

        return expr;
    }

    private Function ParseFunction()
    {
        Expect(TokenType.Fn);
        Eat();

        /*
        fn function(arg: string, arg2: type): int32 {
            return 1
        }
        */

        Expect(TokenType.Identifier);
        var fnName = Eat()._value;

        var @params = new List<Parameter>();
        Expect(TokenType.OpenParen);
        Eat();
        do
        {
            if (At(TokenType.CloseParen))
            {
                break;
            }

            if (@params.Count > 0)
            {
                Expect(TokenType.Comma);
                Eat();
            }

            Expect(TokenType.Identifier);
            var name = Eat()._value;
            Expect(TokenType.Colon);
            Eat();
            var type = ParseType();
            @params.Add(new(name, type));

        } while (NotEOF && At(TokenType.Comma));
        Expect(TokenType.CloseParen);
        Eat();

        Expression returnType = new Identidier("void");
        if (At(TokenType.Colon))
        {
            Eat();
            returnType = ParseType();
        }

        var body = new List<Statement>();
        Expect(TokenType.OpenBrace);
        Eat();
        while (NotEOF && !At(TokenType.CloseBrace))
        {
            body.Add(ParseStatement());
        }
        Expect(TokenType.CloseBrace);
        Eat();

        return new Function(fnName, @params.ToArray(), body.ToArray(), returnType);
    }
}