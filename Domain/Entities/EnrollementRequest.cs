using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EnrollementRequest
    {
        public enum enumType
        {
            AllFingersEnrollment = 1,
            AllFingersIdentification = 2,
            SingleFingersIdentification = 3,
            SearchByApplicantID = 4,
            SearchByPID = 5,
            SearchByTazkaraID = 6,
        }
        public enumType Type;
        public int m_ApplicantId;
        public Guid SPISApplicationId;
        public string m_PID;
        public string m_TazkaraID;
        public int m_UserID;

        public bool m_isNewApplicant;
        public string m_ScannerSN;

        public byte[][] m_arrTemplates = new byte[10][];
        public int[] m_arrTemplateSizes = new int[10];
        public int[] m_arrTemplateQuality = new int[10];
        public int[] m_arrImageQuality = new int[10];

        //public byte[] RightThumb;
        //public byte[] RightIndex;
        //public byte[] RightMiddle;
        //public byte[] RightRing;
        //public byte[] RightLittle;
        //public byte[] LeftThumb;
        //public byte[] LeftIndex;
        //public byte[] LeftMiddle;
        //public byte[] LeftRing;
        //public byte[] LeftLittle;


        //public int RightThumbLen;
        //public int RightIndexLen;
        //public int RightMiddleLen;
        //public int RightRingLen;
        //public int RightLittleLen;
        //public int LeftThumbLen;
        //public int LeftIndexLen;
        //public int LeftMiddleLen;
        //public int LeftRingLen;
        //public int LeftLittleLen;



        //public int RightThumbQuality;
        //public int RightIndexQuality;
        //public int RightMiddleQuality;
        //public int RightRingQuality;
        //public int RightLittleQuality;
        //public int LeftThumbQuality;
        //public int LeftIndexQuality;
        //public int LeftMiddleQuality;
        //public int LeftRingQuality;
        //public int LeftLittleQuality;
    }
}
