# Script de prueba para verificar que los errores de ObjectId están corregidos

Write-Host "=== Probando endpoints de Usuario con IDs inválidos ===" -ForegroundColor Yellow

# Esperar a que la aplicación se inicie
Start-Sleep -Seconds 3

# Test 1: GET con ID inválido (debería retornar 404, no error 500)
Write-Host "`nTest 1: GET usuario con ID inválido" -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5233/api/Users/invalid-id-123" -Method Get -Headers @{"Accept"="application/json"}
    Write-Host "Respuesta exitosa: $response" -ForegroundColor Green
} catch {
    $statusCode = $_.Exception.Response.StatusCode
    Write-Host "Error esperado recibido - StatusCode: $statusCode" -ForegroundColor Green
}

# Test 2: DELETE con ID inválido (debería retornar 404, no error 500)
Write-Host "`nTest 2: DELETE usuario con ID inválido" -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5233/api/Users/invalid-id-456" -Method Delete
    Write-Host "Respuesta exitosa: $response" -ForegroundColor Green
} catch {
    $statusCode = $_.Exception.Response.StatusCode
    Write-Host "Error esperado recibido - StatusCode: $statusCode" -ForegroundColor Green
}

# Test 3: GET con ID de formato ObjectId pero que no existe (24 caracteres hex)
Write-Host "`nTest 3: GET usuario con ObjectId válido pero inexistente" -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5233/api/Users/68c613ddcd15a7ebad999999" -Method Get -Headers @{"Accept"="application/json"}
    Write-Host "Respuesta exitosa: $response" -ForegroundColor Green
} catch {
    $statusCode = $_.Exception.Response.StatusCode
    Write-Host "Error esperado recibido - StatusCode: $statusCode" -ForegroundColor Green
}

# Test 4: POST válido para crear un usuario
Write-Host "`nTest 4: POST crear usuario válido" -ForegroundColor Cyan
$createUserData = @{
    firstName = "Test"
    lastName = "User"
    email = "test.user@example.com"
    phoneNumber = "+1234567890"
    password = "Password123!"
    address = @{
        street = "123 Test Street"
        city = "Test City"
        state = "Test State"
        zipCode = "12345"
        country = "Test Country"
    }
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5233/api/Users" -Method Post -Body $createUserData -Headers @{"Content-Type"="application/json"; "Accept"="application/json"}
    Write-Host "Usuario creado exitosamente - ID: $($response.id)" -ForegroundColor Green
} catch {
    $statusCode = $_.Exception.Response.StatusCode
    Write-Host "Error al crear usuario - StatusCode: $statusCode" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Pruebas completadas ===" -ForegroundColor Yellow