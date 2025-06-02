

using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.DTOs.ProductDTOs
{
    public class AddUpdateProductDTO : GetSingleProductDTO
    {
        public List<ulong>? CategoryIds { get; set; }

    }
}
