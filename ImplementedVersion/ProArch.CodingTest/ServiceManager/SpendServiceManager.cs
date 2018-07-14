using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.Summary;

namespace ProArch.CodingTest.ServiceManager
{
    internal class SpendServiceManager
    {
        private int _supplierId;

        public SpendServiceManager(int suppierId)
        {
            this._supplierId = suppierId;
        }

        public SpendSummary GetTotalSpend()
        {
            //Get Supplier (TODO : need to change with dependency injection)
            var supplierService = new ProArch.CodingTest.Suppliers.SupplierService();
            var supplier = supplierService.GetById(this._supplierId);

            var spendSummary = new SpendSummary();
            spendSummary.Name = supplier.Name;
            var spendDetails = new List<SpendDetail>();

            //need to implement repository pattern or plug and play pattern to load dynamically
            if (!supplier.IsExternal) //Internal Repository
            {
                //need to change with dependency injection
                var invoiceRepository = new Invoices.InvoiceRepository();
                var invoices = invoiceRepository.Get(supplier.Id);
                //sum with group by year
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
            }
            else //External Service
            {
                //need to change with dependency injection and repository pattern
                var externalInvoices = ProArch.CodingTest.External.ExternalInvoiceService.GetInvoices(supplier.Id.ToString());
                //sum with group by year
                var result = externalInvoices.GroupBy(x => x.Year)
                                .Select(g => new
                                {
                                    Year = g.Key,
                                    TotalSpend = g.Sum(x => x.TotalAmount)
                                }).ToList();

                result.ForEach(x => spendSummary.Years.Add(
                    new SpendDetail()
                    {
                        Year = x.Year,
                        TotalSpend = x.TotalSpend
                    }));


            }



            return spendSummary;
        }



    }
}
