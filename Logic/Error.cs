using Suprema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Error
    {
        public string String { get; set; }
        public string StringEx { get; set; }
        public bool bCritical { get; set; }//If critical return 


        protected int _Code;

    }

    public class ErrorMatcher : Error
    {

        public ErrorMatcher(bool IsCritical, UFM_STATUS ErrorCD, string ExErString)
        {
            Code = (int)ErrorCD;
            StringEx = ExErString;
            bCritical = IsCritical;
        }

        public ErrorMatcher(bool IsCritical, UFM_STATUS ErrorCD)
        {
            Code = (int)ErrorCD;

            bCritical = IsCritical;
        }

        public int Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                string sTemp;
                UFMatcher.GetErrorString((UFM_STATUS)_Code, out sTemp);
                String = sTemp;
            }
        }

    }

}
