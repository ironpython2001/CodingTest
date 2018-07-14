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
    public class SpendServiceTestSetUp
    {
        private ISpendService _spendService;
        public ISupplierService _supplierService;
        
        public SpendServiceTestSetUp(ISupplierService supplierService, ISpendService spendService)
        {
            this._spendService = spendService;
            this._supplierService = supplierService;
            var jsonSuppliersData = File.ReadAllText("SuppliersData.json");
            this._supplierService.Suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonSuppliersData);

        }

        public SpendSummary GetInternalSupplierSpendSummary(int supplierId)
        {
            return this._spendService.GetTotalSpend(supplierId);
        }

    }
}
