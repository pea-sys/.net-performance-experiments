namespace ch6.Listing1
{
    public class PriceEngine
    {
        public decimal CalculateDiscount(params Product[] product)
        {
            decimal discount = product.Length * 0.01m;
            return Math.Min(discount, 0.2m);
        }
    }
}
