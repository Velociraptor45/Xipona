cd ../ShoppingList.Api.Infrastructure/

dotnet ef database update --context ItemCategoryContext
dotnet ef database update --context ManufacturerContext
dotnet ef database update --context ShoppingListContext
dotnet ef database update --context ItemContext
dotnet ef database update --context StoreContext