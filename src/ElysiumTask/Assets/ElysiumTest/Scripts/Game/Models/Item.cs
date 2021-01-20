using ElysiumTest.Scripts.Game.Enums;
using UnityEngine;

namespace ElysiumTest.Scripts.Game.Models
{
    [CreateAssetMenu(menuName = Constants.RootMenu + "/Item")]
    public sealed class Item : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string itemName;
        [SerializeField] private ItemType itemType;
        [SerializeField] private float weight;

        public int ID => id;
        public string ItemName => itemName;
        public ItemType ItemType => itemType;
        public float Weight => weight;

        public bool InBackpack { get; set; }
        // add Reactive binding
        
        // add view model with: mouse position and mouse attached state
        
    }
}