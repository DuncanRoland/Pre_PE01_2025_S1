namespace Pre.SalesPerStore.Core.Entities;

public class Store
{
    public string StoreName { get; set; }
    public string StoreCountry { get; set; }
    public DateTime EstablishedDate { get; set; }
    public List<Product> Products { get; set; }
}