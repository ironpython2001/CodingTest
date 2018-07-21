using ProArch.CodingTest.External;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
using System;

namespace ProArch.CodingTest.Interfaces
{
    public interface IExternalInvoiceServiceManager
    {
        Supplier supplier { get; set; }
        SpendSummary spendSummary { get; set; }
        event EventHandler<ServiceManagerArgs> EventExternalInvoiceServiceFailed;
        event EventHandler<ServiceManagerArgs> EventDataNotRefreshed;
        SpendSummary TryGetSpendSummaryFromExternalService(Supplier theSupplier, Func<string, ExternalInvoice[]> externalInvoiceService);
        SpendSummary TryGetSpendSummaryFromFailoverService(Supplier supplier, FailoverInvoiceCollection failOverInvoices);
    }
}
