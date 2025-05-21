using Shop.Domain.Dtoes.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Dtoes.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int NumOfProduct { get { return Products.Count; } }
        //faqat get zadim ke readonly bashe va asan nemikhaim to DB zakhire beshe faqat baraye namayeshe
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        //age khasti faqat tedad dashti va list product ra nemikhasti kafie NumOfProduct { get; set; }
        //beshe va ye method minevisi ke tedad bargardone be ezaye categoryId va dg Products ra meqdar dehi nemikonim
    }
}
