setx ASPNETCORE_ENVIRONMENT "production"
setx PH_SL_VAULT_USERNAME_FILE "E:\\prd-ph-shoppinglist-vault-api-username.txt"
setx PH_SL_VAULT_PASSWORD_FILE "E:\\prd-ph-shoppinglist-vault-api-password.txt"

cd ../ShoppingList.Api.Infrastructure/

dotnet ef database update --context ItemCategoryContext
dotnet ef database update --context ManufacturerContext
dotnet ef database update --context ShoppingListContext
dotnet ef database update --context ItemContext
dotnet ef database update --context StoreContext

setx ASPNETCORE_ENVIRONMENT ""
setx PH_SL_VAULT_USERNAME_FILE ""
setx PH_SL_VAULT_PASSWORD_FILE ""