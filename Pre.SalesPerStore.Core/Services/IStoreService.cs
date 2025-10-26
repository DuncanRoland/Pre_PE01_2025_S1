using System;
using Pre.SalesPerStore.Core.Entities;

namespace Pre.SalesPerStore.Core.Services;

public interface IStoreService
{
    // Haal alle stores op waar ze een bepaald product verkopen
    IEnumerable<string> GetStoresByProduct(string productName);

    // Haal alle unieke landen op waar er winkels zijn
    IEnumerable<string> GetAllCountries();

    // Haal alle unieke winkels op, gesorteerd van A naar Z.
    IEnumerable<string> GetAllStores();

    // Haal alle producten tussen een bepaalde verkoopsprijs uit de lijst
    IEnumerable<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);

    // Geef een lijst terug met de gemiddelde verkoopsprijs per store
    // Formaat anoniem object: StoreName, MeanPrice
    IEnumerable<(string StoreName, decimal MeanPrice)> GetAverageProductPricePerStore();

    // Haal alle producten van een bepaalde store op die bijna uitverkocht zijn
    // (= kleiner dan minNumberOfProducts)
    // Sorteer de producten van minst aantal naar meest
    IEnumerable<Product> GetSalesByStore(string storeName, int minNumberOfProducts);

    // Controleer of een store een bepaalde product verkoopt
    bool StoreHasProduct(string storeName, string productName);

    // Haal alle unieke producten op
    // Sorteer de producten van duur naar goedkoop
    IEnumerable<string> GetUniqueProducts();

    // Geef het product met de hoogste winstmarge terug van een bepaalde winkel
    Product GetProductWithHighestMargin(string storeName);

    // Geef alle winkels terug die voor een bepaald jaar opgericht zijn
    IEnumerable<Store> GetStoresByEstablishedYear(int year);

    // Geef de gemiddelde prijs van een bepaalde product over alle winkel terug
    decimal GetAverageProductPrice(string productName);

    // Geef de gemiddelde winstmarge van een bepaalde product over alle winkel terug
    decimal GetAverageProductMargin(string productName);

    // Geef de 5 duurste producten voor een winkel terug
    IEnumerable<Product> GetFiveMostExpensiveProducts(string storeName);

    // Geef per land de gemiddelde winstmarge van een bepaald product terug
    // gesorteerd van laag naar hoog
    // in het volgende formaat: "Country: [COUNTRY] - Margin: [MARGIN]"
    IEnumerable<string> GetAverageProductMarginPerCountryByProductName(string productName);

    // Geef terug hoeveel winkels er voor een bepaalde product verkopen in een bepaald land
    int GetNumberOfStoresByCountry(string productName, string countryName);
}
