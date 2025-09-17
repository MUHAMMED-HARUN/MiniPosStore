using SharedModels.EF.DTO;
using Microsoft.AspNetCore.Http;

namespace BAL.BALDTO
{
    public class ProductDTO : ProductDTO
    {
        public IFormFile ProductImage { set; get; }
    }
}
