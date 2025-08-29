using DAL.EF.DTO;
using Microsoft.AspNetCore.Http;

namespace BAL.BALDTO
{
    public class ProductBALDTO : ProductDTO
    {
        public IFormFile ProductImage { set; get; }
    }
}
