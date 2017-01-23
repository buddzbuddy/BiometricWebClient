using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EnrollementResponse
    {
         [Flags]
        public enum enumType : ulong
        {
            FingerprintAlreadyEnrolled                  = 1 << 1,
            FingerprintSuccesfullyEnrolled              = 1 << 2,
            ApplicantByFingerprintDeletedFromMOLSAMD    = 1 << 3,
            
            IdentificationFingerprintNotEnrolled        = 1 << 4,
            IdentificationFingerprintAlreadyEnrolled    = 1 << 5,
            
            ApplicantByIDExist                          = 1 << 6,
            ApplicantByIDNotExists                      = 1 << 7,
            ApplicantAlreadyEnrolled                    = 1 << 8,
        }
        public enumType Type;
        public ErrorResponse error = new ErrorResponse();
        public Applicant applicantEntered = new Applicant();
        public Applicant applicantFingerPrint = new Applicant();

    }
}
