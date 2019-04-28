using System;
using System.Text;

namespace cc_lab4.SyntaxAnalyzer
{
    public enum TokenType
    {
		T_PLUS = 0,
		T_MINUS,
		T_MULT,
		T_DIV,	
		T_UMINUS,	
		T_EXP,		
		T_OPAREN,
		T_CPAREN,		
		T_EOL,	
		T_NUMBER,	
		T_UPLUS,
		T_UNKNOWN
    }

    public struct Token
    {
        public TokenType Type;
        public string str;
        public double Value;
    }

    public class CLexer
    {
        public CLexer()
        {
            m_strExpr = "";
            m_nNextPos = 0;
            m_PreviousTokenType = TokenType.T_EOL;
            m_currToken = new Token();
        }

        public void SetExpr(string strExpr)
        {
            m_strExpr = strExpr;
        }

        private bool isdigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        public TokenType GetNextToken()
        {
            StringBuilder strToken = new StringBuilder("", 256);

            if (m_nNextPos >= m_strExpr.Length)
            {
                m_currToken.Type = TokenType.T_EOL;
                m_currToken.str = "EOL";
                m_nNextPos = 0;
                m_PreviousTokenType = TokenType.T_EOL;
                return TokenType.T_EOL;
            }

            char[] strExpr = new char[1024];
            strExpr = m_strExpr.ToCharArray();

            while (true)
            {
                while (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos++] == ' '){}
                --m_nNextPos;

                if (m_nNextPos >= m_strExpr.Length)
                {
                    m_currToken.Type = TokenType.T_EOL;
                    m_currToken.str = "EOL";
                    m_nNextPos = 0;
                    m_PreviousTokenType = TokenType.T_EOL;
                    return TokenType.T_EOL;
                }
                else if (isdigit(strExpr[m_nNextPos]))
                {
                    while (m_nNextPos < m_strExpr.Length && isdigit(strExpr[m_nNextPos]))
                    {
                        strToken.Append(strExpr[m_nNextPos]);
                        m_nNextPos++;
                    }

                    if (m_nNextPos < m_strExpr.Length && (strExpr[m_nNextPos] == '.' || strExpr[m_nNextPos] == ','))
                    {
                        strToken.Append(',');
                        m_nNextPos++;
                        while (m_nNextPos < m_strExpr.Length && isdigit(strExpr[m_nNextPos]))
                        {
                            strToken.Append(strExpr[m_nNextPos]);
                            m_nNextPos++;
                        }
                        m_PreviousTokenType = m_currToken.Type;
                        m_currToken.Type = TokenType.T_NUMBER;
                        m_currToken.str = strToken.ToString();
                        m_currToken.Value = Convert.ToDouble(strToken.ToString());
                        return TokenType.T_NUMBER;
                    }
                    else
                    {
                        m_PreviousTokenType = m_currToken.Type;
                        m_currToken.Type = TokenType.T_NUMBER;
                        m_currToken.str = strToken.ToString();
                        m_currToken.Value = Convert.ToDouble(strToken.ToString());
                        return TokenType.T_NUMBER;
                    }
                }
                else if (m_nNextPos < m_strExpr.Length && (strExpr[m_nNextPos] == '.' || strExpr[m_nNextPos] == ','))
                {
                    strToken.Append(',');
                    m_nNextPos++;
                    while (m_nNextPos < m_strExpr.Length && isdigit(strExpr[m_nNextPos]))
                    {
                        strToken.Append(strExpr[m_nNextPos]);
                        m_nNextPos++;
                    }
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_NUMBER;
                    m_currToken.str = strToken.ToString();
                    m_currToken.Value = Convert.ToDouble(strToken.ToString());
                    return TokenType.T_NUMBER;
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == '(')
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_OPAREN;
                    m_currToken.str = "(";
                    ++m_nNextPos;
                    return TokenType.T_OPAREN;
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == ')')
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_CPAREN;
                    m_currToken.str = ")";
                    ++m_nNextPos;
                    return TokenType.T_CPAREN;
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == '+')
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.str = "+";
                    ++m_nNextPos;
                    m_currToken.Type = TokenType.T_PLUS;
                    return TokenType.T_PLUS;
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == '-')
                {
                    m_currToken.str = "-";
                    ++m_nNextPos;
                    m_PreviousTokenType = m_currToken.Type;
                    if (m_PreviousTokenType == TokenType.T_CPAREN || m_PreviousTokenType == TokenType.T_NUMBER)
                    {
                        m_currToken.Type = TokenType.T_MINUS;
                        return TokenType.T_MINUS;
                    }
                    else
                    {
                        m_currToken.Type = TokenType.T_UMINUS;
                        return TokenType.T_UMINUS;
                    }
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == '*')
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_MULT;
                    m_currToken.str = "*";
                    ++m_nNextPos;
                    return TokenType.T_MULT;
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == '/')
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_DIV;
                    m_currToken.str = "/";
                    ++m_nNextPos;
                    return TokenType.T_DIV;
                }
                else if (m_nNextPos < m_strExpr.Length && strExpr[m_nNextPos] == '^')
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_EXP;
                    m_currToken.str = "^";
                    ++m_nNextPos;
                    return TokenType.T_EXP;
                }                
                else
                {
                    m_PreviousTokenType = m_currToken.Type;
                    m_currToken.Type = TokenType.T_UNKNOWN;
                    m_currToken.str = strExpr[m_nNextPos].ToString();
                    ++m_nNextPos;
                    return TokenType.T_UNKNOWN;
                }
            }
        }

        public Token m_currToken;
        private string m_strExpr;
        private int m_nNextPos;
        private TokenType m_PreviousTokenType;
    }
}