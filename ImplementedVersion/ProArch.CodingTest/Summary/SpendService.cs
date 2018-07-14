using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;

namespace ProArch.CodingTest.Summary
{
    public class SpendService:ISpendService
    {

        public ISupplierService SupplierService { get; set; }
        IInvoiceRepository _invoiceRepository;

        public SpendService(IInvoiceRepository invoiceRepository)
        {
            this._invoiceRepository = invoiceRepository;
        }

        public SpendSummary GetTotalSpend(int supplierId)
        {
            var supplier = this.SupplierService.GetById(supplierId);

            var spendSummary = new SpendSummary();
            spendSummary.Name = supplier.Name;
            var spendDetails = new List<SpendDetail>();

            //need to implement repository pattern or plug and play pattern to load dynamically
            if (!supplier.IsExternal) //Internal Repository
            {
                var invoices = this._invoiceRepository.Get(supplier.Id);
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
