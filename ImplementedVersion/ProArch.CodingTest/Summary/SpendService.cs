using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;
using Unity.Attributes;
using Unity;
using ProArch.CodingTest.Suppliers;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Extensions;
using System;
using System.Threading;

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
            this.externalInvoiceServiceManager.EventSuccess += ExternalInvoiceServiceManager_EventSuccess;
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
                this.externalInvoiceServiceManager.TryGetSpendSummaryFromExternalService();
            }
            return this.spendSummary;
        }

        private void ExternalInvoiceServiceManager_EventExternalInvoiceServiceFailed(object sender, System.EventArgs e)
        {
            consecutiveErrors = consecutiveErrors + 1;
            if(consecutiveErrors>3)
            {
                this.externalInvoiceServiceManager.TryGetSpendSummaryFromFailoverService();
            }
            else
            {
                this.externalInvoiceServiceManager.TryGetSpendSummaryFromExternalService();
            }
        }

        private void ExternalInvoiceServiceManager_EventDataNotRefreshed(object sender, System.EventArgs e)
        {
            consecutiveErrors = consecutiveErrors + 1;
            if(consecutiveErrors>6)
            {
                _autoEvent = new AutoResetEvent(false);
                tm = new Timer(Execute, _autoEvent, 1000, 1000);


            }
            else
            {
                this.externalInvoiceServiceManager.TryGetSpendSummaryFromFailoverService();
            }
        }

        public void Execute(Object stateInfo)
        {
           this.externalInvoiceServiceManager.TryGetSpendSummaryFromExternalService();
           tm.Dispose();
        }


        private void ExternalInvoiceServiceManager_EventSuccess(object sender, EventArgs e)
        {
            this.spendSummary= this.externalInvoiceServiceManager.spendSummary;
        }

    }
}
