namespace Pre.SalesPerStore.Core.Entities;

public class Store
{
    public string StoreName { get; set; }
    public string StoreCountry { get; set; }
    public DateTime EstablishedDate { get; set; }
    public List<Product> Products { get; set; }
    
    public Store(string storeName, string storeCountry, DateTime establishedDate)
    {
        StoreName = storeName;
        StoreCountry = storeCountry;
        EstablishedDate = establishedDate;
        Products = new List<Product>();
    }
}