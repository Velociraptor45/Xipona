setx ASPNETCORE_ENVIRONMENT "production"
setx PH_SL_VAULT_USERNAME_FILE "E:\\prd-ph-shoppinglist-vault-api-username.txt"
setx PH_SL_VAULT_PASSWORD_FILE "E:\\prd-ph-shoppinglist-vault-api-password.txt"
#setx PH_SL_DB_CONNECTION_STRING_FILE "E:\\prd-ph-shoppinglist-connection-string.txt"

cd ../ShoppingList.Api.Infrastructure/

dotnet ef database update --context ItemCategoryContext
dotnet ef database update --context ManufacturerContext
dotnet ef database update --context ShoppingListContext
dotnet ef database update --context ItemContext
dotnet ef database update --context StoreContext

setx ASPNETCORE_ENVIRONMENT ""
setx PH_SL_VAULT_USERNAME_FILE ""
setx PH_SL_VAULT_PASSWORD_FILE ""
setx PH_SL_DB_CONNECTION_STRING_FILE ""