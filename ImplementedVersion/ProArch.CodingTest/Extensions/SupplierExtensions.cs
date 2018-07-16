using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
using System.Collections.Generic;
using System.Linq;

namespace ProArch.CodingTest.Extensions
{
    public static class SupplierExtensions
    {
        public static SpendSummary SpendSummary(this Supplier supplier,IInvoiceRepository invoiceRepository)
        {
            var spendSummary = new SpendSummary() { Years = new List<SpendDetail>() };
            spendSummary.Name = supplier.Name;
            var spendDetails = new List<SpendDetail>();

            var invoices = invoiceRepository.Get(supplier.Id);

            var result = invoices.GroupBy(x => x.InvoiceDate.Year)
                            .Select(g => new
                            {
                                Year = g.Key,
                                TotalSpend = g.Sum(x => x.Amount)
                            }).ToList();

            result.ForEach(x => spendSummary.Years.Add(
                new SpendDetail()
                {
                    Year = x.Year,
                    TotalSpend = x.TotalSpend
                }));
            return spendSummary;
        }
    }
}
