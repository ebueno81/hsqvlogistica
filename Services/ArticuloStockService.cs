using HsqvLogistica.Models.DTOs.Articulos;
using HsqvLogistica.Repositories.Interfaces;
using HsqvLogistica.Services.Interfaces;

namespace HsqvLogistica.Services
{
    public class ArticuloStockService : IArticuloStockService
    {
        private readonly IArticuloRepository _repository;

        public ArticuloStockService(IArticuloRepository repository)
        {
            _repository = repository;
        }

        public async Task<StockArticuloDto> ValidarStockAsync(
            int idArticulo,
            decimal cantidad)
        {
            var articulo = await _repository.GetStockAsync(idArticulo);

            if (articulo == null)
                throw new Exception("El artículo no existe.");

            if (articulo.Stock < cantidad)
            {
                throw new Exception(
                    $"El artículo '{articulo.Descripcion}' no tiene stock suficiente. " +
                    $"Stock actual: {articulo.Stock}, Cantidad solicitada: {cantidad}.");
            }

            return new StockArticuloDto
            {
                IdArticulo = articulo.IdArticulo,
                Codigo = articulo.Codigo,
                Descripcion = articulo.Descripcion,
                Stock = (decimal) articulo.Stock
            };
        }
    }
}