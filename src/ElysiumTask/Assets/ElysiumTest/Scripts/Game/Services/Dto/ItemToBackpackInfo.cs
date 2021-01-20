using System;
using ElysiumTest.Scripts.Game.Enums;
using UnityEngine;

namespace ElysiumTest.Scripts.Game.Services.Dto
{
    [Serializable]
    public class ItemToBackpackInfo
    {
        [SerializeField] private ItemToBackpackEvent @event;
        [SerializeField] private int itemId;
        [SerializeField] private int transactionId;

        public ItemToBackpackEvent Event
        {
            get => @event;
            set => @event = value;
        }

        public int ItemId
        {
            get => itemId;
            set => itemId = value;
        }

        /// <summary>
        /// To support retry we have to make our requests idemponent
        /// </summary>
        public int TransactionId
        {
            get => transactionId;
            set => transactionId = value;
        }

        public void Clean()
        {
            @event = ItemToBackpackEvent.Unspecified;
            itemId = default;
            transactionId = default;
        }
    }
}