using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace cc_lab1
{
    public class TompsonDFABuildAlgorithm : IDFABuildAlgorithm
    {
        public List<Vertex> States { get; set; }
        public List<BaseEdge<Vertex>> Edges { get; set; }
        public HashSet<char> Tokens { get; set; }

        public TompsonDFABuildAlgorithm(NFA nfa)
        {
            SetNFA(nfa);
        }

        public TompsonDFABuildAlgorithm()
        {
        }

        public void SetNFA(NFA nfa)
        {
            Tokens = nfa.Tokens;
            Graph = nfa.Graph;

            StartVertex = nfa.FindStart()[0];
        }

        private BaseVertex StartVertex { get; set; }

        private BidirectionalGraph<BaseVertex, BaseEdge<BaseVertex>> Graph { get; set; }

        public void Build()
        {
            States = new List<Vertex>();
            Edges = new List<BaseEdge<Vertex>>();
            var currentDFAVertex = new Vertex()
            {
                States = EmptyClosure(new HashSet<BaseVertex> {StartVertex}), 
                IsStart = true
            };
            States.Add(currentDFAVertex);
            var queue = new Queue<Vertex>();
            queue.Enqueue(currentDFAVertex);
            while (queue.Count != 0)
            {
                currentDFAVertex = queue.Dequeue();
                foreach (var token in Tokens)
                {
                    var statesByToken = MoveDFA(currentDFAVertex.States, token);
                    if (statesByToken.Count != 0)
                    {
                        var tmpVertex = new Vertex() {States = statesByToken};
                        var alreadyExistVertex = States.FirstOrDefault(vertex => vertex.CompareStates(tmpVertex));
                        if (alreadyExistVertex == null)
                        {
                            queue.Enqueue(tmpVertex);
                            States.Add(tmpVertex);
                        }
                        else
                        {
                            tmpVertex = alreadyExistVertex;
                        }
                        Edges.Add(new BaseEdge<Vertex>(currentDFAVertex, tmpVertex, token));
                    }
                }
            }

            foreach (var vertex in States)
            {
                if (vertex.States.Any(baseVertex => baseVertex.IsFinish))
                    vertex.IsFinish = true;
            }
        }

        private HashSet<BaseVertex> MoveDFA(HashSet<BaseVertex> states, char token)
        {
            var test = MoveNFA(states, token);
            return test.Count == 0 ? test : EmptyClosure(test);
        }

        private HashSet<BaseVertex> MoveNFA(HashSet<BaseVertex> states, char token)
        {
            var statesSet = new HashSet<BaseVertex>();
            var checkSet = new HashSet<BaseVertex>(states);
            while (checkSet.Count != 0)
            {
                var state = checkSet.First();
                var findStates = FindVertex(state, token);

                var statesNotInStatesSet = findStates.Where(vertex => !statesSet.Contains(vertex));
                foreach (var st in statesNotInStatesSet)
                    checkSet.Add(st);

                foreach (var fst in findStates)
                    statesSet.Add(fst);

                checkSet.Remove(state);
            }
            return statesSet;
        }

        private HashSet<BaseVertex> FindVertex(BaseVertex state, char token)
        {
            var findsStates = new HashSet<BaseVertex>();
            var edges = Graph.OutEdges(state).Where(edge => token.Equals(edge.Tag));
            foreach (var edge in edges)
                findsStates.Add(edge.Target);
            
            return findsStates;
        }
        
        private HashSet<BaseVertex> EmptyClosure(HashSet<BaseVertex> states)
        {
            return ConcatenateSet(MoveNFA(states, Lexer.EmptySymbol),states);
        }

        private HashSet<T> ConcatenateSet<T>(HashSet<T> op1, HashSet<T> op2)
        {
            var result = new HashSet<T>(op1);
            foreach (var v in op2)
                result.Add(v);

            return result;
        }
    }
}