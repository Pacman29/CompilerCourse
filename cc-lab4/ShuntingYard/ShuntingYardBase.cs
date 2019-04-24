using System;
using System.Collections.Generic;
using System.Linq;

namespace cc_lab4.ShuntingYard
{
    public abstract class ShuntingYardBase<TResult, TInput>
    {
        public delegate void DebugRPNDelegate(List<object> inter, List<char> opr);
        public event DebugRPNDelegate DebugRPNSteps;
        public delegate void DebugResDelegate(List<object> res, List<TResult> var);
        public event DebugResDelegate DebugResSteps;

        public TResult Execute(List<TInput> InputList, object TagObj)
        {
            Stack<object> inter = new Stack<object>(); // output stack
            Stack<char> opr = new Stack<char>();    // operator stack

            foreach (TInput s in InputList)
            {
                if(IsNoise(s))
                    continue;
                char? o = TypecastOperator(s);
                if (IsOperator(o))
                {
                    while (opr.Count > 0)
                    {
                        char ot = opr.Peek();
                        if (IsOperator(ot) && (
                            (Association((char)o)== PrecedensAssociativity.Asso.Left  && Precedence((char)o, ot) <= 0) ||
                            (Association((char)o) == PrecedensAssociativity.Asso.Right && Precedence((char)o, ot) < 0))
                            )
                            inter.Push(opr.Pop()); 
                        else
                            break;
                    }
                    opr.Push((char)o);
                }
                else if (s.ToString() == "(")
                {
                    opr.Push('(');
                }
                else if (s.ToString() == "{")
                {
                    opr.Push('{');
                }
                else if (s.ToString() == ")")
                {
                    bool pe = false;
                    while (opr.Count > 0)
                    {   // opr to out until (
                        char sc = opr.Peek();
                        if (sc == '(')
                        {
                            pe = true;
                            break;
                        }
                        else
                            inter.Push(opr.Pop());
                    }
                    if (!pe) throw new Exception("No Left (");
                    opr.Pop(); // pop off (
                }
                else if (s.ToString() == "}")
                {
                    bool pe = false;
                    while (opr.Count > 0)
                    {   // opr to out until (
                        char sc = opr.Peek();
                        if (sc == '{')
                        {
                            pe = true;
                            break;
                        }
                        else
                            inter.Push(opr.Pop());
                    }
                    if (!pe) throw new Exception("No Left {");
                    opr.Pop(); // pop off (
                }
                else if (IsIdentifier(s))
                {
                    inter.Push(s);
                }
                else
                {
                    if (!IsNoise(s))
                        throw new Exception("Unknown token");
                }

                DebugRPNSteps?.Invoke(inter.Reverse().ToList(), opr.ToList());
            }

            // put opr to out
            while (opr.Count > 0)
                inter.Push(opr.Pop());
            DebugRPNSteps?.Invoke(inter.Reverse().ToList(), opr.ToList());

            Queue<object> res = new Queue<object>(inter.Reverse());
            Stack<TResult> var = new Stack<TResult>(); // vars stack
            if (DebugResSteps != null)
                DebugResSteps(res.ToList(), var.ToList());
            // execute output stack
            while(res.Count>0)
            {
                object o = res.Dequeue();
                if (o.GetType() == typeof(TInput))
                {
                    var.Push(TypecastIdentifier((TInput)o, TagObj));
                }
                if (o.GetType() == typeof(char))
                {
                    try
                    {
                        TResult r = var.Pop(); TResult l = var.Pop();
                        var.Push(Evaluate(l, (char)o, r));
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new Exception("Vars stack empty, incorrect input");
                    }
                    
                }

                DebugResSteps?.Invoke(res.ToList(), var.ToList());
            }
            if(var.Count != 1)
                throw new Exception("Incorrect numbers of arguments, incorrect input");
            return var.Peek(); // return result
        }
        public abstract bool IsNoise(TInput input);

        public abstract TResult Evaluate(TResult result1, char opr, TResult result2);
        public abstract TResult TypecastIdentifier(TInput input, object TagObj);
        public abstract bool IsIdentifier(TInput input);
        public abstract int Precedence(char opr1, char opr2);
        public abstract PrecedensAssociativity.Asso Association(char opr);
        public abstract bool IsOperator(char? Opr);
        public abstract char? TypecastOperator(TInput opr);
    }
}