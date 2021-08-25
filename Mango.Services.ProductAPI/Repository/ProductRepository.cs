using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<ProductDto, Product>(productDto);
            if(product.ProductId > 0)
            {
                _db.products.Update(product);
            }
            else
            {
                _db.products.Add(product);
            }
            await _db.SaveChangesAsync();
            return  _mapper.Map<Product,ProductDto>(product);

        }

        public async  Task<bool> DeleteProduct(int ProductId)
        {
            try
            {
                Product product = await _db.products.Where(x => x.ProductId == ProductId).FirstOrDefaultAsync();
                if(product == null)
                {
                    return false;
                }
                _db.products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            Product product = await _db.products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            List<Product> productList = await _db.products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(productList);
        }
    }
}
