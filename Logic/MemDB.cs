using Suprema;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace Logic
{
    public   class MemDB
    {

        // Finger position for segmentation
        //
        public const int RS_FGP_RIGHT_THUMB = 1;
        public const int RS_FGP_RIGHT_INDEX = 2;
        public const int RS_FGP_RIGHT_MIDDLE = 3;
        public const int RS_FGP_RIGHT_RING = 4;
        public const int RS_FGP_RIGHT_LITTLE = 5;

        public const int RS_FGP_LEFT_THUMB = 6;
        public const int RS_FGP_LEFT_INDEX = 7;
        public const int RS_FGP_LEFT_MIDDLE = 8;
        public const int RS_FGP_LEFT_RING = 9;
        public const int RS_FGP_LEFT_LITTLE = 10;

        private enum enumSecurityLevel
        {
            FalseAcceptRatioBelow_1 = 1,
            FalseAcceptRatioBelow_Dot1 = 2,
            FalseAcceptRatioBelow_Dot01 = 3,
            FalseAcceptRatioBelow_Dot001 = 4,
            FalseAcceptRatioBelow_Dot0001 = 5,
            FalseAcceptRatioBelow_Dot00001 = 6,
            FalseAcceptRatioBelow_Dot000001 = 7,
        }

        
        private enum ExtractTemplateType
        {
            Suprema = 2001,
            ISO19794_2 = 2002,
            ANSI378 = 2003,
        }

        static int[] m_FingerMatchingOrder = {      RS_FGP_RIGHT_THUMB,
                                                RS_FGP_LEFT_THUMB,
                                                RS_FGP_RIGHT_INDEX,
                                                RS_FGP_LEFT_INDEX,
                                                RS_FGP_RIGHT_MIDDLE,
                                                RS_FGP_LEFT_MIDDLE,
                                                RS_FGP_RIGHT_LITTLE,
                                                RS_FGP_LEFT_LITTLE,
                                                RS_FGP_RIGHT_RING,
                                                RS_FGP_LEFT_RING,
                                         };

        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public int Count = -1;

        public int[] arrApplicantIDs;
        public HashSet<int> hashEnrolledApplicants ; 


        //public byte[] arrApplicant;//i.e arrApplicant[i] = applicantID;  where i is the index of the array below

         public byte[][] RightThumb;
         public byte[][] RightIndex;
         public byte[][] RightMiddle;
         public byte[][] RightRing;
         public byte[][] RightLittle;
         public byte[][] LeftThumb;
         public byte[][] LeftIndex;
         public byte[][] LeftMiddle;
         public byte[][] LeftRing;
         public byte[][] LeftLittle;


         public int[] RightThumbLen;
         public int[] RightIndexLen;
         public int[] RightMiddleLen;
         public int[] RightRingLen;
         public int[] RightLittleLen;
         public int[] LeftThumbLen;
         public int[] LeftIndexLen;
         public int[] LeftMiddleLen;
         public int[] LeftRingLen;
         public int[] LeftLittleLen;



         //public int[] RightThumbQuality;
         //public int[] RightIndexQuality;
         //public int[] RightMiddleQuality;
         //public int[] RightRingQuality;
         //public int[] RightLittleQuality;
         //public int[] LeftThumbQuality;
         //public int[] LeftIndexQuality;
         //public int[] LeftMiddleQuality;
         //public int[] LeftRingQuality;
         //public int[] LeftLittleQuality;







         UFMatcher m_Matcher = null;
        ~MemDB()
        {
            if (cacheLock != null) cacheLock.Dispose();
        }


         public ErrorMatcher Init()
        {
            ErrorMatcher error = null;

            m_Matcher = new Suprema.UFMatcher();
            if (m_Matcher.InitResult != UFM_STATUS.OK)
            {
                return new ErrorMatcher(true, m_Matcher.InitResult);
            }
            m_Matcher.SecurityLevel = (int)enumSecurityLevel.FalseAcceptRatioBelow_Dot001;
            m_Matcher.nTemplateType = (int)ExtractTemplateType.Suprema;
            m_Matcher.UseSIF = false;
            m_Matcher.FastMode = true;
            return error;

        }


         public ErrorMatcher IdentifyMT(byte[][] Templates,
                                   int[] TemplateLen,
                               int Timeout, out int MatchTemplate2Index, out int FingerPosition, out int applicantID)
        {
            UFM_STATUS iResult = UFM_STATUS.OK;
            int len = Templates.Count();
            byte[][] arrTemplate2 = null;
            int[] arrTemplateSize = null;
            MatchTemplate2Index = -1;
            FingerPosition = 0;
            applicantID = -1;
            cacheLock.EnterReadLock();
            try
            {
                // return innerCache[key];
                for (int i = 0; i < m_FingerMatchingOrder.Length; i++)
                {
                    if (TemplateLen[m_FingerMatchingOrder[i]-1] != 0)
                    {
                        switch (m_FingerMatchingOrder[i])
                        {
                            case RS_FGP_RIGHT_THUMB:
                                arrTemplate2 = RightThumb;
                                arrTemplateSize = RightThumbLen;
                                break;
                            case RS_FGP_RIGHT_INDEX:
                                arrTemplate2 = RightIndex;
                                arrTemplateSize = RightIndexLen;
                                break;
                            case RS_FGP_RIGHT_MIDDLE:
                                arrTemplate2 = RightMiddle;
                                arrTemplateSize = RightMiddleLen;
                                break;
                            case RS_FGP_RIGHT_RING:
                                arrTemplate2 = RightRing;
                                arrTemplateSize = RightRingLen;
                                break;
                            case RS_FGP_RIGHT_LITTLE:
                                arrTemplate2 = RightLittle;
                                arrTemplateSize = RightLittleLen;
                                break;

                            case RS_FGP_LEFT_THUMB:
                                arrTemplate2 = LeftThumb;
                                arrTemplateSize = LeftThumbLen;
                                break;
                            case RS_FGP_LEFT_INDEX:
                                arrTemplate2 = LeftIndex;
                                arrTemplateSize = LeftIndexLen;
                                break;
                            case RS_FGP_LEFT_MIDDLE:
                                arrTemplate2 = LeftMiddle;
                                arrTemplateSize = LeftMiddleLen;
                                break;
                            case RS_FGP_LEFT_RING:
                                arrTemplate2 = LeftRing;
                                arrTemplateSize = LeftRingLen;
                                break;
                            case RS_FGP_LEFT_LITTLE:
                                arrTemplate2 = LeftLittle;
                                arrTemplateSize = LeftLittleLen;
                                break;

                        }
                        int index = m_FingerMatchingOrder[i]-1;
                        iResult = m_Matcher.IdentifyMT(
                                Templates[index],
                                TemplateLen[index],
                                arrTemplate2,
                                arrTemplateSize,
                                Count,
                                Timeout, out MatchTemplate2Index);
                        if (iResult != UFM_STATUS.OK)
                        {
                            return new ErrorMatcher(true, iResult);
                        }
                        else if (MatchTemplate2Index != -1)
                        {
                            FingerPosition = m_FingerMatchingOrder[i];
                            applicantID = arrApplicantIDs[MatchTemplate2Index]; 
                            return null;

                        }// if not null

                    }//for

                }//try

            }
            finally
            {
                cacheLock.ExitReadLock();
            }



            return null;
        }

         //public int GetApplicantIDByIndex(int Index)
         //{
         //    int ApplicantID = -1;
         //    cacheLock.EnterReadLock();
         //    try
         //    {
         //        ApplicantID = arrApplicantIDs[Index];
         //    }
         //    finally
         //    {
         //        cacheLock.ExitReadLock();
         //    }
         //    return ApplicantID;
         //}

         public bool IfApplicantIDEnrolled(int  ApplicantID)
         { 
             cacheLock.EnterReadLock();
             try
             {
                 if (hashEnrolledApplicants.Contains(ApplicantID) == true)
                 {
                     return true;
                 }
             }
             finally
             {
                 cacheLock.ExitReadLock();
             }
             return false;
         }
        public void Add(int ApplicantID, byte[][] arrTemplates, int[] arrTemplateSizes, int[] arrTemplateQuality)
        {

            cacheLock.EnterWriteLock();
            try
            {
                arrApplicantIDs[Count] = ApplicantID;
                hashEnrolledApplicants.Add(ApplicantID);
                
                //Templates
                RightThumb[Count] = arrTemplates[RS_FGP_RIGHT_THUMB -1] ?? new byte[0];
                RightIndex[Count] = arrTemplates[RS_FGP_RIGHT_INDEX - 1] ?? new byte[0];
                RightMiddle[Count] = arrTemplates[RS_FGP_RIGHT_MIDDLE - 1] ?? new byte[0];
                RightRing[Count] = arrTemplates[RS_FGP_RIGHT_RING - 1] ?? new byte[0];
                RightLittle[Count] = arrTemplates[RS_FGP_RIGHT_LITTLE - 1] ?? new byte[0];
                LeftThumb[Count] = arrTemplates[RS_FGP_LEFT_THUMB - 1] ?? new byte[0];
                LeftIndex[Count] = arrTemplates[RS_FGP_LEFT_INDEX - 1] ?? new byte[0];
                LeftMiddle[Count] = arrTemplates[RS_FGP_LEFT_MIDDLE - 1] ?? new byte[0];
                LeftRing[Count] = arrTemplates[RS_FGP_LEFT_RING - 1] ?? new byte[0];
                LeftLittle[Count] = arrTemplates[RS_FGP_LEFT_LITTLE - 1] ?? new byte[0];


                //Template Length
                RightThumbLen[Count] = arrTemplateSizes[RS_FGP_RIGHT_THUMB - 1];
                RightIndexLen[Count] = arrTemplateSizes[RS_FGP_RIGHT_INDEX - 1];
                RightMiddleLen[Count] = arrTemplateSizes[RS_FGP_RIGHT_MIDDLE - 1];
                RightRingLen[Count] = arrTemplateSizes[RS_FGP_RIGHT_RING - 1];
                RightLittleLen[Count] = arrTemplateSizes[RS_FGP_RIGHT_LITTLE - 1];
                LeftThumbLen[Count] = arrTemplateSizes[RS_FGP_LEFT_THUMB - 1];
                LeftIndexLen[Count] = arrTemplateSizes[RS_FGP_LEFT_INDEX - 1];
                LeftMiddleLen[Count] = arrTemplateSizes[RS_FGP_LEFT_MIDDLE - 1];
                LeftRingLen[Count] = arrTemplateSizes[RS_FGP_LEFT_RING - 1];
                LeftLittleLen[Count] = arrTemplateSizes[RS_FGP_LEFT_LITTLE - 1];


                //Quality Length
                //RightThumbQuality[Count] = arrTemplateQuality[RS_FGP_RIGHT_THUMB - 1];
                //RightIndexQuality[Count] = arrTemplateQuality[RS_FGP_RIGHT_INDEX - 1];
                //RightMiddleQuality[Count] = arrTemplateQuality[RS_FGP_RIGHT_MIDDLE - 1];
                //RightRingQuality[Count] = arrTemplateQuality[RS_FGP_RIGHT_RING - 1];
                //RightLittleQuality[Count] = arrTemplateQuality[RS_FGP_RIGHT_LITTLE - 1];
                //LeftThumbQuality[Count] = arrTemplateQuality[RS_FGP_LEFT_THUMB - 1];
                //LeftIndexQuality[Count] = arrTemplateQuality[RS_FGP_LEFT_INDEX - 1];
                //LeftMiddleQuality[Count] = arrTemplateQuality[RS_FGP_LEFT_MIDDLE - 1];
                //LeftRingQuality[Count] = arrTemplateQuality[RS_FGP_LEFT_RING - 1];
                //LeftLittleQuality[Count] = arrTemplateQuality[RS_FGP_LEFT_LITTLE - 1];
                Count++;

            }
            finally
            {
                cacheLock.ExitWriteLock();
            }



        }
        public void SetCapacity( int InitialCapacity)
        {


            arrApplicantIDs = new int[InitialCapacity] ;

          
            //Templates
            RightThumb = new byte[InitialCapacity][];
            RightIndex = new byte[InitialCapacity][];
            RightMiddle = new byte[InitialCapacity][];
            RightRing = new byte[InitialCapacity][];
            RightLittle = new byte[InitialCapacity][];

            LeftThumb = new byte[InitialCapacity][];
            LeftIndex = new byte[InitialCapacity][];
            LeftMiddle = new byte[InitialCapacity][];
            LeftRing = new byte[InitialCapacity][];
            LeftLittle = new byte[InitialCapacity][];

            RightThumb[0] = new byte[0];
            RightIndex[0] = new byte[0];
            RightMiddle[0] = new byte[0];
            RightRing[0] = new byte[0];
            RightLittle[0] = new byte[0];

            LeftThumb[0] = new byte[0];
            LeftIndex[0] = new byte[0];
            LeftMiddle[0] = new byte[0];
            LeftRing[0] = new byte[0];
            LeftLittle[0] = new byte[0];

            
            //Templates Sizes

            RightThumbLen = new int[InitialCapacity];
            RightIndexLen = new int[InitialCapacity];
            RightMiddleLen = new int[InitialCapacity];
            RightRingLen = new int[InitialCapacity];
            RightLittleLen = new int[InitialCapacity];

            LeftThumbLen = new int[InitialCapacity];
            LeftIndexLen = new int[InitialCapacity];
            LeftMiddleLen = new int[InitialCapacity];
            LeftRingLen = new int[InitialCapacity];
            LeftLittleLen = new int[InitialCapacity];

            //Templates Quality

            //RightThumbQuality = new int[InitialCapacity];
            //RightIndexQuality = new int[InitialCapacity];
            //RightMiddleQuality = new int[InitialCapacity];
            //RightRingQuality = new int[InitialCapacity];
            //RightLittleQuality = new int[InitialCapacity];

            //LeftThumbQuality = new int[InitialCapacity];
            //LeftIndexQuality = new int[InitialCapacity];
            //LeftMiddleQuality = new int[InitialCapacity];
            //LeftRingQuality = new int[InitialCapacity];
            //LeftLittleQuality = new int[InitialCapacity];

        
        }

    }
}
