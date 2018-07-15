using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.Interfaces
{
    public interface IExternalInvoiceServiceManager
    {
        Supplier supplier { get; set; }
        SpendSummary spendSummary { get; set; }
        event EventHandler<ServiceManagerArgs> EventExternalInvoiceServiceFailed;
        event EventHandler<ServiceManagerArgs> EventDataNotRefreshed;
        event EventHandler<ServiceManagerArgs> EventSuccess;
        void TryGetSpendSummaryFromExternalService();
        void TryGetSpendSummaryFromFailoverService();
    }
}
