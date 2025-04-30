using Core.Entities;
using Riok.Mapperly.Abstractions;

namespace Core.Dtos.Mappers;

[Mapper(UseDeepCloning = true)]
public partial class CloneGenerator
{
    public partial OrderEntity Clone(OrderEntity source);

    public partial CreateOrderReq Clone(CreateOrderReq source);

}

[Mapper(UseDeepCloning = true)]
public static partial class StaticCloneGenerator
{
    public static partial OrderEntity Clone(this OrderEntity source);

}