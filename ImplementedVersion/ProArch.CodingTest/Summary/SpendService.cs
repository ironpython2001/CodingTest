using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;
using Unity.Attributes;
using Unity;
using ProArch.CodingTest.Suppliers;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Extensions;

namespace ProArch.CodingTest.Summary
{
    public class SpendService:ISpendService
    {
        [Dependency]
        public ISupplierService supplierService
        {
            get;
            set;
        }

        [Dependency]
        public IInvoiceRepository invoiceRepository
        {
            get; set;
        }

        private SpendSummary spendSummary;

        public SpendService()
        {
            
        }

        public SpendSummary GetTotalSpend(int supplierId)
        {
            var supplier = this.supplierService.GetById(supplierId);

            if (!supplier.IsExternal) //Internal Repository
            {
                this.spendSummary= supplier.SpendSummary(this.invoiceRepository);
            }
            else //External Service
            {
                this.spendSummary = this.GetSpendSummaryFromExternalService(supplier);
            }
            return this.spendSummary;
        }

        

        private SpendSummary GetSpendSummaryFromExternalService(Supplier supplier)
        {
            var spendSummary = new SpendSummary() { Years = new List<SpendDetail>() };
            spendSummary.Name = supplier.Name;
            var spendDetails = new List<SpendDetail>();

            ExternalInvoice[] invoices=null;
            
            try
            {
                invoices = ExternalInvoiceService.GetInvoices(supplier.Id.ToString());
            }
            catch // external service failed
            {
                invoices = null;
            }

            var result = invoices.GroupBy(x => x.Year)
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
           
            return spendSummary;
        }


    }
}
