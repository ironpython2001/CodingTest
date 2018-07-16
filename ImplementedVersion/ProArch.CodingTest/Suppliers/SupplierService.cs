using ProArch.CodingTest.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Unity.Attributes;

namespace ProArch.CodingTest.Suppliers
{
    public class SupplierService:ISupplierService
    {
        [Dependency]
        public IEnumerable<Supplier> Suppliers { get; set; }

        public Supplier GetById(int id)
        {
            var supplier = Suppliers.Where(x => x.Id == id).Select(x => x).First();
            return supplier;
        }
    }
}
