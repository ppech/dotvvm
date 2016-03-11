﻿using System;

namespace DotVVM.Framework.Compilation.Parser.Binding.Parser
{
    public abstract class BindingParserNodeVisitor<T>
    {

        public virtual T Visit(BindingParserNode node)
        {
            if (node is ArrayAccessBindingParserNode)
            {
                return VisitArrayAccess((ArrayAccessBindingParserNode) node);
            }
            else if (node is BinaryOperatorBindingParserNode)
            {
                return VisitBinaryOperator((BinaryOperatorBindingParserNode) node);
            }
            else if (node is ConditionalExpressionBindingParserNode)
            {
                return VisitConditionalExpression((ConditionalExpressionBindingParserNode) node);
            }
            else if (node is FunctionCallBindingParserNode)
            {
                return VisitFunctionCall((FunctionCallBindingParserNode) node);
            }
            else if (node is IdentifierNameBindingParserNode)
            {
                return VisitIdentifierName((IdentifierNameBindingParserNode) node);
            }
            else if (node is LiteralExpressionBindingParserNode)
            {
                return VisitLiteralExpression((LiteralExpressionBindingParserNode) node);
            }
            else if (node is MemberAccessBindingParserNode)
            {
                return VisitMemberAccess((MemberAccessBindingParserNode) node);
            }
            else if (node is ParenthesizedExpressionBindingParserNode)
            {
                return VisitParenthesizedExpression((ParenthesizedExpressionBindingParserNode) node);
            }
            else if (node is UnaryOperatorBindingParserNode)
            {
                return VisitUnaryOperator((UnaryOperatorBindingParserNode) node);
            }
            else
            {
                throw new NotSupportedException($"The node of type {node.GetType()} is not supported!");
            }
        }

        protected virtual T VisitArrayAccess(ArrayAccessBindingParserNode node)
        {
            return DefaultVisit(node);
        }
        
        protected virtual T VisitBinaryOperator(BinaryOperatorBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitConditionalExpression(ConditionalExpressionBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitFunctionCall(FunctionCallBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitIdentifierName(IdentifierNameBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitLiteralExpression(LiteralExpressionBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitMemberAccess(MemberAccessBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitParenthesizedExpression(ParenthesizedExpressionBindingParserNode node)
        {
            return DefaultVisit(node);
        }

        protected virtual T VisitUnaryOperator(UnaryOperatorBindingParserNode node)
        {
            return DefaultVisit(node);
        }


        protected virtual T DefaultVisit(BindingParserNode node)
        {
            throw new NotImplementedException("The visitor implementation should implement this method!");
        }
    }
}