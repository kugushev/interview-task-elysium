using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Controllers;
using ElysiumTest.Scripts.Presentation.Interfaces;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Item item;

        private Transform _transform;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        public UniTask Drag(IInputInfo inputInfo, CancellationToken token)
        {
            _rigidbody.isKinematic = true;
            gameObject.layer = LayerMask.NameToLayer(TagsAndLayers.BackpackItemLayer);
            inputInfo.CursorMove += JumpTo;
            return UniTask.CompletedTask;
        }

        public UniTask Drop(IInputInfo inputInfo, CancellationToken token)
        {
            _rigidbody.isKinematic = false;
            gameObject.layer = LayerMask.NameToLayer(TagsAndLayers.DefaultLayer);
            inputInfo.CursorMove -= JumpTo;
            return UniTask.CompletedTask;
        }

        public async UniTask DropToBackpack(IInputInfo inputInfo, IBackpack backpack, CancellationToken token)
        {
            if (backpack.TryGetAttachPosition(item, out var attachPoint))
            {
                inputInfo.CursorMove -= JumpTo;

                await MoveTo(token, attachPoint);
            }
            else
                Debug.LogError($"Attach point for {item} with name {item.name} not found");
        }

        private async Task MoveTo(CancellationToken token, Position attachPoint)
        {
            // todo: animate in FixedUpdate and set rigidbody position to support appropriate collisions
            var startPosition = _transform.position;
            var startRotation = _transform.rotation;

            const float animationTime = 1f;
            for (float i = 0f; i < 1f; i += Time.deltaTime / animationTime)
            {
                if (token.IsCancellationRequested)
                    break;

                _transform.position = Vector3.Lerp(startPosition, attachPoint.Point, i);
                _transform.rotation = Quaternion.Slerp(startRotation, attachPoint.Rotation, i);

                await UniTask.NextFrame(cancellationToken: token);
            }
        }

        private void JumpTo(Vector3 position)
        {
            transform.position = position;
        }
    }
}