using Quark.Core.Interpreter.DataTypes;
using Quark.Core.Lexer;
using Quark.Core.Parser.AST;
using Quark.ErrorHandler;
using System.Collections.Generic;
using System.Linq;

namespace Quark.Core.Parser;

class Parser
{
    public ProgramUnit Program { get; }
    public bool ErrorEncountered { get; private set; }

    private readonly List<Token> tokens;
    private readonly Token eof;

    public Parser(Token[] tokens, string path)
    {
        this.tokens = tokens.ToList();
        eof = tokens.Last();
        Program = new ProgramUnit();

        while (NotEOF())
        {
            Program.body.Add(ParseStmt());
        }
    }

    private bool NotEOF()
    {
        return At().type != TokenType.EOF;
    }

    private Token At()
    {
        if (tokens.Count > 0) return tokens[0];
        return eof;
    }

    private Token Eat()
    {
        if (tokens.Count > 0)
        {
            var prev = tokens[0];
            tokens.RemoveAt(0);
            return prev;
        }
        return eof;
    }

    private bool Expect(TokenType? type)
    {
        Token prev = At();
        if (prev.type != type)
        {
            ErrorLoger.Log(new ParserError(prev.type + " expected " + type, prev.line, prev.start, prev.file));
            ErrorEncountered = true;
        }
        return At().type == type;
    }

    private Statement ParseStmt()
    {
        return ParseImportStmt();
    }

    private Statement ParseImportStmt()
    {
        if (At().type != TokenType.Import) return ParseVariableDeclarationStmt();
        Eat();
        string module = "";

        if (Expect(TokenType.Identifier))
        {
            module = Eat().value;
        }

        if (Expect(TokenType.Semicolon)) Eat();

        return new ImportStatement(module);
    }

    private Statement ParseVariableDeclarationStmt()
    {
        if (At().type is TokenType.Var or TokenType.Const)
        {
            bool constant = Eat().type == TokenType.Const; // eats var/const
            string name = Expect(TokenType.Identifier) ? Eat().value : ""; // eats name
            if (Expect(TokenType.EqualSign)) Eat(); // eats =
            Expression value = ParseExpr();
            if (Expect(TokenType.Semicolon)) Eat();
            return new VariableDeclaration(name, value, constant);
        }
        return ParseFunctionDeclarationStmt();
    }

    private Statement ParseFunctionDeclarationStmt()
    {
        if (At().type == TokenType.Fun)
        {
            Eat(); // fun
            var name = Expect(TokenType.Identifier) ? Eat().value : ""; // name
            Expect(TokenType.OpenParen);
            Eat(); // (

            List<FunctionDeclaration.Argument> args = new();

            while (tokens.Count > 0 && At().type != TokenType.CloseParen)
            {
                var argname = Expect(TokenType.Identifier) ? Eat().value : ""; // arg
                if (Expect(TokenType.Colon)) Eat();
                var argtype = Expect(TokenType.Identifier) ? Eat().value : ""; // type

                args.Add(new(argname, new TypeValue(argtype)));

                if (args.Count > 0 && At().type != TokenType.CloseParen)
                {
                    if (Expect(TokenType.Comma)) Eat(); // ,
                }
            }
            if (Expect(TokenType.CloseParen)) Eat(); // )

            var returnType = new TypeValue(new VoidValue());
            if (At().type == TokenType.Colon)
            {
                Eat();

                returnType = Expect(TokenType.Identifier) ? new TypeValue(Eat().value) : returnType;
            }

            var body = new List<Statement>();

            Expect(TokenType.OpenBrace);
            Eat();
            while (At().type != TokenType.CloseBrace && NotEOF())
            {
                body.Add(ParseStmt());
            }
            Expect(TokenType.CloseBrace);
            Eat();

            return new FunctionDeclaration(name, args.ToArray(), returnType, body.ToArray());
        }

        return ParseWhileLoop();
    }

    private Statement ParseWhileLoop()
    {
        if (At().type != TokenType.While) return ParseIfStmt();
        Eat();

        var condition = ParseExpr();
        Expect(TokenType.OpenBrace);
        Eat();
        var body = new List<Statement>();
        while (At().type != TokenType.CloseBrace && NotEOF())
        {
            body.Add(ParseStmt());
        }
        Expect(TokenType.CloseBrace);
        Eat();

        return new WhileLoop(condition, body.ToArray());
    }

    private Statement ParseIfStmt()
    {
        if (At().type == TokenType.If)
        {
            Eat();

            List<Expression> conditions = new();
            List<List<Statement>> blocks = new();
            conditions.Add(ParseExpr());

            Expect(TokenType.OpenBrace);
            Eat();
            blocks.Add(new());
            while (At().type != TokenType.CloseBrace && NotEOF())
            {
                blocks[0].Add(ParseStmt());
            }
            Expect(TokenType.CloseBrace);
            Eat();

            int i = 1;
            while (At().type == TokenType.Elif)
            {
                blocks.Add(new());
                Eat();

                conditions.Add(ParseExpr());
                Expect(TokenType.OpenBrace);
                Eat();
                while (At().type != TokenType.CloseBrace && NotEOF())
                {
                    blocks[i].Add(ParseStmt());
                }
                Expect(TokenType.CloseBrace);
                Eat();
                i++;
            }

            List<Statement>? falseBlock = new();
            if (At().type == TokenType.Else)
            {
                Eat();
                Expect(TokenType.OpenBrace);
                Eat();
                while (At().type != TokenType.CloseBrace && NotEOF())
                {
                    falseBlock.Add(ParseStmt());
                }
                Expect(TokenType.CloseBrace);
                Eat();
            }

            return new ConditionalStatement(conditions.ToArray(), blocks, falseBlock.ToArray());
        }

        return ParseReturnStmt();
    }

