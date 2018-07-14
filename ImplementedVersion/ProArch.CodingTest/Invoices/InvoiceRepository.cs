using System.Collections.Generic;
using System.Linq;
using ProArch.CodingTest.Interfaces;
using Unity.Attributes;

namespace ProArch.CodingTest.Invoices
{
    public class InvoiceRepository:IInvoiceRepository
    {
        [Dependency]
        public IEnumerable<Invoice> Invoices
        {
            get;
            set;
        }

        public IEnumerable<Invoice> Get(int supplierId)
        {
            var invoices = Invoices.Where(x => x.SupplierId == supplierId).Select(x => x).AsEnumerable();
            return invoices;
        }
    }
}
