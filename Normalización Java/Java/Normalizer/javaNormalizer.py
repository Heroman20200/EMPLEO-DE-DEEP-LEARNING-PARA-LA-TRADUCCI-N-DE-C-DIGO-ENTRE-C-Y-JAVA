from antlr4 import *
from Gramaticas.Java.JavaLexer import JavaLexer
from Gramaticas.Java.JavaParser import JavaParser
from Gramaticas.Java.JavaParserVisitor import JavaParserVisitor
from antlr4.tree.Tree import TerminalNode

class javaNormalizer(JavaParserVisitor):
    def __init__(self):
        self.normalized_code = []
        self.identifiers = {}

    def defaultResult(self):
        return ''

    def aggregateResult(self, aggregate, nextResult):
        if nextResult:
            return f'{aggregate} {nextResult}'.strip()
        return aggregate

    def visitVariableDeclaratorId(self, ctx):
            variableName = ctx.getText()
            variableDeclarationContext = ctx.parentCtx
            
            if hasattr(variableDeclarationContext, 'typeType'):
                variableTypeContext = variableDeclarationContext.typeType()
                if variableTypeContext is not None:
                    variableTypeText = variableTypeContext.getText()
                    variableName = 'VAR_' + variableTypeText.upper()
                    self.identifiers[ctx.getText()] = variableName

            return variableName
        
    def visitLocalVariableDeclaration(self, ctx):
        variableTypeText = ctx.typeType().getText()

        declarations = []
        for varDecl in ctx.variableDeclarators().variableDeclarator():
            varName = varDecl.variableDeclaratorId().identifier().getText()
            normalizedVarName = 'VAR_' + variableTypeText.upper()
            self.identifiers[varName] = normalizedVarName

            declaration = f"{variableTypeText} {normalizedVarName}"
            if varDecl.variableInitializer():
                initializer = self.visit(varDecl.variableInitializer())
                declaration += f" = {initializer}"
            declarations.append(declaration)

        normalizedDeclarations = ' '.join(declarations)
        return normalizedDeclarations

    def visitLiteral(self, ctx:JavaParser.LiteralContext):
        if ctx.integerLiteral() is not None:
            return 'LITERAL_INT'
        elif ctx.floatLiteral() is not None:
            return 'LITERAL_FLOAT'
        elif ctx.CHAR_LITERAL() is not None:
            return 'LITERAL_CHAR'
        elif ctx.STRING_LITERAL() is not None:
            return 'LITERAL_STRING'
        else:
            return super().visitLiteral(ctx)
            
    def visitConstructorDeclaration(self, ctx):
        parts = ['CONSTRUCTOR_NAME']

        if ctx.formalParameters():
            parts.append(ctx.formalParameters().accept(self))

        if ctx.qualifiedNameList():
            parts.append('throws')
            parts.append(ctx.qualifiedNameList().accept(self))

        if ctx.constructorBody:
            parts.append(ctx.constructorBody.accept(self))

        return ' '.join(parts)

    def visitMethodDeclaration(self, ctx:JavaParser.MethodDeclarationContext):
        parts = []

        if ctx.typeParameters():
            parts.append(ctx.typeParameters().accept(self))

        if ctx.typeTypeOrVoid():
            parts.append(ctx.typeTypeOrVoid().accept(self))

        for child in ctx.children:
            if isinstance(child, JavaParser.IdentifierContext):
                methodName = child.getText()
                self.identifiers[methodName] = 'METHOD_NAME'
                parts.append('METHOD_NAME')
                break

        if ctx.formalParameters():
            parts.append(ctx.formalParameters().accept(self))

        if ctx.qualifiedNameList():
            parts.append('throws')
            parts.append(ctx.qualifiedNameList().accept(self))

        if ctx.methodBody():
            parts.append(ctx.methodBody().accept(self))
        else:
            parts.append(';')

        return ' '.join(parts)
        
    def visitTerminal(self, node):
        text = node.getText()
        if text in self.identifiers:
            return self.identifiers[text]
        elif text == '<EOF>':
            return ''
        else:
            return text
                   
def normalize_java_code(code):
    lexer = JavaLexer(InputStream(code))
    stream = CommonTokenStream(lexer)
    parser = JavaParser(stream)
    tree = parser.compilationUnit()

    visitor = javaNormalizer()
    normalized_code = visitor.visit(tree)

    return normalized_code

with open('train.java', 'r', encoding='utf-8') as f:
    with open('train_normalizado.java', 'w', encoding='utf-8') as f_out:
        for line in f:
            f_out.write(f'{normalize_java_code(line)}\n')
                
with open('valid.java', 'r', encoding='utf-8') as f:
        with open('valid_normalizado.java', 'w', encoding='utf-8') as f_out:
            for line in f:
                f_out.write(f'{normalize_java_code(line)}\n')
