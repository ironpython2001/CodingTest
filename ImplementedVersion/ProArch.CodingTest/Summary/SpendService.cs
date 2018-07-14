using System.Linq;
using System.Collections.Generic;
using ProArch.CodingTest.ServiceManager;
using ProArch.CodingTest.Interfaces;

namespace ProArch.CodingTest.Summary
{
    public class SpendService:ISpendService
    {
        private ISpendServiceManager _spendServiceManager;
        public SpendService(ISpendServiceManager spendServiceManager)
        {
            this._spendServiceManager = spendServiceManager;
        }

        public SpendSummary GetTotalSpend(int supplierId)
        {
            var spendSummary = this._spendServiceManager.GetTotalSpend(supplierId);
            return spendSummary;
        }
    }
}
