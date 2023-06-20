namespace QuarkLang.Core.Utils;

public enum TokenType
{
    Int32,
    Float32,
    Boolean,
    String,
    Identifier,

    Operator,

    EqualSign,

    Dot,
    Comma,
    Colon,
    Semicolon,

    Import,

    Class,

    Var,
    Const,

    If,
    Elif,
    Else,

    While,
    Break,

    Fn,
    Return,

    OpenParen,
    CloseParen,

    OpenBrace,
    CloseBrace,

    EOF,
    NAT,
}