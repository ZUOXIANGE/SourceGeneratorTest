using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace SourceGeneratorTest.Infrastructure;

/// <summary>
/// OpenAPI 文档转换器：把每个接口的 200 响应模型包装成统一响应结构
/// </summary>
public static class ApiResultDocumentTransformer
{
    public static Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        foreach (var pathItem in document.Paths.Values)
        {
            if (pathItem.Operations is null)
                continue;

            foreach (var operation in pathItem.Operations.Values)
            {
                var responses = operation.Responses;
                if (responses is null || !responses.TryGetValue("200", out var response))
                    continue;

                var content = response.Content;
                if (content is null || content.Count == 0)
                    continue;

                var jsonMediaType = content
                    .FirstOrDefault(item => item.Key.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                    .Value;

                // 文件下载等非 JSON 响应不包装成统一响应，仅标注为二进制
                if (jsonMediaType is null)
                {
                    foreach (var (mediaTypeName, mediaType) in content)
                    {
                        if (mediaTypeName.Contains("octet-stream", StringComparison.OrdinalIgnoreCase))
                            mediaType.Schema = new OpenApiSchema { Type = JsonSchemaType.String, Format = "binary" };
                    }

                    continue;
                }

                var dataSchema = jsonMediaType.Schema;

                // 过滤器统一输出 application/json，这里同步修正文档中的响应类型
                content.Clear();
                content["application/json"] = new OpenApiMediaType
                {
                    Schema = Wrap(dataSchema)
                };
            }
        }

        return Task.CompletedTask;
    }

    private static OpenApiSchema Wrap(IOpenApiSchema? dataSchema)
    {
        return new OpenApiSchema
        {
            Type = JsonSchemaType.Object,
            Required = new HashSet<string> { "code", "data", "msg" },
            Properties = new Dictionary<string, IOpenApiSchema>
            {
                ["code"] = new OpenApiSchema
                {
                    Type = JsonSchemaType.Integer,
                    Format = "int32",
                    Description = "业务状态码，200 表示成功"
                },
                ["data"] = new OpenApiSchema
                {
                    Description = "业务数据",
                    OneOf = new List<IOpenApiSchema>
                    {
                        new OpenApiSchema { Type = JsonSchemaType.Null },
                        dataSchema ?? new OpenApiSchema { Type = JsonSchemaType.Null }
                    }
                },
                ["msg"] = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Description = "提示信息"
                }
            }
        };
    }
}
