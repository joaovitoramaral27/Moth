
using System;
using System.Collections.Generic;

namespace NeX.Lexer;

public class Lexer
{
    private readonly string source;
    private int position;
    private char Current =>
        position >= source.Length ? '\0' : source[position];
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        ["print"] = TokenType.Print,
        ["get"] = TokenType.Get,
        ["num"] = TokenType.Num,
        ["dec"] = TokenType.Dec,
        ["char"] = TokenType.Char,
        ["str"] = TokenType.Str,
        ["tf"] = TokenType.Tf,
        ["var"] = TokenType.Var,
        ["const"] = TokenType.Const,
        ["true"] = TokenType.True,
        ["false"] = TokenType.False
    };
    public Lexer(string source)
    {
        this.source = source;
        position = 0;
    }
    private char Advance()
    {
        char current = Current;
        position++;
        return current;
    }
    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(Current))
        {
            Advance();
        }
    }
    private Token ReadIdentifier()
    {
        int start = position;
        while (char.IsLetterOrDigit(Current) || Current == '_')
        {
            Advance();
        }
        string value = source[start..position];
        if (Keywords.TryGetValue(value, out TokenType type))
        {
            return new Token(type, value, start);
        }
        return new Token(TokenType.Identifier, value, start);
    }
    private Token ReadNumber()
    {
        int start = position;
        while (char.IsDigit(Current))
        {
            Advance();
        }
        if (Current == '.' && position + 1 < source.Length && char.IsDigit(source[position + 1]))
        {
            Advance();
            while (char.IsDigit(Current))
            {
                Advance();
            }
            return new Token(
                TokenType.Decimal,
                source[start..position],
                start
            );
        }
        return new Token(
            TokenType.Number,
            source[start..position],
            start
        );
    }
    private Token ReadString()
    {
        int start = position;
        Advance();
        while (Current != '"' && Current != '\0')
        {
            Advance();
        }
        if (Current == '\0')
        {
            throw new Exception($"String não terminada na posição {start}.");
        }
        Advance();
        return new Token(
            TokenType.String,
            source[start..position],
            start
        );
    }
    private Token ScanToken()
    {
        SkipWhitespace();
        int start = position;
        char current = Advance();
        if (current == '\0')
        {
            return new Token(TokenType.EndOfFile, "", start);
        }
        if (char.IsLetter(current) || current == '_')
        {
            position--;
            return ReadIdentifier();
        }
        if (char.IsDigit(current))
        {
            position--;
            return ReadNumber();
        }
        switch (current)
        {
            case '"':
                position--;
                return ReadString();
            case '+': return new Token(TokenType.Plus, "+", start);
            case '-': return new Token(TokenType.Minus, "-", start);
            case '*': return new Token(TokenType.Multiply, "*", start);
            case '/':
                if (Current == '/')
                {
                    while (Current != '\n' && Current != '\0')
                    {
                        Advance();
                    }
                    return ScanToken();
                }
                return new Token(TokenType.Divide, "/", start);
            case '=':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.Equal, "==", start);
                }
                return new Token(TokenType.Assignment, "=", start);
            case '!':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.NotEqual, "!=", start);
                }
                break;
            case '>':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.GreaterEqual, ">=", start);
                }
                return new Token(TokenType.Greater, ">", start);
            case '<':
                if (Current == '=')
                {
                    Advance();
                    return new Token(TokenType.LessEqual, "<=", start);
                }
                return new Token(TokenType.Less, "<", start);
            case '(': return new Token(TokenType.OpenParenthesis, "(", start);
            case ')': return new Token(TokenType.CloseParenthesis, ")", start);
            case '{': return new Token(TokenType.OpenBrace, "{", start);
            case '}': return new Token(TokenType.CloseBrace, "}", start);
            case ';': return new Token(TokenType.Semicolon, ";", start);
            case ',': return new Token(TokenType.Comma, ",", start);
        }
        throw new Exception(
            $"Caractere inválido '{current}' na posição {start}."
        );
    }
    public List<Token> Tokenize()
    {
        List<Token> tokens = new();
        while (true)
        {
            Token token = ScanToken();
            tokens.Add(token);
            if (token.Type == TokenType.EndOfFile)
            {
                break;
            }
        }
        return tokens;
    }
}