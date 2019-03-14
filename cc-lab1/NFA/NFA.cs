using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace cc_lab1
{
    public class NFA : FA<BaseVertex,BaseEdge<BaseVertex>>
    {
        public static NFA fromPostfix(string postfix)
        {
            var nfa = new NFA();
            var stackFA = new Stack<BidirectionalGraph<BaseVertex,BaseEdge<BaseVertex>>>();
            nfa.Tokens = new HashSet<char>();
            foreach (var ch in postfix)
            {
                if (Lexer.AvailableSymbols.Contains(ch))
                {
                    stackFA.Push(GetTokenFA(ch));
                    nfa.Tokens.Add(ch);
                }
                else if (Lexer.ZeroOrMoreOperand.Equals(ch))
                {
                    stackFA.Push(GetZeroOrMoreFA(stackFA.Pop()));
                }
                else if (Lexer.OneOrMoreOperand.Equals(ch))
                {
                    stackFA.Push(GetOneOrMoreFA(stackFA.Pop()));
                }
                else if (Lexer.AndOperand.Equals(ch))
                {
                    var second = stackFA.Pop();
                    var first = stackFA.Pop();
                    stackFA.Push(getAndFA(first,second));
                }
                else if (Lexer.OrOperand.Equals(ch))
                {
                    var second = stackFA.Pop();
                    var first = stackFA.Pop();
                    stackFA.Push(getOrFA(first,second));
                }
            }
            nfa.Graph = stackFA.Pop();
            var i = 0;
            foreach (var v in nfa.Graph.Vertices)
                v.Title = (i++).ToString();
            return nfa;
        }

        private static BaseVertex getStartVertex(BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> graph)
        {
            return graph.Vertices.First(vertex => vertex.IsStart);
        }
        
        private static BaseVertex getFinishVertex(BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> graph)
        {
            return graph.Vertices.First(vertex => vertex.IsFinish);
        }
        
        private static void UnmarkVertex(BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> graph)
        {
            getStartVertex(graph).IsStart = false;
            getFinishVertex(graph).IsFinish = false;
        }

        private static (BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>>, BaseVertex, BaseVertex) initGraph()
        {
            var result = new BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>>();

            var startVertex = new BaseVertex {IsStart = true};
            var finishVertex = new BaseVertex {IsFinish = true};

            result.AddVertex(startVertex);
            result.AddVertex(finishVertex);

            return (result, startVertex, finishVertex);
        }

        public static BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> GetTokenFA(char token)
        {
            var (result, startVertex, finishVertex) = initGraph();

            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, finishVertex, token));
            
            return result;
        }
        
        public static BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> 
            GetZeroOrMoreFA(BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> op)
        {
            var (result, startVertex, finishVertex) = initGraph();
            
            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, finishVertex, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishVertex, startVertex, Lexer.EmptySymbol));

            result.AddVerticesAndEdgeRange(op.Edges);

            var startInnerVertex = getStartVertex(op);
            var finishInnerVertex = getFinishVertex(op);

            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, startInnerVertex, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishInnerVertex, finishVertex, Lexer.EmptySymbol));

            UnmarkVertex(op);
            
            return result;
        }

        public static BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> getAndFA(
            BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> firstOp,
            BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> secondOp)
        {
            var (result, startVertex, finishVertex) = initGraph();
            
            var startInnerVertex1 = getStartVertex(firstOp);
            var finishInnerVertex1 = getFinishVertex(firstOp);
            
            var startInnerVertex2 = getStartVertex(secondOp);
            var finishInnerVertex2 = getFinishVertex(secondOp);
            
            UnmarkVertex(firstOp);
            UnmarkVertex(secondOp);

            result.AddVerticesAndEdgeRange(firstOp.Edges);
            result.AddVerticesAndEdgeRange(secondOp.Edges);

            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, startInnerVertex1, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishInnerVertex1, startInnerVertex2, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishInnerVertex2, finishVertex, Lexer.EmptySymbol));

            return result;
        }
        
        public static BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> getOrFA(
            BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> firstOp,
            BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> secondOp)
        {
            var (result, startVertex, finishVertex) = initGraph();
            
            var startInnerVertex1 = getStartVertex(firstOp);
            var finishInnerVertex1 = getFinishVertex(firstOp);
            
            var startInnerVertex2 = getStartVertex(secondOp);
            var finishInnerVertex2 = getFinishVertex(secondOp);
            
            UnmarkVertex(firstOp);
            UnmarkVertex(secondOp);

            result.AddVerticesAndEdgeRange(firstOp.Edges);
            result.AddVerticesAndEdgeRange(secondOp.Edges);

            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, startInnerVertex1, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, startInnerVertex2, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishInnerVertex1, finishVertex, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishInnerVertex2, finishVertex, Lexer.EmptySymbol));

            return result;
        }
        
        public static BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> 
            GetOneOrMoreFA(BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> op)
        {
            var (result, startVertex, finishVertex) = initGraph();
            
            result.AddEdge(new BaseEdge<BaseVertex>(finishVertex, startVertex, Lexer.EmptySymbol));
            result.AddVerticesAndEdgeRange(op.Edges);

            var startInnerVertex = getStartVertex(op);
            var finishInnerVertex = getFinishVertex(op);

            result.AddEdge(new BaseEdge<BaseVertex>(startVertex, startInnerVertex, Lexer.EmptySymbol));
            result.AddEdge(new BaseEdge<BaseVertex>(finishInnerVertex, finishVertex, Lexer.EmptySymbol));

            UnmarkVertex(op);
            
            return result;
        }

        public NFA()
        {
            
        }

        public BaseVertex StartVertex => getStartVertex(Graph);
        public BaseVertex FinishVertex => getFinishVertex(Graph);
    }
}