using ProArch.CodingTest.Summary;

namespace ProArch.CodingTest.Interfaces
{
    public interface ISpendService
    {
        SpendSummary GetTotalSpend(int supplierId);
    }
}
