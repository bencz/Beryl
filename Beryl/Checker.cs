/**
 * Issues:
 *
 *     1. How are procedures implemented?  The "func" keyword could be reused by making the return type optional.
 *     2. How are functions invoked?  C-style (ignore return value) or more strictly?
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beryl.AST;

namespace Beryl
{
    public class Checker: Visitor
    {
        private SymbolTable _symbols;
        private AST.Program _program;
        private uint        _counter;

        public Checker(SymbolTable symbols, AST.Program program)
        {
            _symbols = symbols;
            _program = program;

            // create the standard set of predefined identifiers that Triangle defines
            CreateStandardEnvironment();

            // start walking the AST from the topmost node
            _program.visit(this);
        }

        /** Creates a synthetic block name for those blocks that are anonymous. */
        private string CreateName()
        {
            string result = _counter.ToString("D4");
            _counter += 1;
            return result;
        }

        private Declaration[] CreateStandardEnvironment(bool insert = false)
        {
            List<Declaration> result = new List<Declaration>();

            // signal that the following symbols are part of the standard library
            Position position = new Position("(library)", 0, 0);
            Declaration declaration;
            Expression expression;

            // enter the predefined constant 'maxint' into the symbol table
            expression = new IntegerExpression(position, int.MaxValue);
            declaration = new ConstantDeclaration(position, "maxint", new IntegerType(position), expression);
            result.Add(declaration);

            // enter the predefined constants 'false' and 'true' into the symbol table
            expression = new BooleanExpression(position, false);
            declaration = new ConstantDeclaration(position, "false", new BooleanType(position), expression);
            result.Add(declaration);

            expression = new BooleanExpression(position, true);
            declaration = new ConstantDeclaration(position, "true", new BooleanType(position), expression);
            result.Add(declaration);

            // enter the predefined operators into the symbol table
            // ... the \ operator
            declaration = new FunctionDeclaration(
                position,
                "\\",
                new BooleanType(position),
                new ParameterDeclaration[] { new ParameterDeclaration(position, "value", new BooleanType(position)) },
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            result.Add(declaration);

            // ... all Triangle operators of the form Boolean x Boolean -> Boolean
            string[] boolean_and_boolean_to_boolean_operators = { "/\\", "\\/", "=", "\\=" };
            foreach (string @operator in boolean_and_boolean_to_boolean_operators)
            {
                declaration = new FunctionDeclaration(
                    position,
                    @operator,
                    new BooleanType(position),
                    new ParameterDeclaration[]
                    {
                        new ParameterDeclaration(position, "first", new BooleanType(position)),
                        new ParameterDeclaration(position, "other", new BooleanType(position))
                    },
                    null    // note: the code generator must handle these predefined functions so no body is defined
                );
                result.Add(declaration);
            }

            // ... all Triangle operators of the form Integer x Integer -> Integer
            string[] integer_and_integer_to_integer_operators = { "+", "-", "*", "/", "//" };
            foreach (string @operator in integer_and_integer_to_integer_operators)
            {
                declaration = new FunctionDeclaration(
                    position,
                    @operator,
                    new IntegerType(position),
                    new ParameterDeclaration[]
                    {
                        new ParameterDeclaration(position, "first", new IntegerType(position)),
                        new ParameterDeclaration(position, "other", new IntegerType(position))
                    },
                    null    // note: the code generator must handle these predefined functions so no body is defined
                );
                result.Add(declaration);
            }

            // ... all Triangle operators of the form Integer x Integer -> Boolean
            string[] integer_and_integer_to_boolean_operators = { "<", "<=", ">", ">=", "=", "\\=" };
            foreach (string @operator in integer_and_integer_to_boolean_operators)
            {
                declaration = new FunctionDeclaration(
                    position,
                    @operator,
                    new BooleanType(position),
                    new ParameterDeclaration[]
                    {
                        new ParameterDeclaration(position, "first", new IntegerType(position)),
                        new ParameterDeclaration(position, "other", new IntegerType(position))
                    },
                    null    // note: the code generator must handle these predefined functions so no body is defined
                );
                result.Add(declaration);
            }

            // enter the predefined functions (getint and putint) into the symbol table
            declaration = new FunctionDeclaration(
                position,
                "getint",
                new IntegerType(position),
#if false
                new ParameterDeclaration[] { new ParameterDeclaration(position, "value", new IntegerType(position))},
#else
                new ParameterDeclaration[0],
#endif
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            result.Add(declaration);

            declaration = new FunctionDeclaration(
                position,
                "putint",
                new IntegerType(position),
                new ParameterDeclaration[] { new ParameterDeclaration(position, "value", new IntegerType(position))},
                null        // note: the code generator must handle these predefined functions so no body is defined
            );
            result.Add(declaration);

            return result.ToArray();
        }

        public void visit(AssignCommand that)
        {
            // determine the type of the right-hand-side (RHS) by visiting it
            that.Expression.visit(this);

            // check that the symbol exists - by trying to look it up
            Declaration declaration = _symbols.Lookup(that.Name);
            if (declaration == null)
                throw new CheckerError(that.Position, "Variable '" + that.Name + "' not declared in assignment statement");

            // check that the symbol is indeed a variable
            switch (declaration.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot assign to a constant");

                case SymbolKind.Function:
                    throw new CheckerError(that.Position, "Cannot assign to a function");

                case SymbolKind.Parameter:
                    throw new CheckerError(that.Position, "Cannot call parameter");

                case SymbolKind.Variable:
                    break;

                default:
                    throw new CheckerError(that.Position, "Unknown symbol kind: " + declaration.Kind.ToString());
            }

            // check that the types of the left-hand-side is equal to the type of the right-hand-side
            TypeKind firstType = declaration.Type.Kind;
            TypeKind otherType = that.Expression.Type.Kind;
            if (firstType != otherType)
                throw new CheckerError(that.Position, "Type mismatch in assignment");
        }

        public void visit(BeginCommand that)
        {
            that.Commands.visit(this);
        }

        public void visit(BooleanExpression that)
        {
            that.Type = new BooleanType(that.Position);
        }

        public void visit(BooleanType that)
        {
        }

        public void visit(CallCommand that)
        {
            // let the arguments resolve their types
            foreach (Expression argument in that.Arguments)
                argument.visit(this);

            // mangle the parameter list so as to be able to look up the mangled symbol
            System.Text.StringBuilder mangled = new System.Text.StringBuilder(64);
            that.Encode(mangled);
            string name = mangled.ToString();

            // look up the function name
            Declaration declaration = _symbols.Lookup(name);
            if (declaration == null)
                throw new CheckerError(that.Position, "Unknown function name '" + Demangler.Decode(name) + "' in call command");

            switch (declaration.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot call constant");

                case SymbolKind.Function:
                    break;

                case SymbolKind.Parameter:
                    throw new CheckerError(that.Position, "Cannot call parameter");

                case SymbolKind.Variable:
                    throw new CheckerError(that.Position, "Cannot call variable");

                default:
                    throw new CheckerError(declaration.Position, "Unknown symbol kind: " + declaration.Kind.ToString());
            }

            // check that the expected number of parameters is specified
            FunctionDeclaration function = (FunctionDeclaration) declaration;
            if (that.Arguments.Length != function.Parameters.Length)
                throw new CheckerError(that.Position, "Incorrect number of parameters in function call");

            // check that the argument types match the parameter types
            for (int i = 0; i < that.Arguments.Length; i++)
            {
                if (that.Arguments[i].Type.Kind != function.Parameters[i].Type.Kind)
                    throw new CheckerError(that.Arguments[i].Position, "Type mismatch in argument to procedure");
            }
        }

        public void visit(Commands that)
        {
            foreach (Command command in that.CommandArray)
                command.visit(this);
        }

        public void visit(ConstantDeclaration that)
        {
            // resolve the type of the expression
            that.Expression.visit(this);

            // hack: fix up the likely incorrect type that the Parser created for the constant (always "Integer")
            switch (that.Expression.Type.Kind)
            {
                case TypeKind.Boolean:
                    that.Type = new BooleanType(that.Expression.Position);
                    break;

                case TypeKind.Integer:
                    // simply keep the default integer type
                    break;

                case TypeKind.String:
                    that.Type = new StringType(that.Expression.Position);
                    break;

                default:
                    throw new CheckerError(that.Position, "Unknown type encountered: " + that.Expression.Type.Kind.ToString());
            }

            // nothing to check as we've just computed the type of the constant (cannot mismatch)

            if (!_symbols.Insert(that.Name, that))
                throw new CheckerError(that.Position, "Duplicate name '" + that.Name + "' detected in constant declaration");
        }

        public void visit(Declarations that)
        {
            foreach (Declaration declaration in that.DeclarationsArray)
                declaration.visit(this);
        }

        public void visit(FunctionDeclaration that)
        {
            System.Text.StringBuilder mangled = new System.Text.StringBuilder(64);
            that.Encode(mangled);
            string name = mangled.ToString();

            if (!_symbols.Insert(name, that))
                throw new CheckerError(that.Position, "Duplicate name '" + Demangler.Decode(name) + "' detected in function declaration");
            _symbols.EnterScope(name);

            that.Type.visit(this);

            foreach (ParameterDeclaration parameter in that.Parameters)
            {
                // insert each parameter into the current scope so that it becomes visible to the code
                if (!_symbols.Insert(parameter.Name, parameter))
                    throw new CheckerError(parameter.Position, "Duplicate name '" + parameter.Name + "' detected in parameter");

                parameter.visit(this);
            }

            if (that.Body != null)
                that.Body.visit(this);

            _symbols.LeaveScope(name);
        }

        public void visit(FunctionExpression that)
        {
            // let the arguments resolve their types
            foreach (Expression argument in that.Arguments)
                argument.visit(this);

            // mangle the parameter list so as to be able to look up the mangled symbol
            System.Text.StringBuilder mangled = new System.Text.StringBuilder(64);
            that.Encode(mangled);
            string name = mangled.ToString();

            // look up the function declaration
            Declaration declaration = _symbols.Lookup(name);
            if (declaration == null)
                throw new CheckerError(that.Position, "Unknown function name '" + Demangler.Decode(name) + "' in function call");

            // check that the symbol is actually a function
            switch (declaration.Kind)
            {
                case SymbolKind.Constant:
                    throw new CheckerError(that.Position, "Cannot call constant");

                case SymbolKind.Function:
                    break;

                case SymbolKind.Parameter:
                    throw new CheckerError(that.Position, "Cannot call parameter");

                case SymbolKind.Variable:
                    throw new CheckerError(that.Position, "Cannot call variable");

                default:
                    throw new CheckerError(declaration.Position, "Unknown symbol kind: " + declaration.Kind.ToString());
            }

            // check that the number of arguments match the number of parameters
            FunctionDeclaration function = (FunctionDeclaration) declaration;
            if (that.Arguments.Length != function.Parameters.Length)
                throw new CheckerError(that.Position, "Incorrect number of parameters in function call");

            // check that the argument types match the parameter types
            for (int i = 0; i < that.Arguments.Length; i++)
            {
                if (that.Arguments[i].Type.Kind != function.Parameters[i].Type.Kind)
                    throw new CheckerError(that.Arguments[i].Position, "Type mismatch in argument to function");
            }

            that.Type = declaration.Type;
        }

        public void visit(IfCommand that)
        {
            that.Expression.visit(this);
            if (that.Expression.Type.Kind != TypeKind.Boolean)
                throw new CheckerError(that.Position, "Boolean expression expected in 'if' statement");

            that.If.visit(this);
            that.Else.visit(this);
        }

        public void visit(IntegerExpression that)
        {
            that.Type = new IntegerType(that.Position);
        }

        public void visit(IntegerType that)
        {
        }

        public void visit(LetCommand that)
        {
            string name = CreateName();
            _symbols.EnterScope(name);
            foreach (Declaration declaration in that.Declarations)
                declaration.visit(this);
            that.Command.visit(this);
            _symbols.LeaveScope(name);
        }

        public void visit(ParameterDeclaration that)
        {
        }

        public void visit(ParenthesisExpression that)
        {
            that.Expression.visit(this);
            that.Type = that.Expression.Type;
        }

        public void visit(AST.Program that)
        {
            that.Command.Declarations = CreateStandardEnvironment();
            that.Command.visit(this);
        }

        public void visit(StringExpression that)
        {
            that.Type = new StringType(that.Position);
        }

        public void visit(StringType that)
        {
        }

        public void visit(VariableDeclaration that)
        {
            if (!_symbols.Insert(that.Name, that))
                throw new CheckerError(that.Position, "Duplicate name '" + that.Name + "' detected in variable declaration");
        }

        /* VariableExpression perhaps ought to be called IdentifierExpression as it is used for both constants and variables. */
        public void visit(VariableExpression that)
        {
            Declaration declaration = _symbols.Lookup(that.Name);
            if (declaration == null)
                throw new CheckerError(that.Position, "Undefined constant or variable '" + that.Name + "' in expression");
            that.Type = declaration.Type;
        }

        public void visit(WhileCommand that)
        {
            that.Expression.visit(this);
            that.Command.visit(this);

            if (that.Expression.Type.Kind != TypeKind.Boolean)
                throw new CheckerError(that.Position, "Boolean expression expected in 'while' statement");
        }
    }
}

