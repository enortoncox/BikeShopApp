using AutoMapper;
using BikeShopApp.Core.Models;
using BikeShopApp.Infrastructure.DatabaseContext;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BikeShopApp.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DecreaseQuantityAsync(int productId)
        {
            var product = await GetProductAsync(productId);

            if (product != null && product.Quantity > 0)
            {
                product.Quantity -= 1;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await GetProductAsync(productId);

            if (product != null)
            {
                _context.Products.Remove(product);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public Task<List<Product>> GetAllProductsThatStartWithLetterAsync(string letter)
        {
            return _context.Products.Where(p => p.Name.StartsWith(letter)).ToListAsync();
        }

        public Task<List<Product>> GetAllProductsWithQuantityOrLowerAsync(int quantity)
        {
            return _context.Products.Where(p => p.Quantity <= quantity).ToListAsync();
        }

        public Task<Product?> GetProductAsync(int productId)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public Task<List<Product>> GetProductsAsync(string category)
        {
            if (category != null && category != "" && int.TryParse(category, out int result))
            {
                return _context.Products.Where(p => p.CategoryId == result).ToListAsync();
            }
            else
            {
                return _context.Products.ToListAsync();
            }
        }

        public async Task<ProductsPageResponseDto?> GetProductsByPageAsync(string category, string currentPage, string pageResults)
        {
            if (category != null && category != "" && int.TryParse(category, out int categoryParsed))
            {
                if (currentPage != null && currentPage != "" && int.TryParse(currentPage, out int pageParsed))
                {
                    if (pageResults != null && pageResults != "" && float.TryParse(pageResults, out float resultsParsed))
                    {

                        var pageCount = Math.Ceiling(await _context.Products.Where(p => p.CategoryId == categoryParsed).CountAsync() / resultsParsed);

                        var products = await _context.Products
                            .Where(p => p.CategoryId == categoryParsed)
                            .Skip((pageParsed - 1) * (int)resultsParsed)
                            .Take((int)resultsParsed)
                            .ToListAsync();

                        var productsMapped = _mapper.Map<List<ProductDto>>(products);

                        var response = new ProductsPageResponseDto
                        {
                            Products = productsMapped,
                            CurrentPage = pageParsed,
                            Pages = (int)pageCount
                        };

                        return response;
                    }
                }
            }

            return null;
        }

        public async Task<ProductsPageResponseDto?> GetFilteredProductsByPageAsync(string category, string currentPage, string pageResults, string price, string rating)
        {
            if (category != null && category != "" && int.TryParse(category, out int categoryParsed))
            {
                if (currentPage != null && currentPage != "" && int.TryParse(currentPage, out int pageParsed))
                {
                    if (pageResults != null && pageResults != "" && float.TryParse(pageResults, out float resultsParsed))
                    {
                        if (price != null && price != "" && int.TryParse(price, out int priceParsed))
                        {
                            if (rating != null && rating != "" && int.TryParse(rating, out int ratingParsed))
                            {

                                var products = new List<Product>();

                                if (priceParsed == 0 && ratingParsed != 0)
                                {
                                    products = await _context.Products.Where(p => p.CategoryId == categoryParsed && p.AvgRating >= ratingParsed).ToListAsync();
                                }
                                else if (priceParsed != 0 && ratingParsed == 0)
                                {
                                    products = await _context.Products.Where(p => p.CategoryId == categoryParsed && p.Price <= priceParsed).ToListAsync();
                                }
                                else if (priceParsed != 0 && ratingParsed != 0)
                                {
                                    products = await _context.Products.Where(p => p.CategoryId == categoryParsed && p.Price <= priceParsed && p.AvgRating >= ratingParsed).ToListAsync();
                                }
                                else
                                {
                                    products = await _context.Products.Where(p => p.CategoryId == categoryParsed).ToListAsync();
                                }

                                var pageCount = Math.Ceiling(products.Count() / resultsParsed);

                                products = products.Skip((pageParsed - 1) * (int)resultsParsed).Take((int)resultsParsed).ToList();

                                var productsMapped = _mapper.Map<List<ProductDto>>(products);

                                var response = new ProductsPageResponseDto
                                {
                                    Products = productsMapped,
                                    CurrentPage = pageParsed,
                                    Pages = (int)pageCount
                                };

                                return response;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public Task<bool> ProductExistsAsync(int productId)
        {
            return _context.Products.AnyAsync(p => p.ProductId == productId);
        }

        public async Task<bool> SetAvgRatingOfProductAsync(int productId)
        {
            var product = await GetProductAsync(productId);

            var reviews = await _context.Reviews.Where(r => r.ProductId == productId).ToListAsync();

            if (product != null && reviews != null)
            {
                int fullRating = 0;

                foreach (var review in reviews)
                {
                    fullRating += review.Rating;
                }

                if (fullRating == 0)
                {
                    product.AvgRating = 0;
                }
                else
                {
                    product.AvgRating = fullRating / reviews.Count;
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
