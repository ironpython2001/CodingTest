using ProArch.CodingTest.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProArch.CodingTest.Interfaces
{
    public interface IInvoiceRepository
    {
        IQueryable<Invoice> Get(int supplierId);
    }
}
