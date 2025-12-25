using System.ComponentModel;
using System.Reflection;
using Core.Dtos;
using Core.Entities;
using Riok.Mapperly.Abstractions;
using NetEscapades.EnumGenerators;

namespace Core.Dtos.Mappers;

[Mapper]
public partial class ProductMapper
{
    /// <summary>
    /// 请求转实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [MapperIgnoreTarget(nameof(ProductEntity.ProductId))]
    [MapperIgnoreTarget(nameof(ProductEntity.CreatedAt))]
    public partial ProductEntity ReqToEntity(CreateProductReq req);

    /// <summary>
    /// 实体转DTO
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [MapProperty(nameof(ProductEntity.Category), nameof(ProductDto.CategoryDesc), Use = nameof(MapProductCategory))]
    public partial ProductDto EntityToDto(ProductEntity entity);

    [UserMapping(Default = false)]
    private static string MapProductCategory(ProductCategory category)
    {
        return category.ToStringFast(true);
        //var fieldInfo = category.GetType().GetField(category.ToString());
        //if (fieldInfo == null) return category.ToString();
        //var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        //return attributes.Length > 0 ? attributes[0].Description : category.ToString();
    }
}

[Mapper]
public static partial class ProductStaticMapper
{
    [MapperIgnoreTarget(nameof(ProductEntity.ProductId))]
    [MapperIgnoreTarget(nameof(ProductEntity.CreatedAt))]
    public static partial ProductEntity ToEntity(this CreateProductReq req);
}