using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Logic
{
    public class DB
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

        public int Index = 0;//Should be thread safe


        //Convert this to type of int
        public List<string> arrApplicant;//i.e arrApplicant[i] = applicantID;  where i is the index of the array below

        //byte[][] 
        public List<byte[]> RightThumb;
        public List<byte[]> RightIndex;
        public List<byte[]> RightMiddle;
        public List<byte[]> RightRing;
        public List<byte[]> RightLittle;
        public List<byte[]> LeftThumb;
        public List<byte[]> LeftIndex;
        public List<byte[]> LeftMiddle;
        public List<byte[]> LeftRing;
        public List<byte[]> LeftLittle;

        public List<byte[]>[] arrFingers;

        //int[] 
        public List<int> RightThumbLen;
        public List<int> RightIndexLen;
        public List<int> RightMiddleLen;
        public List<int> RightRingLen;
        public List<int> RightLittleLen;
        public List<int> LeftThumbLen;
        public List<int> LeftIndexLen;
        public List<int> LeftMiddleLen;
        public List<int> LeftRingLen;
        public List<int> LeftLittleLen;

        public List<int>[] arrTemplateLength;




        public void Add(string ApplicantID, byte[][] arrTemplates, int[] arrTemplateSizes, int[] arrTemplateQuality)
        {
            //Save into the database

            //Needs thread safe handling
            if (arrApplicant.Contains(ApplicantID) == true)
            {
                return;
            }
            arrApplicant.Add(ApplicantID);

            //Templates
            RightThumb.Add(arrTemplates[RS_FGP_RIGHT_THUMB - 1]);
            RightIndex.Add(arrTemplates[RS_FGP_RIGHT_INDEX - 1]);
            RightMiddle.Add(arrTemplates[RS_FGP_RIGHT_MIDDLE - 1]);
            RightRing.Add(arrTemplates[RS_FGP_RIGHT_RING - 1]);
            RightLittle.Add(arrTemplates[RS_FGP_RIGHT_LITTLE - 1]);


            LeftThumb.Add(arrTemplates[RS_FGP_LEFT_THUMB - 1]);
            LeftIndex.Add(arrTemplates[RS_FGP_LEFT_INDEX - 1]);
            LeftMiddle.Add(arrTemplates[RS_FGP_LEFT_MIDDLE - 1]);
            LeftRing.Add(arrTemplates[RS_FGP_LEFT_RING - 1]);
            LeftLittle.Add(arrTemplates[RS_FGP_LEFT_LITTLE - 1]);

            //Template Length
            RightThumbLen.Add(arrTemplateSizes[RS_FGP_RIGHT_THUMB - 1]);
            RightIndexLen.Add(arrTemplateSizes[RS_FGP_RIGHT_INDEX - 1]);
            RightMiddleLen.Add(arrTemplateSizes[RS_FGP_RIGHT_MIDDLE - 1]);
            RightRingLen.Add(arrTemplateSizes[RS_FGP_RIGHT_RING - 1]);
            RightLittleLen.Add(arrTemplateSizes[RS_FGP_RIGHT_LITTLE - 1]);

            LeftThumbLen.Add(arrTemplateSizes[RS_FGP_LEFT_THUMB - 1]);
            LeftIndexLen.Add(arrTemplateSizes[RS_FGP_LEFT_INDEX - 1]);
            LeftMiddleLen.Add(arrTemplateSizes[RS_FGP_LEFT_MIDDLE - 1]);
            LeftRingLen.Add(arrTemplateSizes[RS_FGP_LEFT_RING - 1]);
            LeftLittleLen.Add(arrTemplateSizes[RS_FGP_LEFT_LITTLE - 1]);


            //Adds Quality
            //Index = LeftThumb.Count;//Interlocked.Increment(ref Index)

        }
        public void LoadDB(int InitialCapacity, SqlDataReader dataReader)
        {


            arrApplicant = new List<string>(InitialCapacity);


            //Templates
            RightThumb = new List<byte[]>(InitialCapacity);
            RightIndex = new List<byte[]>(InitialCapacity);
            RightMiddle = new List<byte[]>(InitialCapacity);
            RightRing = new List<byte[]>(InitialCapacity);
            RightLittle = new List<byte[]>(InitialCapacity);

            LeftThumb = new List<byte[]>(InitialCapacity);
            LeftIndex = new List<byte[]>(InitialCapacity);
            LeftMiddle = new List<byte[]>(InitialCapacity);
            LeftRing = new List<byte[]>(InitialCapacity);
            LeftLittle = new List<byte[]>(InitialCapacity);

            //Templates Sizes

            RightThumbLen = new List<int>(InitialCapacity);
            RightIndexLen = new List<int>(InitialCapacity);
            RightMiddleLen = new List<int>(InitialCapacity);
            RightRingLen = new List<int>(InitialCapacity);
            RightLittleLen = new List<int>(InitialCapacity);

            LeftThumbLen = new List<int>(InitialCapacity);
            LeftIndexLen = new List<int>(InitialCapacity);
            LeftMiddleLen = new List<int>(InitialCapacity);
            LeftRingLen = new List<int>(InitialCapacity);
            LeftLittleLen = new List<int>(InitialCapacity);

            if (dataReader != null)
            {
                int OrdinalRightThumb = dataReader.GetOrdinal("RightThumb");
                int OrdinalRightIndex = dataReader.GetOrdinal("RightIndex");
                int OrdinalRightMiddle = dataReader.GetOrdinal("RightMiddle");
                //...
                while (dataReader.Read())
                {
                    RightThumb[Index] = (byte[])dataReader[OrdinalRightThumb];
                    RightIndex[Index] = (byte[])dataReader[OrdinalRightIndex];
                    RightMiddle[Index] = (byte[])dataReader[OrdinalRightMiddle];
                    //...

                }
                Index++;
            }

            arrFingers = new List<byte[]>[] { 
            RightThumb,
            RightIndex,
            RightMiddle,
            RightRing,
            RightLittle,
            LeftThumb,
            LeftIndex,
            LeftMiddle,
            LeftRing,
            LeftLittle,
        };

            arrTemplateLength = new List<int>[]{
         RightThumbLen,
         RightIndexLen,
         RightMiddleLen,
         RightRingLen,
         RightLittleLen,
         LeftThumbLen,
         LeftIndexLen,
         LeftMiddleLen,
         LeftRingLen,
         LeftLittleLen
        };
        }

    }
}
