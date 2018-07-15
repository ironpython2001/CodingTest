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
        public event EventHandler<ServiceManagerArgs> EventSuccess;

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
                EventExternalInvoiceServiceFailed(this, new ServiceManagerArgs() { supplier=this.supplier});
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
                EventSuccess(this,  new ServiceManagerArgs() { supplier = this.supplier });
            }
        }

        public void TryGetSpendSummaryFromFailoverService()
        {
            var failOverService = new FailoverInvoiceService();
            var failOverInvoices = failOverService.GetInvoices(supplier.Id);

            TimeSpan diff = DateTime.Today - failOverInvoices.Timestamp;
            if (diff.Days > 30)
            {
                EventDataNotRefreshed(this, new ServiceManagerArgs() { supplier = this.supplier });
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
                EventSuccess(this, new ServiceManagerArgs() { supplier = this.supplier });
            }
        }

       
    }
}
