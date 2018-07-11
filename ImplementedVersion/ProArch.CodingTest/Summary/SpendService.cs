using System.Linq;
using System.Collections.Generic;

namespace ProArch.CodingTest.Summary
{
    public class SpendService
    {
        public SpendSummary GetTotalSpend(int supplierId)
        {
            //Get Supplier (need to change with dependency injection)
            var supplierService = new ProArch.CodingTest.Suppliers.SupplierService();
            var supplier = supplierService.GetById(supplierId);

            var spendSummary = new SpendSummary();
            spendSummary.Name = supplier.Name;
            var spendDetails = new List<SpendDetail>();
            
            if(!supplier.IsExternal) //Internal Repository
            {
                //need to change with dependency injection
                var invoiceRepository = new Invoices.InvoiceRepository();
                var invoices= invoiceRepository.Get(supplier.Id);
                //invoices.Sum()
            }
            else //External Service
            {

            }
            


            return spendSummary;
        }
    }
}
