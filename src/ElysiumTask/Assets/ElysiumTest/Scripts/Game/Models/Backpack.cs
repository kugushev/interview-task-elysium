using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ElysiumTest.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = Constants.RootMenu + "/Backpack")]
    public sealed class Backpack : ScriptableObject
    {
        [SerializeField] private UnityEvent onPut;
        [SerializeField] private UnityEvent onTake;

        private readonly Dictionary<int, Item> _items = new Dictionary<int, Item>();

        public bool TryPut(Item item)
        {
            if (_items.ContainsKey(item.ID))
                return false;

            onPut.Invoke();
            _items.Add(item.ID, item);
            return true;
        }

        public bool TryTake(int id, out Item item)
        {
            if (_items.TryGetValue(id, out item))
            {
                onTake.Invoke();
                return true;
            }
            return false;
        }
    }
}