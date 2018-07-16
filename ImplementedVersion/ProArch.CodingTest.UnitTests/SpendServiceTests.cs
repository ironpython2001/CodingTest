using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.Invoices;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Summary;
using ProArch.CodingTest.Suppliers;
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
        [Description("Internal Supplier Test For Supplier1")]
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
        [Description("External Supplier Test For Supplier2")]
        public void ExternalSupplierTestForSupplier2()
        {
            var supplier = this._supplierService.Suppliers.Where(x => x.Id == 2).First();
            IExternalInvoiceServiceManager mgr = new ExternalInvoiceServiceManager();
            var result = mgr.TryGetSpendSummaryFromExternalService(supplier);
            Assert.IsTrue(result!=null);
        }

    }
}
