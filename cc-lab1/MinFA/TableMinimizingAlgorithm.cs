using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cc_lab1
{
    public enum Mark
    {
        FINAL,
        DIAGONAL,
        MARK,
        NONE
    }
    
    public class TableMinimizingAlgorithm : IMinimizingAlgorithm
    {
        public int Size { get; private set; }

        public Mark[,] Matrix { get; private set; }

        public List<Vertex> States { get; set; }
        public List<BaseEdge<Vertex>> Edges { get; set; }
        public HashSet<char> Tokens { get; set; }
        
        public TableMinimizingAlgorithm(DFA dfa)
        {
            SetDFA(dfa);
        }

        public TableMinimizingAlgorithm()
        {
            
        }

        public void SetDFA(DFA dfa)
        {
            States = dfa.Graph.Vertices.ToList();
            Edges = dfa.Graph.Edges.ToList();
            Tokens = dfa.Tokens;
            
            InitDeadState();
            
            Size = States.Count;
            Matrix = new Mark[Size,Size];

            for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                Matrix[i, j] = i == j ? Mark.DIAGONAL : Mark.NONE;
        }
        
        private void InitDeadState()
        {
            DeadState = new Vertex()
            {
                States = new HashSet<BaseVertex>()
            };
            foreach (var token in Tokens)
                Edges.Add(new BaseEdge<Vertex>(DeadState,DeadState, token));

            States.Add(DeadState);
        }

        private Vertex DeadState { get; set; }

        private int GetId(Vertex state)
        {
            var result = -1;
            for (var i = 0; i < States.Count && result == -1; i++)
                if (States[i].Compare(state))
                    result = i;
            
            return result;
        }

        private void SetMark(Pair<Vertex> pair, Mark mark)
        {
            SetMark(pair.Value1,pair.Value2, mark);
        }
        
        private void SetMark(Vertex v1, Vertex v2, Mark mark)
        {
            var id1 = GetId(v1);
            var id2 = GetId(v2);
            if (id1 != id2)
                Matrix[id1, id2] = Matrix[id2, id1] = mark;
        }

        private Mark GetMark(Pair<Vertex> pair)
        {
            return GetMark(pair.Value1, pair.Value2);
        }

        private Mark GetMark(Vertex v1, Vertex v2)
        {
            var id1 = GetId(v1);
            var id2 = GetId(v2);

            return Matrix[id1, id2];
        }

        public void Build()
        {
            InitFinalCells();
            Console.WriteLine(ToString());
            Marking();
            Console.WriteLine(ToString());
            BuildNewGraph();
        }

        private void BuildNewGraph()
        {
            var newPairState = GetNoneMarkPairs();
            if(newPairState.Count == 0)
                return;
            
            var newStates = States.ToList();
            var deletedStates = new List<Vertex>();
            foreach (var oldState in States)
                foreach (var pair in newPairState)
                    if (oldState.Compare(pair.Value1) || oldState.Compare(pair.Value2))
                    {
                        newStates.Remove(oldState);
                        deletedStates.Add(oldState);
                    }

            var createdCombineVertex = new List<CombineVertex>();
            foreach (var pair in newPairState)
            {
                var cv = new CombineVertex(new HashSet<Vertex>() {pair.Value1, pair.Value2});
                createdCombineVertex.Add(cv);
                newStates.Add(cv);
            }

            var deletedEdges = new List<BaseEdge<Vertex>>();
            var newEdges = new HashSet<BaseEdge<Vertex>>(Edges.ToList());

            foreach (var oldEdge in Edges)
                if (deletedStates.Any(
                    vertex => oldEdge.Source.Compare(vertex) || oldEdge.Target.Compare(vertex)))
                {
                    newEdges.Remove(oldEdge);
                    deletedEdges.Add(oldEdge);
                }

            foreach (var deletedEdge in deletedEdges)
            {
                if (deletedStates.Any(vertex => deletedEdge.Target.Compare(vertex) &&
                                                deletedEdge.Source.Compare(vertex)))
                {
                    var sourceTargetVertex = createdCombineVertex.Find(
                        vertex => vertex.Vertices.Contains(deletedEdge.Target) 
                                  && vertex.Vertices.Contains(deletedEdge.Source));
                    var newEdge = new BaseEdge<Vertex>(sourceTargetVertex,sourceTargetVertex,deletedEdge.Tag);
                    if(!newEdges.Any(edge => edge.Compare(newEdge)))
                        newEdges.Add(newEdge);
                }
                else if(!newPairState.Any(pair => pair.SamePair(new Pair<Vertex>(deletedEdge.Source,deletedEdge.Target))))
                {                        
                    if (deletedStates.Any(vertex => deletedEdge.Target.Compare(vertex)))
                    {
                        var targetVertex = createdCombineVertex.Find(
                            vertex => vertex.Vertices.Contains(deletedEdge.Target));
                        var newEdge = new BaseEdge<Vertex>(deletedEdge.Source,targetVertex,deletedEdge.Tag);
                        if(!newEdges.Any(edge => edge.Compare(newEdge)))
                            newEdges.Add(newEdge);
                    } 
                    else if (deletedStates.Any(vertex => deletedEdge.Source.Compare(vertex)))
                    {
                        var sourceVertex = createdCombineVertex.Find(
                            vertex => vertex.Vertices.Contains(deletedEdge.Source));
                        var newEdge = new BaseEdge<Vertex>(sourceVertex,deletedEdge.Target,deletedEdge.Tag);
                        if(!newEdges.Any(edge => edge.Compare(newEdge)))
                            newEdges.Add(newEdge);
                    }
                }
            }
            
            States = newStates.Where(state => state.GetHashCode() != DeadState.GetHashCode()).ToList();
            Edges = newEdges.Where(edge => edge.Source.GetHashCode() != DeadState.GetHashCode() && 
                                           edge.Target.GetHashCode() != DeadState.GetHashCode()).ToList();
        }

        private void InitFinalCells()
        {
            foreach (var finalState in States.Where(state => state.IsFinish))
            {
                var f = GetId(finalState);
                for (int i = 0; i < Size; i++)
                    Matrix[i, f] = Matrix[f, i] = Mark.FINAL;
            }
        }

        private List<Pair<Vertex>> GetNoneMarkPairs()
        {
            var result = new List<Pair<Vertex>>();
            for (int i = 1; i < Size; i++)
                for (int j = 0; j < i; j++)
                    if(Matrix[i,j] == Mark.NONE)  
                        result.Add(new Pair<Vertex>(States[i],States[j]));

            return result;
        }

        private Vertex NextVertexByToken(Vertex source, char token)
        {
            Vertex result = null;
            var e =  Edges.FirstOrDefault(edge => edge.Source.Compare(source) && edge.Tag.Equals(token));
            if (e != null)
                result = e.Target;
            return result;
        }

        private void Marking()
        {
            var queue = new Queue<Pair<Vertex>>(GetNoneMarkPairs());
            while (queue.Count != 0)
            {
                //Console.WriteLine(ToString());
                var checkPair = queue.Dequeue();
                //Console.WriteLine(checkPair);
                foreach (var token in Tokens)
                {
                    var checkPairMark = GetMark(checkPair);
                    if (checkPairMark != Mark.MARK && checkPairMark != Mark.FINAL)
                    {
                        var nextV1 = NextVertexByToken(checkPair.Value1, token);
                        var nextV2 = NextVertexByToken(checkPair.Value2, token);

                        if (nextV1 == null)
                            nextV1 = DeadState;
                        
                        if (nextV2 == null)
                            nextV2 = DeadState;

                        
                        var mark = GetMark(nextV1, nextV2);
                        switch (mark)
                        {
                            case Mark.NONE:
                            {
                                var newPair = new Pair<Vertex>(nextV1, nextV2);
                                if (!queue.Any(pair => pair.SamePair(newPair)))
                                {
                                    queue.Enqueue(newPair);
                                }
                                queue.Enqueue(checkPair);
                                break;
                            }
                            case Mark.FINAL:
                            case Mark.MARK:
                            {
                                SetMark(checkPair,Mark.MARK);
                                break;
                            }
                            case Mark.DIAGONAL:
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var table = new StringBuilder();
            var header = new StringBuilder();
            header.Append(" \t");
            for (int i = 0; i < Size; i++)
                header.Append(i + "\t");

            header.Append("\n");
            table.Append(header);
            
            for (int i = 0; i < Size; i++)
            {
                var row = new StringBuilder();
                row.Append(i+"\t");
                for (int j = 0; j < Size; j++)
                {
                    var mark = Matrix[i, j];
                    var str = " ";
                    switch (mark)
                    {
                        case Mark.FINAL:
                            str = "F";
                            break;
                        case Mark.MARK:
                            str = "*";
                            break;
                        case Mark.DIAGONAL:
                            str = "D";
                            break;
                        case Mark.NONE:
                            str = ".";
                            break;
                    }

                    row.Append(str + "\t");
                }

                row.Append("\n");
                table.Append(row);
            }

            return table.ToString();
        }
    }
}