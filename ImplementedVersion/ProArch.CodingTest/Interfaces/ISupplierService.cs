using ProArch.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProArch.CodingTest.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> Suppliers { get; set; }
        Supplier GetById(int id);
    }
}
