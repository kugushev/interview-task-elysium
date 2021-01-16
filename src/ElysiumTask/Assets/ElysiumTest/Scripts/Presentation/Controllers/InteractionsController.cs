using System.Threading;
using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Presentation.Components;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace ElysiumTest.Scripts.Presentation.Controllers
{
    public class InteractionsController : MonoBehaviour
    {
        [SerializeField] private InputController inputController;
        [SerializeField] private Transform nearCameraAnchor;

        private ItemWidget _selectedItem;
        private CancellationTokenSource _cancellationTokenSource;

        public void OnItemClick(ItemWidget item)
        {
            if (_selectedItem != null)
                return;

            _selectedItem = item;
            _cancellationTokenSource = new CancellationTokenSource();

            StartCoroutine(UniTask.ToCoroutine(RunItemDrag));
        }

        private UniTask RunItemDrag() => _selectedItem.Drag(inputController, _cancellationTokenSource.Token);

        public void OnRelease()
        {
            if (!ReferenceEquals(_selectedItem, null)) 
                StartCoroutine(UniTask.ToCoroutine(RunItemDrop));
        }
        
        private async UniTask RunItemDrop()
        {
            await _selectedItem.Drop(inputController);
            _selectedItem = null;
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        public void OnMove(Vector3 position)
        {
            if (_selectedItem == null)
                return;

            _selectedItem.MoveTo(position);
        }
    }
}