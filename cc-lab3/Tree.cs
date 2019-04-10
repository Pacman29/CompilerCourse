using System.Collections.Generic;
using System.IO;
using cc_lab1;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace cc_lab3
{
    public class Tree
    {
        public string Data { get; set; } = "";
        public LinkedList<Tree> Children { get; set; }  = new LinkedList<Tree>();


        public Tree Add(string val)
        {
            var res = new Tree()
            {
                Data = val
            };
            
            Children.AddFirst(res);
            return res;
        }

        public Tree GetChild(int i)
        {
            foreach (var n in Children)
                if (--i == 0)
                    return n;
            return null;
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

        private void ReadTree(Tree root, BidirectionalGraph<Tree, Edge<Tree>> graph)
        {
            graph.AddVertex(this);
            foreach (var rootChild in root.Children)
            {
                graph.AddVerticesAndEdge(new Edge<Tree>(root, rootChild));
                ReadTree(rootChild, graph);
            }
        }
        
        public void PrintTree(string filename)
        {
            var graph = new BidirectionalGraph<Tree, Edge<Tree>>();
            ReadTree(this, graph);
            
            var graphViz = new GraphvizAlgorithm<Tree, Edge<Tree>>(graph, @".", QuickGraph.Graphviz.Dot.GraphvizImageType.Png);
            graphViz.FormatVertex += FormatVertex;
            graphViz.FormatEdge += FormatEdge;
            graphViz.Generate(new FileDotEngine(), filename);
            $"dot -T png {filename} > {filename}.png".Bash();
        }

        private static void FormatEdge(object sender, FormatEdgeEventArgs<Tree, Edge<Tree>> e)
        {
        }

        private static void FormatVertex(object sender, FormatVertexEventArgs<Tree> e)
        {
            e.VertexFormatter.Label = e.Vertex.Data;
        }
    }
}