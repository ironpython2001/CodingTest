using ProArch.CodingTest.Summary;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.Interfaces
{
    public interface ISpendService
    {
        SpendSummary GetTotalSpend(int supplierId);
    }
}
