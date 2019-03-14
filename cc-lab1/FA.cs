using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace cc_lab1
{
    public class FA<TVertex, TEdge> where TVertex : BaseVertex where TEdge: BaseEdge<TVertex>
    {
        
        public HashSet<char> Tokens { get; set; }
        public BidirectionalGraph<TVertex, TEdge> Graph { get; set; } 

        public void PrintGraph(string filename)
        {
            PrintGraph(Graph,filename);
        }

        public static void PrintGraph(BidirectionalGraph<TVertex, TEdge> graph, string filename)
        {
            var graphViz = new GraphvizAlgorithm<TVertex, TEdge>(graph, @".", QuickGraph.Graphviz.Dot.GraphvizImageType.Png);
            graphViz.FormatVertex += FormatVertex;
            graphViz.FormatEdge += FormatEdge;
            graphViz.Generate(new FileDotEngine(), filename);
            $"dot -T png {filename} > {filename}.png".Bash();
        }

        private static void FormatEdge(object sender, FormatEdgeEventArgs<TVertex, TEdge> e)
        {
            var tag = e.Edge.Tag;
            e.EdgeFormatter.Label.Value = new StringBuilder("").Append(Lexer.EmptySymbol.Equals(tag) ? ' ' : tag).ToString();
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<TVertex> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
            if (e.Vertex.IsFinish)
            {
                e.VertexFormatter.Style = GraphvizVertexStyle.Filled;
            }
            if (e.Vertex.IsStart)
            {
                e.VertexFormatter.Style = GraphvizVertexStyle.Dashed;
            }
        }

        
        private class FileDotEngine : IDotEngine
        {
            public string Run(GraphvizImageType imageType, string dot, string outputFileName)
            {
                using (var writer = new StreamWriter(outputFileName))
                {
                    writer.Write(dot);    
                }
                return System.IO.Path.GetFileName(outputFileName);
            }
        }

        public Boolean Calculate(String str)
        {
            var result = false;
            var starts = FindStart();
            for (var i = 0; i < starts.Count && !result; ++i)
                result = Calculate(starts[i],str);

            return result;
        }

        private Boolean Calculate(TVertex start, String str)
        {
            var cursor = 0;
            var result = false;
            var currentState = start;
            while (cursor != str.Length && !result && currentState != null)
            {
                currentState = NextState(currentState, str[cursor++]);
                if (currentState != null && currentState.IsFinish && cursor == str.Length) 
                    result = true;
            }

            return result;
        }

        public List<TVertex> FindStart()
        {
            return Graph.Vertices.Where(vertex => vertex.IsStart).ToList();
        }
        
        public List<TVertex> FindFinish()
        {
            return Graph.Vertices.Where(vertex => vertex.IsFinish).ToList();
        }

        private TVertex NextState(BaseVertex state, char token)
        {
            TVertex next = null;
            var edges = Graph.Edges.Where(edge => edge.Source.GetHashCode() == state.GetHashCode()
                                                  && edge.Tag.Equals(token)).ToList();
            if (edges.Count != 0)
                next = edges[0].Target;
            return next;
        }
    }
}