    private Statement ParseReturnStmt()
    {
        if (At().type != TokenType.Return) return ParseVariableAssignmentStmt();
        Eat();

        Expression? expr = null;

        if (At().type != TokenType.Semicolon)
        {
            expr = ParseExpr();
        }

        if (Expect(TokenType.Semicolon)) Eat();

        return new ReturnStatement(expr);
    }

    private Statement ParseVariableAssignmentStmt()
    {
        var variable = ParseMemberExpr();

        if (At().type != TokenType.EqualSign)
        {
            if (Expect(TokenType.Semicolon))
                Eat();

            if (variable is CallExpression call)
            {
                call.useAsStmt = true;
            }
            return variable;
        }
        Eat();
        var value = ParseExpr();
        if (Expect(TokenType.Semicolon))
            Eat();

        return new VariableAssignment(variable, value);
    }

    private Expression ParseExpr()
    {
        return ParseLogicalExpr();
    }

    private Expression ParseLogicalExpr()
    {
        Expression left = ParseRelationalExpr();

        while (At().value is "&&" or "||")
        {
            var op = Eat().value;
            Expression right = ParseRelationalExpr();
            left = new LogicalExpression(left, right, op == "&&" ? LogicalExpression.Type.And : LogicalExpression.Type.Or);
        }
        return left;
    }

    private Expression ParseRelationalExpr()
    {
        Expression left = ParseAdditiveExpr();

        while (At().value is "==" or "!=" or ">" or ">=" or "<" or "<=")
        {
            var op = Eat().value;
            Expression right = ParseAdditiveExpr();
            left = new RelationalExpression(left, right, op);
        }
        return left;
    }

    private Expression ParseAdditiveExpr()
    {
        Expression left = ParseMultiplicativeExpr();

        while (At().value == "+" || At().value == "-")
        {
            string op = Eat().value;
            Expression right = ParseMultiplicativeExpr();
            left = new ArithmeticExpression(left, right, op);
        }
        return left;
    }

    private Expression ParseMultiplicativeExpr()
    {
        Expression left = ParseUnaryExpr();

        while (At().value == "*" || At().value == "/" || At().value == "%")
        {
            string op = Eat().value;
            Expression right = ParseUnaryExpr();
            left = new ArithmeticExpression(left, right, op);
        }
        return left;
    }

    private Expression ParseUnaryExpr()
    {
        if (At().value is "+" or "-" or "!")
        {
            var op = Eat().value;
            Expression e = ParseMemberExpr();
            return new UnaryExpression(e, op);
        }

        return ParseMemberExpr();
    }

    private Expression ParseMemberExpr()
    {
        var expr = ParseCallExpr();

        while (At().type == TokenType.Dot)
        {
            Eat();
            Expect(TokenType.Identifier);
            expr = new MemberExpression(expr, Eat().value);

            if (At().type == TokenType.OpenParen)
            {
                Eat();
                List<Expression> args = new();
                while (At().type != TokenType.CloseParen)
                {
                    args.Add(ParseExpr());

                    if (At().type != TokenType.CloseParen)
                    {
                        if (Expect(TokenType.Comma))
                            Eat();
                    }
                }
                Eat();
                expr = new CallExpression(expr, args.ToArray(), false);
            }
        }

        return expr;
    }

    private Expression ParseCallExpr()
    {
        var expr = ParsePrimaryExpr();

        if (At().type == TokenType.OpenParen)
        {
            Eat();
            List<Expression> args = new();
            while (At().type != TokenType.CloseParen && NotEOF())
            {
                args.Add(ParseExpr());

                if (At().type != TokenType.CloseParen)
                {
                    if (Expect(TokenType.Comma))
                        Eat();
                }
            }
            Eat();
            return new CallExpression(expr, args.ToArray(), false);
        }

        return expr;
    }

    private Expression ParsePrimaryExpr()
    {
        var tk = At().type;

        switch (tk)
        {
            case TokenType.Identifier:
                return new Identifier(Eat().value);

            case TokenType.Null:
                Eat();
                return new NullLiteral();

            case TokenType.Int32:
                return new Int32Literal(int.Parse(Eat().value.Replace("_", "")));

            case TokenType.Float32:
                return new Float32Literal(float.Parse(Eat().value.Replace("_", "")));

            case TokenType.Boolean:
                return new BooleanLiteral(bool.Parse(Eat().value));

            case TokenType.String:
                return new StringLiteral(Eat().value);

            case TokenType.OpenParen:
                Eat(); // eat the opening paren
                var value = ParseExpr();
                Expect(TokenType.CloseParen);
                Eat();
                return value;
        }
        Expect(null);
        Eat();
        return new Expression();
    }
}