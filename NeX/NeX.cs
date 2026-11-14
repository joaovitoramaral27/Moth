using NeX.Lexer;

string code = """num age = 16; print("Hello World!");""";
Lexer lexer = new Lexer(code);
List<Token> tokens = lexer.Tokenize();
foreach (Token token in tokens)
{
    Console.WriteLine(token);
}