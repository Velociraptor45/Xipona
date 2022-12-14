if [[ "${ASPNETCORE_ENVIRONMENT}" = "production" ]]; then
  echo "Pheww, that was close! Your environment variable is set to production. Better terminating here..."
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