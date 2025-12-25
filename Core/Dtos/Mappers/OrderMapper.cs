using System.ComponentModel;
using System.Reflection;
using Core.Dtos;
using Core.Entities;
using Riok.Mapperly.Abstractions;
using NetEscapades.EnumGenerators;

namespace Core.Dtos.Mappers;

[Mapper]
public partial class OrderMapper
{
    //一定要明确指定哪些忽略,不然会有警告,这就是源生成的优点
    [MapperIgnoreTarget(nameof(OrderEntity.OrderId))]
    public partial OrderEntity ReqToEntity(CreateOrderReq req);

    //自定义映射
    [MapProperty(nameof(OrderEntity.OrderType), nameof(OrderDto.OrderTypeDesc), Use = nameof(MapOrderType))]
    public partial OrderDto EntityToDto(OrderEntity req);

    [UserMapping(Default = false)]
    private static string MapOrderType(OrderType type)
    {
        return type.ToStringFast(true);
        //var fieldInfo = type.GetType().GetField(type.ToString());
        //if (fieldInfo == null) return type.ToString();
        //var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        //return attributes.Length > 0 ? attributes[0].Description : type.ToString();
    }

}

[Mapper]
public static partial class Order2Mapper
{
    [MapperIgnoreTarget(nameof(OrderEntity.OrderId))]
    public static partial OrderEntity ToEntity(this CreateOrderReq car);
}