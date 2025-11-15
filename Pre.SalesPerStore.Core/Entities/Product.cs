namespace Pre.SalesPerStore.Core.Entities;

public class Product
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal SellPrice { get; set; }
    public decimal BuyingPrice { get; set; }
}