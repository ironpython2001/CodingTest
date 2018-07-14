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

namespace ProArch.CodingTest.UnitTests
{
    [TestClass]
    public class SpendServiceTests
    {
        private SpendServiceTestSetUp _setUp;
        public SpendServiceTests()
        {
            var container = new UnityContainer();
            container.RegisterType<ISpendService, SpendService>();
            container.RegisterType<ISpendServiceManager, SpendServiceManager>();
            container.RegisterType<ISupplierService, SupplierService>();
            this._setUp = container.Resolve<SpendServiceTestSetUp>();
        }

        [TestMethod]
        [Description("Internal Supplier Test")]
        public void InternalSupplierTest()
        {
            //from our test data we know that first supplier is internal 
            var supplier = this._setUp._supplierService.Suppliers.First();
            this._setUp.GetInternalSupplierSpendSummary(supplier.Id);
        }
    }
}
