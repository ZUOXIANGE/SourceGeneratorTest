using AutoCtor;
using Core.Data;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl;

[AutoConstruct]
public partial class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// 添加产品
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<Guid> AddAsync(ProductEntity product)
    {
        var id = Guid.NewGuid();
        product.ProductId = id;
        product.CreatedAt = DateTime.Now;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        return id;
    }

    /// <summary>
    /// 根据ID获取产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProductEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == id && p.IsAvailable);
    }

    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            // 软删除：标记为不可用
            product.IsAvailable = false;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// 获取所有产品
    /// </summary>
    /// <returns></returns>
    public async Task<List<ProductEntity>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => p.IsAvailable)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// 创建产品实体
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        product.ProductId = Guid.NewGuid();
        product.CreatedAt = DateTime.Now;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        return product;
    }

    /// <summary>
    /// 更新产品
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public async Task<ProductEntity> UpdateAsync(ProductEntity product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// 根据分类获取产品
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public async Task<List<ProductEntity>> GetByCategoryAsync(ProductCategory category)
    {
        return await _context.Products
            .Where(p => p.Category == category && p.IsAvailable)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <summary>
    /// 删除产品（返回是否成功）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        // 软删除：标记为不可用
        product.IsAvailable = false;
        await _context.SaveChangesAsync();
        
        return true;
    }
}