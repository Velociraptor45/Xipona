using ProjectHermes.ShoppingList.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Stores.Models
{
    public class StoresState
    {
        private List<Store> stores;

        public StoresState(IEnumerable<Store> stores)
        {
            this.stores = stores.ToList();
        }

        public IReadOnlyCollection<Store> Stores => stores.AsReadOnly();
        public Store EditedStore { get; private set; }
        public bool IsInEditMode => EditedStore != null;
        public Func<Task> StoreChanged { get; set; }
        public Action StateChanged { get; set; }

        public void EnterEditor(Store store)
        {
            EditedStore = store;
            StateChanged?.Invoke();
        }

        public async Task LeaveEditorAsync()
        {
            EditedStore = null;
            await StoreChanged?.Invoke();
        }

        public void UpdateStores(IEnumerable<Store> stores)
        {
            this.stores = stores.ToList();
            StateChanged?.Invoke();
        }

        public void EnterEditorForNewStore()
        {
            var defaultSection = new Section(new StoreSectionId(Guid.NewGuid()), "Default", 0, true);
            var store = new Store(Guid.Empty, "", new[] { defaultSection });

            EnterEditor(store);
        }
    }
}