using ProArch.CodingTest.Suppliers;
using System.Collections.Generic;

namespace ProArch.CodingTest.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> Suppliers { get; set; }
        Supplier GetById(int id);
    }
}
