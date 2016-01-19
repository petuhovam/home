using System;
using VendingMachine.Domain.Models;

namespace VendingMachine.UI.WPF.Models
{
    public class ProductCount
    {
        public Product Product
        {
            get;
            set;
        }

        public UInt16 Count
        {
            get;
            set;
        }

        public override String ToString()
        {
            return String.Format("{0} - {1} порций", Product, Count);
        }
    }
}
