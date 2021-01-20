using System.Collections.Generic;
using ElysiumTest.Scripts.Game.Events;
using UnityEngine;

namespace ElysiumTest.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = Constants.RootMenu + "/Backpack")]
    public sealed class Backpack : ScriptableObject
    {
        [SerializeField] private BackpackUpdatedEvent onPut;
        [SerializeField] private BackpackUpdatedEvent onTake;

        private readonly Dictionary<int, Item> _items = new Dictionary<int, Item>();

        public bool TryPut(Item item)
        {
            if (_items.ContainsKey(item.ID))
                return false;

            onPut.Invoke(item);
            _items.Add(item.ID, item);
            return true;
        }

        public bool TryTake(int id, out Item item)
        {
            if (_items.TryGetValue(id, out item))
            {
                _items.Remove(id);
                onTake.Invoke(item);
                return true;
            }
            return false;
        }
    }
}