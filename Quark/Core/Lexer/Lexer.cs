using System;
using System.Collections.Generic;
using System.Linq;

namespace QuarkLang.Core.Lexer;

public class Lexer
{
    public Token[] Tokens { get; }

    public Lexer(string code, string path)
    {
        Tokens = Tokenize(code, path);
    }

    public static Token[] Tokenize(string? sourceCode, string path)
    {
        if (sourceCode == null) throw new ArgumentNullException(nameof(sourceCode));
        sourceCode += " ";
        Token.path = path;

        List<Token> tokens = new();
        string str = "";

        bool lookForOperator = false;
        bool insideString = false;
        bool comment = false;

        short line = 1;
        int current = 1;

        foreach (char c in sourceCode)
        {
            if (c is '\n')
            {
                line++; current = 0;
                comment = false;
            }

            else if (c == '#') // comments
                comment = true;
            else if (comment)
                continue;

            if (!comment)
                current++;

            if (c == '"')
            {
                if (insideString)
                {
                    tokens.Add(new(TokenType.String, line, current, str));
                    str = "";
                }
                insideString = !insideString;
            }
            else if (insideString) 
                str += c;
            else
            {
                if (c is ' ' or '\n' or '\t' or '\r' or
                    '=' or '!' or '<' or '>' or
                    '+' or '-' or '*' or '/' or '%' or
                    '(' or ')' or '{' or '}' or '&' or
                    '|' or ',' or ':' or ';' or '#'
                    || c == '.' && !int.TryParse(str.Replace("_", ""), out int _))
                {
                    // multi character tokens eg. 1000, my_value
                    if (str != "")
                    {
                        if (int.TryParse(str.Replace("_", ""), out int i))
                            tokens.Add(new(TokenType.Int32, line, current - str.Trim().Length - 1, str.Replace('.', ',')));

                        else if (float.TryParse(str.Replace("_", "").Replace('.', ','), out float f))
                            tokens.Add(new(TokenType.Float32, line, current - str.Trim().Length - 1, str.Replace('.', ',')));

                        else if (str == "true" || str == "false")
                            tokens.Add(new(TokenType.Boolean, line, current - str.Trim().Length - 1, str));

                        else if (str == "null")
                            tokens.Add(new(TokenType.Null, line, current - 5));

                        else if (str == "var")
                            tokens.Add(new(TokenType.Var, line, current - 4));
                        else if (str == "const")
                            tokens.Add(new(TokenType.Const, line, current - 6));



                        else if (str == "if")
                            tokens.Add(new(TokenType.If, line, current - 3));
                        else if (str == "elif")
                            tokens.Add(new(TokenType.Elif, line, current - 5));
                        else if (str == "else")
                            tokens.Add(new(TokenType.Else, line, current - 5));

                        else if (str == "while")
                            tokens.Add(new(TokenType.While, line, current - 6));
                        else if (str == "break")
                            tokens.Add(new(TokenType.Break, line, current - 6));

                        else if (str == "fun")
                            tokens.Add(new(TokenType.Fun, line, current - 4));
                        else if (str == "return")
                            tokens.Add(new(TokenType.Return, line, current - 6));

                        else if (str == "import")
                            tokens.Add(new(TokenType.Import, line, current - 6));

                        else if (str.Trim() != "")
                            tokens.Add(new(TokenType.Identifier, line, current - str.Trim().Length - 1, str.Trim()));

                        str = "";
                        lookForOperator = false;
                    }

                    // checks for single char tokens, eg. +, -
                    // also checks for tokens like >=, ==
                    if (c == ' ') lookForOperator = false;
                    else if (c == '=')
                    {
                        if (lookForOperator
                            && tokens.Count > 0
                            && tokens.Last().type is TokenType.BinaryOperator or TokenType.EqualSign
                            && tokens.Last().value is not "&" or "|")
                        {

                            if (tokens.Last().type == TokenType.EqualSign)
                                tokens.Add(new(TokenType.BinaryOperator, line, current - 2, "=="));
                            else tokens.Add(new(TokenType.BinaryOperator, line, current - 2, tokens.Last().value + "="));

                            tokens.RemoveAt(tokens.Count - 2);
                            lookForOperator = false;
                        }
                        else
                        {
                            tokens.Add(new(TokenType.EqualSign, line, current - 1));
                            lookForOperator = true;
                        }
                    }
                    else if (c is '+' or '-' or '*' or '/' or '%' or '>' or '<' or '!' or '|' or '&')
                    {
                        if (c is '&' or '|' && lookForOperator && tokens.Last().value == c.ToString())
                        {
                            tokens[^1].value += c.ToString();
                            lookForOperator = false;
                        }
                        else
                        {
                            tokens.Add(new(TokenType.BinaryOperator, line, current - 1, c.ToString()));
                            lookForOperator = true;
                        }
                    }
                    else if (c == ',') { tokens.Add(new(TokenType.Comma, line, current - 1, ",")); lookForOperator = false; }
                    else if (c == '.') { tokens.Add(new(TokenType.Dot, line, current - 1, ".")); lookForOperator = false; }
                    else if (c == ':') { tokens.Add(new(TokenType.Colon, line, current - 1, ":")); lookForOperator = false; }
                    else if (c == ';') { tokens.Add(new(TokenType.Semicolon, line, current - 1, ";")); lookForOperator = false; }
                    else if (c == '(') { tokens.Add(new(TokenType.OpenParen, line, current - 1, "(")); lookForOperator = false; }
                    else if (c == ')') { tokens.Add(new(TokenType.CloseParen, line, current - 1, ")")); lookForOperator = false; }
                    else if (c == '{') { tokens.Add(new(TokenType.OpenBrace, line, current - 1, "{")); lookForOperator = false; }
                    else if (c == '}') { tokens.Add(new(TokenType.CloseBrace, line, current - 1, "}")); lookForOperator = false; }
                }
                else str += c;
            }
        }
        tokens.Add(new(TokenType.EOF, line, current));
        return tokens.ToArray();
    }
}

public enum TokenType
{
    Null,
    Int32,
    Float32,
    Boolean,
    String,
    Identifier,

    BinaryOperator,

    EqualSign,

    Dot,
    Comma,
    Colon,
    Semicolon,

    Import,

    Var,
    Const,

    If,
    Elif,
    Else,

    While,
    Break,

    Fun,
    Return,

    OpenParen,
    CloseParen,

    OpenBrace,
    CloseBrace,

    EOF,
}

public class Token
{
    public static string path = "";

    public TokenType type;
    public string value = "";
    public short line;
    public int start;
    public short length;
    public string file;


    public Token(TokenType type, short line, int start, string value = "", short length = 1)
    {
        this.type = type;
        this.value = value;
        this.line = line;
        this.start = start;
        this.length = length;
        file = path;
    }

    public override string ToString()
    {
        if (value == "") return $"{type,-15}";
        return $"{type,-15} '{value}'";
    }
}