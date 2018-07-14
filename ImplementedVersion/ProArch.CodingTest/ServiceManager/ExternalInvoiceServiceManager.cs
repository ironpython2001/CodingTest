using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;
using Unity.Attributes;
using Unity;
using ProArch.CodingTest.Suppliers;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Extensions;
using ProArch.CodingTest.Summary;
using System;
using ProArch.CodingTest.Invoices;

namespace ProArch.CodingTest.ServiceManager
{
    public class ExternalInvoiceServiceManager
    {

        private Supplier supplier;
        public SpendSummary spendSummary = new SpendSummary() { Years = new List<SpendDetail>() };

        public event EventHandler EventExternalInvoiceServiceFailed;
        public event EventHandler EventDataNotRefreshed;
        public event EventHandler EventSuccess;

        public ExternalInvoiceServiceManager()
        {
            
        }

        public void TryGetSpendSummaryFromExternalService()
        {
            ExternalInvoice[] invoices = null;

            try
            {
                invoices = ExternalInvoiceService.GetInvoices(supplier.Id.ToString());
            }
            catch // external service failed
            {
                invoices = null;
            }
            if(invoices==null)
            {
                EventExternalInvoiceServiceFailed(this, null);
            }
            else
            {
                #region Calculate TotalSpend
                var result = invoices.GroupBy(x => x.Year)
                                       .Select(g => new
                                       {
                                           Year = g.Key,
                                           TotalSpend = g.Sum(x => x.TotalAmount)
                                       }).ToList();

                result.ForEach(x => this.spendSummary.Years.Add(
                    new SpendDetail()
                    {
                        Year = x.Year,
                        TotalSpend = x.TotalSpend
                    }));
                #endregion
                EventSuccess(this, null);
            }
        }

        public void TryGetSpendSummaryFromFailoverService()
        {
            var failOverService = new FailoverInvoiceService();
            var failOverInvoices = failOverService.GetInvoices(supplier.Id);

            TimeSpan diff = DateTime.Today - failOverInvoices.Timestamp;
            if (diff.Days > 30)
            {
                EventDataNotRefreshed(this, null);
            }
            else
            {
                #region Calculate TotalSpend
                var result = failOverInvoices.Invoices.GroupBy(x => x.Year)
                                       .Select(g => new
                                       {
                                           Year = g.Key,
                                           TotalSpend = g.Sum(x => x.TotalAmount)
                                       }).ToList();

                result.ForEach(x => this.spendSummary.Years.Add(
                    new SpendDetail()
                    {
                        Year = x.Year,
                        TotalSpend = x.TotalSpend
                    }));
                #endregion
                EventSuccess(this, null);
            }
        }



    }
}
