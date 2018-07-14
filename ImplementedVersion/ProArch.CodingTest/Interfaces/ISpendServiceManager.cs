using ProArch.CodingTest.Summary;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.Interfaces
{
    public interface ISpendServiceManager
    {
        SpendSummary GetTotalSpend(int suppierId);
    }
}
