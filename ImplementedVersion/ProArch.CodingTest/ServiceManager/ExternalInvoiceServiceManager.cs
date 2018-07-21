using ProArch.CodingTest.External;
using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProArch.CodingTest.ServiceManager
{
    public class ExternalInvoiceServiceManager:IExternalInvoiceServiceManager
    {

        public Supplier supplier { get; set; }

        private SpendSummary _spendSummary = new SpendSummary() { Years = new List<SpendDetail>() };
        public SpendSummary spendSummary
        {
            get { return _spendSummary; }
            set { _spendSummary = value; }
        }

        public event EventHandler<ServiceManagerArgs> EventExternalInvoiceServiceFailed;
        public event EventHandler<ServiceManagerArgs> EventDataNotRefreshed;
        

        public ExternalInvoiceServiceManager()
        {
            
        }

        public SpendSummary TryGetSpendSummaryFromExternalService(Supplier theSupplier,Func<string,ExternalInvoice[]> externalInvoiceService )
        {
            var theSpendSummary = new SpendSummary() { Years = new List<SpendDetail>() };
            ExternalInvoice[] invoices = null;
            try
            {
                invoices = externalInvoiceService.Invoke(theSupplier.Id.ToString()); //ExternalInvoiceService.GetInvoices(theSupplier.Id.ToString());
            }
            catch // external service failed
            {
                invoices = null;
            }
            if (invoices == null)
            {
                EventExternalInvoiceServiceFailed(this, new ServiceManagerArgs() { supplier = theSupplier });
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

                result.ForEach(x => theSpendSummary.Years.Add(
                    new SpendDetail()
                    {
                        Year = x.Year,
                        TotalSpend = x.TotalSpend
                    }));
                #endregion
            }
            return theSpendSummary;
        }

        public SpendSummary TryGetSpendSummaryFromFailoverService(Supplier theSupplier,FailoverInvoiceCollection failOverInvoices)
        {
            var theSpendSummary = new SpendSummary() { Years = new List<SpendDetail>() };
            TimeSpan diff = DateTime.Today - failOverInvoices.Timestamp;
            if (diff.Days > 30)
            {
                EventDataNotRefreshed(this, new ServiceManagerArgs() { supplier = theSupplier });
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
            }

            return theSpendSummary;
        }

        
    }
}
