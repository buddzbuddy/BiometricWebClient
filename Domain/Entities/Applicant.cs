using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Applicant
    {
        public int PensionCardID { get; set; }
        public int  ApplicantId { get; set; }
        public string PID { get; set; }
        //public string  SerialNumber { get; set; }
        public string  BeneficiaryName { get; set; }
        //public string  RetirementType { get; set; }
        public string  CardHolderName { get; set; }
        public string  CardHolderSurname { get; set; }
        public string  CardHolderFatherName { get; set; }
        public string  CardHolderTazkaraID { get; set; }
        public string  PensionCardImage { get; set; }
        public string  NameOfAccount { get; set; }
        //public string  FirstBenefitPeriod { get; set; }
        //public string  BankName { get; set; }
        //public string  BankBranchName { get; set; }
        //public string  AccountNumber { get; set; }
        //public DateTime  CardIssueDate { get; set; }
        //public DateTime CardExpiryDate { get; set; }
        //public string  Status { get; set; }
        //public string  PensionCategory { get; set; }
    }
}
