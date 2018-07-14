using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;
using Unity.Attributes;
using Unity;
using ProArch.CodingTest.Suppliers;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Summary;

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
