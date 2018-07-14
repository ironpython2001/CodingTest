using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProArch.CodingTest.Suppliers;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using ProArch.CodingTest.Interfaces;
using ProArch.CodingTest.Summary;
using Unity;

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
            this._setUp = container.Resolve<SpendServiceTestSetUp>();
        }

        [TestMethod]
        [Description("Internal Supplier Test")]
        public void InternalSupplierTest()
        {
            //from our test data we know that first supplier is internal 
            var supplier = this._setUp.SuppliersData[0];
            this._setUp.GetInternalSupplierSpendSummary(supplier.Id);
        }
    }
}
