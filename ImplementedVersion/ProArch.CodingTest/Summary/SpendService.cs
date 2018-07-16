using ProArch.CodingTest.Extensions;
using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Suppliers;
using System;
using System.Threading;
using Unity.Attributes;

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
        private ExternalInvoiceServiceManager externalInvoiceServiceManager;
        Timer tm = null;
        AutoResetEvent _autoEvent = null;
        private static int consecutiveErrors=0;

        public SpendService()
        {
            this.externalInvoiceServiceManager = new ExternalInvoiceServiceManager();
            this.externalInvoiceServiceManager.EventExternalInvoiceServiceFailed += ExternalInvoiceServiceManager_EventExternalInvoiceServiceFailed;
            this.externalInvoiceServiceManager.EventDataNotRefreshed += ExternalInvoiceServiceManager_EventDataNotRefreshed;
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
                this.externalInvoiceServiceManager.supplier = supplier;
                this.spendSummary= this.externalInvoiceServiceManager.TryGetSpendSummaryFromExternalService(supplier);
            }
            return this.spendSummary;
        }

        private void ExternalInvoiceServiceManager_EventExternalInvoiceServiceFailed(object sender, System.EventArgs e)
        {
            var serviceManagerArgs = e as ServiceManagerArgs;
            consecutiveErrors = consecutiveErrors + 1;
            if(consecutiveErrors>3)
            {
                this.spendSummary = this.externalInvoiceServiceManager.TryGetSpendSummaryFromFailoverService(serviceManagerArgs.supplier);
            }
            else
            {
                this.spendSummary = this.externalInvoiceServiceManager.TryGetSpendSummaryFromExternalService(serviceManagerArgs.supplier);
            }
        }

        private void ExternalInvoiceServiceManager_EventDataNotRefreshed(object sender, System.EventArgs e)
        {
            var serviceManagerArgs = e as ServiceManagerArgs;
            consecutiveErrors = consecutiveErrors + 1;
            if(consecutiveErrors>6)
            {
                _autoEvent = new AutoResetEvent(false);
                tm = new Timer(Execute, _autoEvent, 1000, 1000);
            }
            else
            {
                this.spendSummary= this.externalInvoiceServiceManager.TryGetSpendSummaryFromFailoverService(serviceManagerArgs.supplier);
            }
        }

        public void Execute(Object stateInfo)
        {
            this.spendSummary = this.externalInvoiceServiceManager.TryGetSpendSummaryFromExternalService(stateInfo as Supplier);
           tm.Dispose();
        }


        private void ExternalInvoiceServiceManager_EventSuccess(object sender, EventArgs e)
        {
            this.spendSummary= this.externalInvoiceServiceManager.spendSummary;
        }

    }
}
