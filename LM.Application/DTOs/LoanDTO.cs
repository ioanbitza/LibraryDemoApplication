﻿using LM.Domain.Aggregates.Loan;

namespace LM.Application.DTOs
{
    public class LoanDTO
    {
        public LoanDTO(Loan loan)
        {
            Id = loan.Id;
            LoanDate = loan.LoanDate;
            DueDate = loan.DueDate;
            ReturnDate = loan.ReturnDate;
        }
        public Guid Id { get; }
        public DateTime LoanDate { get; }
        public DateTime DueDate { get; }
        public DateTime? ReturnDate { get; }
    }
}
