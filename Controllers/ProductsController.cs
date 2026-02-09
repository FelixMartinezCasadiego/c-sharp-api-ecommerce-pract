using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Models.Dtos.Responses;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    [Authorize(Roles = "Admin")]
    public class ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper) : ControllerBase
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;

        [AllowAnonymous] // Allow anonymous access to this action
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            var productsDto = _mapper.Map<List<ProductDto>>(products); // AutoMapper can map collections directly
            
            return Ok(productsDto);
        }
        
        [AllowAnonymous] // Allow anonymous access to this action
        [HttpGet("{ProductId:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProduct(int ProductId)
        {
            var product = _productRepository.GetProduct(ProductId);
            if (product == null)
            {
                return NotFound($"Product with id {ProductId} not found.");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [AllowAnonymous] // Allow anonymous access to this action
        [HttpGet("Paged", Name = "GetProductsInPage")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProductsInPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size run must be greater than zero.");
            }

            var totalProducts = _productRepository.GetTotalProducts();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            if(pageNumber > totalPages)
            {
                return NotFound($"Page number {pageNumber} exceeds total pages {totalPages}.");
            }
            var products = _productRepository.GetProductsInPages(pageNumber, pageSize);
            var productDto = _mapper.Map<List<ProductDto>>(products);
            var paginationResponse = new PaginationResponse<ProductDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Items = productDto
            };

            return Ok(paginationResponse);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreatedProduct([FromForm] CreateProductDto createProductDto)
        {
            if(createProductDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_productRepository.ProductExists(createProductDto.Name))
            {
                ModelState.AddModelError("CustomError", "Product already exists!");
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(createProductDto.CategoryId))
            {
                ModelState.AddModelError("CustomError", "Category does not exist!");
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(createProductDto);
            // * Add image
            if(createProductDto.Image != null)
            {
                UploadProductImage(createProductDto, product);
            } else
            {
                product.ImgUrl = "https://placehold.co/600x400"; 
            }

            if (!_productRepository.CreateProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Something went wrong when saving the record {product.Name}");
                return StatusCode(500, ModelState);
            }

            var createdProduct = _productRepository.GetProduct(product.ProductId);
            var productDto = _mapper.Map<ProductDto>(createdProduct);
            return CreatedAtRoute("GetProduct", new { productId = product.ProductId }, productDto); // return 201
        }

        [HttpGet("searchProductByCategory/{categoryId:int}", Name = "GetProductsForCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProductsForCategory(int categoryId)
        {
            var products = _productRepository.GetProductsForCategory(categoryId);
            if (products.Count == 0)
            {
                return NotFound($"Products with category {categoryId} not found.");
            }

            var productDto = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDto);
        }

        [HttpGet("searchProductByCNameDescription/{searchTerm}", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SearchProducts(string searchTerm)
        {
            var products = _productRepository.SearchProducts(searchTerm);
            if (products.Count == 0)
            {
                return NotFound($"Products with name {searchTerm} not found.");
            }

            var productDto = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDto);
        }
        
        [HttpPatch("buyProduct/{name}/{quantity:int}", Name = "BuyProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult BuyProduct(string name, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
            {
                return BadRequest("Invalid product name or quantity.");
            }

            var foundProduct = _productRepository.ProductExists(name);
            if (!foundProduct)
            {
                return NotFound($"Product with name {name} not found.");
            }

            var isPurchased = _productRepository.BuyProduct(name, quantity);
            if (!isPurchased)
            {
                return BadRequest($"Insufficient stock for product {name}.");
            }
            
            return Ok($"Successfully purchased {quantity} of product {name}.");
        }

        [HttpPut("{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct(int productId, [FromForm] UpdateProductDto updateProductDto)
        {
            if(updateProductDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!_productRepository.ProductExists(productId))
            {
                ModelState.AddModelError("CustomError", "Product does not exist!");
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(updateProductDto.CategoryId))
            {
                ModelState.AddModelError("CustomError", "Category does not exist!");
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(updateProductDto);
            product.ProductId = productId;

            // * Update image
            if(updateProductDto.Image != null)
            {
                UploadProductImage(updateProductDto, product);
            }
            else
            {
                product.ImgUrl = "https://placehold.co/600x400"; 
            }

            if (!_productRepository.UpdateProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Something went wrong when updating the record {product.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent(); // return 204
        }

        private void UploadProductImage(dynamic productDto, Product product)
        {
            string fileName = product.ProductId + Guid.NewGuid().ToString() + Path.GetExtension(productDto.Image.FileName);
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductsImages");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            var filePath = Path.Combine(imagesFolder, fileName);
            FileInfo file = new(filePath);
            if (file.Exists)
            {
                file.Delete();
            }
            using var fileStream = new FileStream(filePath, FileMode.Create); // Create the file
            productDto.Image.CopyTo(fileStream); // Copy the uploaded image to the file stream
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}"; // Get the base URL of the request
            product.ImgUrl = $"{baseUrl}/ProductsImages/{fileName}"; // Set the ImgUrl property
            product.ImgUrlLocal = filePath; // Set the local file path
        }

        [HttpDelete("{ProductId:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteProduct(int ProductId)
        {
            if (ProductId == 0)
            {
                return BadRequest("Invalid product id.");
            }

            var product = _productRepository.GetProduct(ProductId);
            if (product == null)
            {
                return NotFound($"Product with id {ProductId} not found.");
            }

            if (!_productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError("CustomError", $"Something went wrong when deleting the record {product.Name}");
                return StatusCode(500, ModelState);
            }
            
            return NoContent(); // return 204           
        }
    }
}
