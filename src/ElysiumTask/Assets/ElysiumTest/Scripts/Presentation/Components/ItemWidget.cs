using System.Threading;
using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Controllers;
using UnityEngine;

namespace ElysiumTest.Scripts.Presentation.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Item item;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public UniTask Drag(IInputController inputController, CancellationToken token)
        {
            _rigidbody.useGravity = false;
            
            return UniTask.CompletedTask;
        }

        public UniTask Drop(InputController inputController)
        {
            _rigidbody.useGravity = true;
            return UniTask.CompletedTask;
        }

        public void MoveTo(Vector3 position)
        {
            transform.position = position;
        }
    }
}