using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;

namespace ProArch.CodingTest.Summary
{
    public class SpendService:ISpendService
    {
        public SpendSummary GetTotalSpend(int supplierId)
        {
            var manager = new SpendServiceManager(supplierId);
            var spendSummary = manager.GetTotalSpend();
            return spendSummary;
        }
    }
}
