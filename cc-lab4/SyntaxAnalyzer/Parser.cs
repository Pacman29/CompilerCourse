using System;
using System.Collections.Generic;

namespace cc_lab4.SyntaxAnalyzer
{
    public class CParser
    {
        public CParser()
        {
            m_top = -1;            
            m_value = 0;
            m_stack = new List<double>();
            m_stackOpr = new List<TokenType>();
            m_Lexer = new CLexer();
        }
        
		bool shift()
		{
			if ( m_topOpr >= MAXSTACK )
			{
				Console.WriteLine("Error 4: stack operator overflow.");
				return false;
			}
		
			m_stackOpr.Add(m_Lexer.m_currToken.Type);
			++m_topOpr;
	
			Console.WriteLine("SHIFT:");
			m_stackOpr.ForEach(type => Console.Write($"{type}\t"));
			Console.WriteLine();
			
			m_Lexer.GetNextToken();
	
			return true;
		}

		bool reduce()
		{
			double right;
	
			if ( m_topOpr < 1 )
			{
				Console.WriteLine("Error 5: missing operator or parenthesis.");
				return false;
			}		
		
			if ( m_top < 1 && m_stackOpr[m_topOpr] != TokenType.T_OPAREN && m_stackOpr[m_topOpr] != TokenType.T_CPAREN )
			{
				if ( m_stackOpr[m_topOpr] != TokenType.T_UMINUS )
				{
					Console.WriteLine("Error 6: missing operand.");
					return false;
				}
				else
				{
					if ( m_top < 0 )
					{
						Console.WriteLine("Error 7: missing operand.");
						return false;
					}
				}
			}
					
			switch ( m_stackOpr[m_topOpr] )
			{
				case TokenType.T_PLUS:			
					right = m_stack[m_top--];
					m_stack[m_top] += right;
					break;
				case TokenType.T_MINUS:
					right = m_stack[m_top--];
					m_stack[m_top] -= right;		
					break;
				case TokenType.T_MULT:
					right = m_stack[m_top--];
					m_stack[m_top] *= right;
					break;
				case TokenType.T_DIV:
					right = m_stack[m_top--];
					if ( right == 0 )
					{
						Console.WriteLine("Error 8: division by 0");
						return false;
					}
					m_stack[m_top] /= right;
					break;
				case TokenType.T_UMINUS:
					m_stack[m_top] *= -1;
					break;
				case TokenType.T_EXP:
					right = m_stack[m_top--];
					m_stack[m_top] = Math.Pow(m_stack[m_top], right);
					break;					
				case TokenType.T_OPAREN:
					if ( m_Lexer.m_currToken.Type == TokenType.T_CPAREN )
						m_Lexer.GetNextToken();
					break;			
				case TokenType.T_CPAREN:
					break;											
				default:
					Console.WriteLine("Error 9: {0} {1}", m_Lexer.m_currToken.str, m_stackOpr[m_topOpr]);
					return false;
			}
	
			Console.WriteLine("REDUCE:");
			m_stackOpr.ForEach(type => Console.Write($"{type}\t"));
			Console.WriteLine();
			
			m_topOpr--;
		
			return true;
		}
        
        public bool Parse(string strExpr)
        {
            //bool ret = true;
			const int S  = 0; /* shift */
			const int R  = 1; /* reduce */
			const int A  = 2; /* accept */  
			const int E1 = 3; /* error: missing right parenthesis */
			const int E2 = 4; /* error: missing operator */
			const int E3 = 5; /* error: unbalanced parenthesis */    			          
            
			int[,] parseTable = new int[,]
			{
				/* stack   -------- input ----------- */
				/*         +   -   *   /   UM  ^   $  */
				/*         --  --  --  --  --  --  -- */
				/* +  */ { R,  R,  S,  S,  S,  S,  S,  R,  R },
				/* -  */ { R,  R,  S,  S,  S,  S,  S,  R,  R },
				/* *  */ { R,  R,  R,  R,  S,  S,  S,  R,  R },
				/* /  */ { R,  R,  R,  R,  S,  S,  S,  R,  R },
				/* UM */ { R,  R,  R,  R,  S,  S,  S,  R,  R },
				/* ^  */ { R,  R,  R,  R,  R,  S,  S,  R,  R },
				/* (  */ { S,  S,  S,  S,  S,  S,  S,  R,  E1},
				/* )  */ { R,  R,  R,  R,  R,  R,  E2, R,  R },
				/* $  */ { S,  S,  S,  S,  S,  S,  S,  E3, A }												
			};            
            
            //m_strExpr = strExpr;
            m_top = -1;
            m_value = 0;
            
			m_topOpr = 0;
			m_stackOpr.Add(TokenType.T_EOL);
            
            m_Lexer.SetExpr(strExpr);

            m_Lexer.GetNextToken();
            
			if ( m_Lexer.m_currToken.Type == TokenType.T_EOL )
			{
				return true;
			}

			while ( true )
			{		
				switch ( m_Lexer.m_currToken.Type )
				{
					case TokenType.T_UNKNOWN:
						Console.WriteLine("Error 0: invalid token: {0}", m_Lexer.m_currToken.str);
						return false;			
					case TokenType.T_NUMBER:
						m_stack.Add(m_Lexer.m_currToken.Value);
						++m_top;
						m_Lexer.GetNextToken();
						break;
					case TokenType.T_UPLUS:
						m_Lexer.GetNextToken();
						break;
					default:
						switch ( parseTable[(int)m_stackOpr[m_topOpr], (int)m_Lexer.m_currToken.Type] )
						{
							case S:
								if ( !shift() )
									return false;
								break;
							case R:
								if ( !reduce() )
									return false;
								break;
							case A:
								if ( m_top != 0 )
								{
									Console.WriteLine("Error 10: missing operator.");
									return false;
								}
								if ( m_topOpr != 0 )
								{
									Console.WriteLine("Error 11: missing operand.");
									return false;
								}						
								m_value = m_stack[m_top--];
								return true;
							case E1:
								Console.WriteLine("Error 1: missing right parenthesis");
								return false;
							case E2:
								Console.WriteLine("Error 2: missing operator");
								return false;					
							case E3:
								Console.WriteLine("Error 3: unbalanced parenthesis");
								return false;														
						}
						break;
				}		
			}            
        }

        public double GetValue()
        {
            return m_value;
        }

        private CLexer m_Lexer;
        //private string m_strExpr;
        private int m_top;
        private List<double> m_stack;
		private int m_topOpr;
		private List<TokenType> m_stackOpr;
        private double m_value;
		private const int MAXSTACK = 1024;
    }
}