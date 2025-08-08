# API Test Script
Write-Host "=== SourceGeneratorTest API Test ===" -ForegroundColor Green

$baseUrl = "http://localhost:5088"

# Test health check
Write-Host "`n1. Testing health check..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/test/ping" -Method GET
    Write-Host "Success: Ping test passed: $response" -ForegroundColor Green
} catch {
    Write-Host "Error: Ping test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test create order
Write-Host "`n2. Testing create order..." -ForegroundColor Yellow
$orderData = @{
    name = "Zhang San"
    phone = "13800138000"
    address = @{
        city = "Beijing"
        detail = "Chaoyang District"
    }
    orderType = 1
} | ConvertTo-Json -Depth 3

try {
    $orderId = Invoke-RestMethod -Uri "$baseUrl/api/test/createOrder" -Method POST -Body $orderData -ContentType "application/json"
    Write-Host "Success: Order created with ID: $orderId" -ForegroundColor Green
    
    # Test get order
    Write-Host "`n3. Testing get order..." -ForegroundColor Yellow
    $order = Invoke-RestMethod -Uri "$baseUrl/api/test/getOrder?id=$orderId" -Method GET
    Write-Host "Success: Order retrieved: $($order.name), Phone: $($order.phone), Type: $($order.orderTypeDesc)" -ForegroundColor Green
} catch {
    Write-Host "Error: Order operation failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test create product
Write-Host "`n4. Testing create product..." -ForegroundColor Yellow
$productData = @{
    name = "iPhone 15"
    description = "Latest Apple smartphone"
    price = 7999.00
    category = 1
    isAvailable = $true
} | ConvertTo-Json -Depth 3

try {
    $productId = Invoke-RestMethod -Uri "$baseUrl/api/product" -Method POST -Body $productData -ContentType "application/json"
    Write-Host "Success: Product created with ID: $productId" -ForegroundColor Green
    
    # Test get product
    Write-Host "`n5. Testing get product..." -ForegroundColor Yellow
    $product = Invoke-RestMethod -Uri "$baseUrl/api/product/$productId" -Method GET
    Write-Host "Success: Product retrieved: $($product.name), Price: $($product.price), Category: $($product.categoryDesc)" -ForegroundColor Green
    
    # Test get all products
    Write-Host "`n6. Testing get all products..." -ForegroundColor Yellow
    $products = Invoke-RestMethod -Uri "$baseUrl/api/product" -Method GET
    Write-Host "Success: Retrieved $($products.Count) products" -ForegroundColor Green
} catch {
    Write-Host "Error: Product operation failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Test Complete ===" -ForegroundColor Green
Write-Host "Visit Swagger documentation: $baseUrl/swagger" -ForegroundColor Cyan
Write-Host "`nPress any key to exit..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")