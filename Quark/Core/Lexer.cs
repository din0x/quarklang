using QuarkLang.Core.Utils;
using System.Collections.Generic;
using System;
using System.Linq;

namespace QuarkLang.Core;

public static class Lexer
{
    public static Token[] Tokenize(string? sourceCode)
    {
        sourceCode += " ";
        List<Token> tokens = new();
        string str = "";

        bool lookForOperator = false;
        bool insideString = false;
        bool comment = false;

        uint current = 0;

        foreach (char c in sourceCode)
        {
            current++;

            if (c is '\n')
            {
                comment = false;
            }

            else if (c == '#') // comments
                comment = true;
            else if (comment)
                continue;

            if (c == '"')
            {
                if (insideString)
                {
                    tokens.Add(new(TokenType.String, str, current));
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
                            tokens.Add(new(TokenType.Int32, str.Replace('.', ','), current - (uint)str.Length));

                        else if (float.TryParse(str.Replace("_", "").Replace('.', ','), out float f))
                            tokens.Add(new(TokenType.Float32, str.Replace('.', ','), current - (uint)str.Length));

                        else if (str == "true" || str == "false")
                            tokens.Add(new(TokenType.Boolean, str, current - (uint)str.Length));

                        else if (str == "var")
                            tokens.Add(new(TokenType.Var, "var", current - (uint)str.Length));
                        else if (str == "const")
                            tokens.Add(new(TokenType.Const, "const", current - (uint)str.Length));



                        else if (str == "if")
                            tokens.Add(new(TokenType.If, "if", current - (uint)str.Length));
                        else if (str == "elif")
                            tokens.Add(new(TokenType.Elif, "elif", current - (uint)str.Length));
                        else if (str == "else")
                            tokens.Add(new(TokenType.Else, "else", current - (uint)str.Length));

                        else if (str == "while")
                            tokens.Add(new(TokenType.While, "while", current - (uint)str.Length));
                        else if (str == "break")
                            tokens.Add(new(TokenType.Break, "break", current - (uint)str.Length));

                        else if (str == "class")
                            tokens.Add(new(TokenType.Class, "class", current - (uint)str.Length));

                        else if (str == "fn")
                            tokens.Add(new(TokenType.Fn, "fn", current - (uint)str.Length));
                        else if (str == "return")
                            tokens.Add(new(TokenType.Return, "return", current - (uint)str.Length));

                        else if (str == "import")
                            tokens.Add(new(TokenType.Import, "import", current - (uint)str.Length));

                        else if (str.Trim() != "")
                            tokens.Add(new(TokenType.Identifier, str.Trim(), current - (uint)str.Length));

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
                            && tokens.Last()._type is TokenType.Operator or TokenType.EqualSign
                            && tokens.Last()._value is not "&" or "|")
                        {

                            if (tokens.Last()._type == TokenType.EqualSign)
                                tokens.Add(new(TokenType.Operator, "==", current - (uint)str.Length));
                            else 
                                tokens.Add(new(TokenType.Operator, tokens.Last()._value + "=", current - (uint)str.Length));

                            tokens.RemoveAt(tokens.Count - 2);
                            lookForOperator = false;
                        }
                        else
                        {
                            tokens.Add(new(TokenType.EqualSign, "=", current - (uint)str.Length));
                            lookForOperator = true;
                        }
                    }
                    else if (c is '+' or '-' or '*' or '/' or '%' or '>' or '<' or '!' or '|' or '&')
                    {
                        if (c is '&' or '|' && lookForOperator && tokens.Last()._value == c.ToString())
                        {
                            tokens[^1]._value += c.ToString();
                            lookForOperator = false;
                        }
                        else
                        {
                            tokens.Add(new(TokenType.Operator, c.ToString(), current - (uint)str.Length));
                            lookForOperator = true;
                        }
                    }
                    else if (c == ',') 
                    { 
                        tokens.Add(new(TokenType.Comma, ",", current - 1)); lookForOperator = false; 
                    }
                    else if (c == '.') 
                    { 
                        tokens.Add(new(TokenType.Dot, ".", current - 1)); lookForOperator = false; 
                    }
                    else if (c == ':') 
                    { 
                        tokens.Add(new(TokenType.Colon, ":", current - 1)); lookForOperator = false; 
                    }
                    else if (c == ';') 
                    { 
                        tokens.Add(new(TokenType.Semicolon, ";", current - 1)); lookForOperator = false; 
                    }
                    else if (c == '(') 
                    { 
                        tokens.Add(new(TokenType.OpenParen, "(", current - 1)); lookForOperator = false; 
                    }
                    else if (c == ')') 
                    { 
                        tokens.Add(new(TokenType.CloseParen, ")", current - 1)); lookForOperator = false; 
                    }
                    else if (c == '{') 
                    { 
                        tokens.Add(new(TokenType.OpenBrace, "{", current - 1)); lookForOperator = false; 
                    }
                    else if (c == '}') 
                    { 
                        tokens.Add(new(TokenType.CloseBrace, "}", current - 1)); lookForOperator = false; 
                    }
                }
                else str += c;
            }
        }
        tokens.Add(new(TokenType.EOF, "eof", current));
        return tokens.ToArray();
    }
}


/*
public static class Lexer
{
    public static string _characters;

    private static bool IsAlphanumeric(char c)
    {
        var chars = "_ABCDEFGHIJKLMNOPRSTUWXYZabcdefghijklmnoprstuwxyz0123456789";
        return chars.Contains(c);
    }

    private static bool IsNumeric(char c)
    {
        var chars = "0123456789";
        return chars.Contains(c);
    }

    private static TokenType TypeFromString(string s)
    {
        var keywords = new Dictionary<string, TokenType>
        {
            { "import", TokenType.Import },
            { "var", TokenType.Var },
            { "const", TokenType.Const },
            { "fn", TokenType.Import },
            { "return", TokenType.Fn },
            { "class", TokenType.Class },
            { "if", TokenType.If },
            { "elif", TokenType.Elif },
            { "else", TokenType.Else },
            { "while", TokenType.While },
            { "break", TokenType.Break },
            { "true", TokenType.Boolean },
            { "false", TokenType.Boolean },
        };

        if (keywords.TryGetValue(s, out TokenType keyword))
            return keyword;

        else if (int.TryParse(s.Replace("_", ""), out int _))
            return TokenType.Int32;

        else if (float.TryParse(s.Replace("_", "").Replace(".", ","), out float _))
            return TokenType.Float32;

        return TokenType.NAT;
    }

    private static char At()
    {
        return _characters[0];
    }

    private static char Eat()
    {
        var at = At();

        _ = _characters.Remove(0);

        return at;
    }

    public static Token[] Tokenize(string characters)
    {
        _characters = characters;
        var tokens = new List<Token>();

        while (_characters.Length > 0)
        {
            tokens.Add(MultiCharacter());
        }

        return tokens.ToArray();
    }

    private static Token MultiCharacter()
    {
        string str = "";

        if ("_ABCDEFGHIJKLMNOPRSTUWXYZabcdefghijklmnoprstuwxyz".Contains(At()))
        {
            str += Eat();
            while (IsAlphanumeric(At()))
            {
                str += Eat();
            }
            return new Token()
        }

        return Parenthesis();
    }

    private static Token String()
    {

    }

    private static Token Parenthesis() 
    { 
        
    }
}
*/