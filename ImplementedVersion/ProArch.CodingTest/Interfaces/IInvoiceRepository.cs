using ProArch.CodingTest.Invoices;
using System.Collections.Generic;

namespace ProArch.CodingTest.Interfaces
{
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> Get(int supplierId);
    }
}
