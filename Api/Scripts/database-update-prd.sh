if [[ "${ASPNETCORE_ENVIRONMENT}" = "development" ]]; then
  echo "Hmmm, your environment variable is set to development. This doesn't seem right..."
  read -n 1
  exit
fi

cd ../ShoppingList.Api.Repositories/

dotnet ef database update --context ItemCategoryContext
dotnet ef database update --context ManufacturerContext --no-build
dotnet ef database update --context ShoppingListContext --no-build
dotnet ef database update --context ItemContext --no-build
dotnet ef database update --context StoreContext --no-build
dotnet ef database update --context RecipeContext --no-build
dotnet ef database update --context RecipeTagContext --no-build

echo ""
echo "Database update finished"
read -n 1
exit