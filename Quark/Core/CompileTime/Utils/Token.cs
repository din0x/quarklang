using System;

namespace QuarkLang.Core.CompileTime.Utils;

public class Token
{
    public TokenType _type;
    public string _value;
    public uint _character;
    public ushort _length;

    public Token(TokenType type, string value, uint character)
    {
        _type = type;
        _value = value;
        _character = character;
        _length = (ushort)value.Length;
    }

    public override string ToString()
    {
        return $"{_type,-20} {_value}";
    }
}