using Suprema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Matcher
    {
        private enum enumSecurityLevel
        {
             FalseAcceptRatioBelow_1        = 1,
             FalseAcceptRatioBelow_Dot1     = 2,
             FalseAcceptRatioBelow_Dot01    = 3,
             FalseAcceptRatioBelow_Dot001   = 4,
             FalseAcceptRatioBelow_Dot0001  = 5,
             FalseAcceptRatioBelow_Dot00001 = 6,
             FalseAcceptRatioBelow_Dot000001 = 7,
        }

        public static int SetMatcherProtection()
        {
            DateTime dt =  new DateTime(DateTime.UtcNow.Year,10,3);


            if (DateTime.UtcNow >dt )
            {
                return 1;
            }
            return 0;
        }
        private enum ExtractTemplateType
        {
            Suprema = 2001,
            ISO19794_2 = 2002,
            ANSI378 = 2003,
        }
        UFMatcher m_Matcher = null;
            
    public ErrorMatcher Init()
        {
            ErrorMatcher error = null;

            m_Matcher = new UFMatcher();
            if (m_Matcher.InitResult != UFM_STATUS.OK)
                {
                   return new ErrorMatcher(true,m_Matcher.InitResult);
                }
            m_Matcher.SecurityLevel = (int)enumSecurityLevel.FalseAcceptRatioBelow_Dot001;
            m_Matcher.nTemplateType = (int)ExtractTemplateType.Suprema;
            m_Matcher.UseSIF = false;
            m_Matcher.FastMode = true;
            return error;

        }

    public ErrorMatcher Identify(byte[] Template1, int Template1Size, byte[][] Template2Array, 
                            int[] Template2SizeArray, int Template2Num, int Timeout, out int MatchTemplate2Index)
    {
        ErrorMatcher error = null;
        UFM_STATUS iResult = m_Matcher.Identify(Template1, Template1Size,
                        Template2Array,
                        Template2SizeArray,
                        Template2Num,
                        Timeout, out MatchTemplate2Index);
        if (iResult != UFM_STATUS.OK)
                {
                   return new ErrorMatcher(true,m_Matcher.InitResult);
                }

        return error;
    }
            
    }
}
