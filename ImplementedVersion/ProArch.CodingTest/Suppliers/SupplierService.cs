using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProArch.CodingTest.Interfaces;

namespace ProArch.CodingTest.Suppliers
{
    public class SupplierService:ISupplierService
    {
        public IEnumerable<Supplier> Suppliers { get; set; }

        public Supplier GetById(int id)
        {
            var supplier = Suppliers.Where(x => x.Id == id).Select(x => x).First();
            return supplier;
        }
    }
}
