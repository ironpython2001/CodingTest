using System.Collections.Generic;
using System.Linq;
using ProArch.CodingTest.Interfaces;

namespace ProArch.CodingTest.Invoices
{
    public class InvoiceRepository:IInvoiceRepository
    {
        public IQueryable<Invoice> Get(int supplierId)
        {
            return new List<Invoice>().AsQueryable();
        }
    }
}
