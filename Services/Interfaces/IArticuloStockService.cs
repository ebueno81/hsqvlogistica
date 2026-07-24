using HsqvLogistica.Models.DTOs.Articulos;

namespace HsqvLogistica.Services.Interfaces
{
    public interface IArticuloStockService
    {
        Task<StockArticuloDto> ValidarStockAsync(int idArticulo, decimal cantidad);
    }
}
