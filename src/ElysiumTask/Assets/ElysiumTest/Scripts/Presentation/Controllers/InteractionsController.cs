using System.Threading;
using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Presentation.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public class InteractionsController : MonoBehaviour
    {
        [SerializeField] private InputController inputController;

        private ItemWidget _selectedItem;

        public void OnItemClick(ItemWidget item)
        {
            if (_selectedItem != null)
                return;

            _selectedItem = item;

            StartCoroutine(UniTask.ToCoroutine(RunItemDrag));
        }

        private async UniTask RunItemDrag()
        {
            // todo: user custom poolable CancellationTokenSources to reduce GC pressure 
            using (var cts = new CancellationTokenSource())
            {
                await _selectedItem.Drag(inputController, cts.Token);
            }
        }

        public void OnRelease([CanBeNull] BackpackWidget backpackWidget)
        {
            if (_selectedItem == null)
                return;

            if (backpackWidget != null)
            {
                var task = RunItemDropToBackpack(backpackWidget);
                StartCoroutine(task.ToCoroutine());
            }
            else
                StartCoroutine(UniTask.ToCoroutine(RunItemDrop));
        }

        private async UniTask RunItemDropToBackpack(BackpackWidget backpackWidget)
        {
            // todo: user custom poolable CancellationTokenSources to reduce GC pressure
            using (var cts = new CancellationTokenSource())
            {
                await _selectedItem.DropToBackpack(inputController, backpackWidget, cts.Token);
                _selectedItem = null;
            }
        }

        private async UniTask RunItemDrop()
        {
            // todo: user custom poolable CancellationTokenSources to reduce GC pressure
            using (var cts = new CancellationTokenSource())
            {
                await _selectedItem.Drop(inputController, cts.Token);
                _selectedItem = null;
            }
        }
    }
}