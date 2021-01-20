using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Presentation.Components;
using ElysiumTest.Scripts.Presentation.Interfaces;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public class InteractionsController : MonoBehaviour
    {
        [SerializeField] private InputController inputController;
        [SerializeField] private BackpackWidget backpack;

        private IItem _selectedItem;

        public void OnItemClick(IItem item)
        {
            if (_selectedItem != null)
                return;

            _selectedItem = item;

            StartCoroutine(UniTask.ToCoroutine(RunItemDrag));
        }

        private async UniTask RunItemDrag()
        {
            await _selectedItem.Drag(inputController);
        }

        public void OnRelease()
        {
            if (_selectedItem == null)
                return;

            StartCoroutine(UniTask.ToCoroutine(RunItemDrop));
        }

        public void OnReleaseToBackpack(BackpackWidget backpackWidget)
        {
            if (_selectedItem == null)
                return;

            var task = RunItemDropToBackpack(backpackWidget);
            StartCoroutine(task.ToCoroutine());
        }

        private async UniTask RunItemDropToBackpack(BackpackWidget backpackWidget)
        {
            var item = _selectedItem;
            _selectedItem = null;
            await item.DropToBackpack(inputController, backpackWidget);
        }

        private async UniTask RunItemDrop()
        {
            var item = _selectedItem;
            _selectedItem = null;
            await item.Drop(inputController);
        }
        
        public void OnReleaseOnMenu(ItemUIWidget itemUIWidget)
        {
            if (_selectedItem != null)
                return;

            if (backpack.TryTakeAway(itemUIWidget.Item, out var itemWidget))
            {
                _selectedItem = itemWidget;
                StartCoroutine(UniTask.ToCoroutine(RunItemDrag));
            }
            else
                Debug.LogWarning($"Item {itemUIWidget.Item} no found in backpack");
        }
    }
}