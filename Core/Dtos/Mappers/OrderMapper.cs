using Core.Entities;
using Riok.Mapperly.Abstractions;

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
    }

}

[Mapper]
public static partial class Order2Mapper
{
    [MapperIgnoreTarget(nameof(OrderEntity.OrderId))]
    public static partial OrderEntity ToEntity(this CreateOrderReq car);
}