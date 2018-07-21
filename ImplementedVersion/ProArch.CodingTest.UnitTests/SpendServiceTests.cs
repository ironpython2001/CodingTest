using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ProArch.CodingTest.External;
using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity;

namespace ProArch.CodingTest.UnitTests
{
    [TestClass]
    public class SpendServiceTests
    {
        private SpendService _spendService;
        private SupplierService _supplierService;
        private InvoiceRepository _invoiceRepository;

        public SpendServiceTests()
        {
            var container = new UnityContainer();
            container.RegisterType<IInvoiceRepository, InvoiceRepository>();
            container.RegisterType<ISupplierService, SupplierService>();

            this._spendService = container.Resolve<SpendService>();
            this._supplierService = container.Resolve<SupplierService>();
            this._invoiceRepository = container.Resolve<InvoiceRepository>();

            var jsonSuppliersData = File.ReadAllText("SuppliersData.json");
            this._supplierService.Suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonSuppliersData);

            var jsonInvoicesData = File.ReadAllText("InvoicesData.json");
            this._invoiceRepository.Invoices = JsonConvert.DeserializeObject<List<Invoice>>(jsonInvoicesData);

            this._spendService.supplierService = this._supplierService;
            this._spendService.invoiceRepository = this._invoiceRepository;
        }

        [TestMethod]
        [Description("Test For Internal Supplier")]
        public void InternalSupplierTestForSupplier1()
        {
            var supplier = this._supplierService.Suppliers.Where(x => x.Id == 1).First();
            var spendSummary = this._spendService.GetTotalSpend(supplier.Id);
            Assert.IsNotNull(spendSummary);
            Assert.IsNotNull(spendSummary.Years);
            Assert.IsTrue(spendSummary.Years.Count > 0);
            spendSummary.Years.ForEach(x => Assert.IsTrue(x.TotalSpend == 200));
        }

        [TestMethod]
        [Description("External Service Is Able To Successfully Get Data.")]
        public void ExternalSupplierTestForSupplier2()
        {
            var supplier = this._supplierService.Suppliers.Where(x => x.Id == 2).First();
            IExternalInvoiceServiceManager mgr = new ExternalInvoiceServiceManager();
            var result = mgr.TryGetSpendSummaryFromExternalService(supplier, ExternalInvoiceService.GetInvoices);
            Assert.IsTrue(result!=null);
        }

        [TestMethod]
        [Description("Test To Identify, External Service Is Failed To Get Data.")]
        public void ExternalSupplierTestForSupplier3()
        {
            var supplier = this._supplierService.Suppliers.Where(x => x.Id == 2).First();
            IExternalInvoiceServiceManager mgr = new ExternalInvoiceServiceManager();
            mgr.EventExternalInvoiceServiceFailed += delegate (object sender, ServiceManagerArgs e)
            {
                Assert.IsTrue(e.supplier.Id== 2);
            };
            var result = mgr.TryGetSpendSummaryFromExternalService(supplier, null);
        }


        [TestMethod]
        [Description("Test To Identify that we are able to get data from failoverservice")]
        public void ExternalSupplierTestForSupplier4()
        {
            var supplier = this._supplierService.Suppliers.Where(x => x.Id == 2).First();
            IExternalInvoiceServiceManager mgr = new ExternalInvoiceServiceManager();
            var failoverInvoices= new FailoverInvoiceCollection();
            failoverInvoices.Invoices = new ExternalInvoice[] 
            {
                new ExternalInvoice(){ TotalAmount=100,Year=2018 }
                ,new ExternalInvoice(){ TotalAmount=100,Year=2018 }
            };
            failoverInvoices.Timestamp = DateTime.Now;
            var result = mgr.TryGetSpendSummaryFromFailoverService(supplier, failoverInvoices);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        [Description("Test To Identify that we are Not able to get data from failoverservice. The data is older then 30 days")]
        public void ExternalSupplierTestForSupplier5()
        {
            var supplier = this._supplierService.Suppliers.Where(x => x.Id == 2).First();
            IExternalInvoiceServiceManager mgr = new ExternalInvoiceServiceManager();
            var failoverInvoices = new FailoverInvoiceCollection();
            failoverInvoices.Invoices = new ExternalInvoice[]
            {
                new ExternalInvoice(){ TotalAmount=100,Year=2018 }
                ,new ExternalInvoice(){ TotalAmount=100,Year=2018 }
            };
            failoverInvoices.Timestamp = DateTime.Now.AddMonths(-2);//Just to simulate the data is 2 months old
            mgr.EventDataNotRefreshed += delegate (object sender, ServiceManagerArgs e)
            {
                Assert.IsTrue(e.supplier.Id == 2);
            };
            var result = mgr.TryGetSpendSummaryFromFailoverService(supplier, failoverInvoices);
            Assert.IsTrue(result != null);
        }

    }

    
}
