﻿using LM.Domain.Aggregates.Book;
using LM.Domain.DomainEvents;
using LM.Domain.Enums;
using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Loan
{
    public class Loan : Entity, IAggregateRoot
    {
        public Guid BookItemId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime LoanDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public Money PaidAmount { get; private set; }

        public Loan(Guid bookItemId, Guid userId, DateTime? dueDate = null) : base()
        {
            BookItemId = bookItemId;
            UserId = userId;
            LoanDate = DateTime.UtcNow;
            DueDate = dueDate ?? LoanDate.AddDays(14);
            ReturnDate = null;
            PaidAmount = new Money(0, Currency.RON);
            AddDomainEvent(new BookLoanedEvent(bookItemId));
        }

        public void ReturnBook(BookQualityState qualityState, DateTime returnDate)
        {
            ReturnDate = returnDate;
            AddDomainEvent(new BookReturnedEvent(BookItemId, qualityState, returnDate));
        }

        public decimal CalculatePenalty(Money rentPrice, int differenceQualityState)
        {
            decimal totalAmount = 0;
            if (differenceQualityState > 2)
            {
                //If the user return the book with another state that he previous loaned, he will pay the damage
                totalAmount = differenceQualityState * rentPrice.Amount * 0.2m; // 20% of rent price per difference quality state
            }

            var overdueDays = (DateTime.UtcNow - DueDate).Days;
            if (overdueDays > 0) totalAmount += overdueDays * (rentPrice.Amount * 0.01m); // 1% of rent price per day
            PaidAmount = new Money(totalAmount, rentPrice.Currency);
            return totalAmount;
        }
    }
}
