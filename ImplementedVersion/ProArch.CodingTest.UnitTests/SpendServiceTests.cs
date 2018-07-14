using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProArch.CodingTest.Suppliers;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.Summary;
using Unity;
using ProArch.CodingTest.ServiceManager;
using System.Linq;
using ProArch.CodingTest.Invoices;

namespace ProArch.CodingTest.UnitTests
{
    [TestClass]
    public class SpendServiceTests
    {
        private SpendService _spendService;
        private SupplierService _supplierService;

        public SpendServiceTests()
        {
            var container = new UnityContainer();
            container.RegisterType<IInvoiceRepository, InvoiceRepository>();
            container.RegisterType<ISupplierService, SupplierService>();
            this._spendService = container.Resolve<SpendService>();
            this._supplierService = container.Resolve<SupplierService>();


            var jsonSuppliersData = File.ReadAllText("SuppliersData.json");
            this._supplierService.Suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonSuppliersData);
            this._spendService.SupplierService = this._supplierService;
        }

        [TestMethod]
        [Description("Internal Supplier Test")]
        public void InternalSupplierTest()
        {
            //from our test data we know that first supplier is internal 
            var supplier = this._supplierService.Suppliers.First();
            this._spendService.GetTotalSpend(supplier.Id);
        }
    }
}
