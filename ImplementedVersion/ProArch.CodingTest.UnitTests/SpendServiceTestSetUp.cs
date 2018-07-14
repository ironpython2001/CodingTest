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
        public List<Supplier> SuppliersData { get; set; }
        
        public SpendServiceTestSetUp(ISpendService spendService)
        {
       
            this._spendService = spendService;


            var testData = File.ReadAllText("TestData.json");
            this.SuppliersData = JsonConvert.DeserializeObject<List<Supplier>>(testData);

        }

        public SpendSummary GetInternalSupplierSpendSummary(int supplierId)
        {
            return this._spendService.GetTotalSpend(supplierId);
        }

    }
}
