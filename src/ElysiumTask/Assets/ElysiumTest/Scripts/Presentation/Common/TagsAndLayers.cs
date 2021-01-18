namespace ElysiumTest.Scripts.Presentation.Common
{
    internal static class TagsAndLayers
    {
        #region Tags

        internal const string InventoryTag = "Inventory";
        internal const string InventoryItemTag = "InventoryItem";

        #endregion

        #region Layers

        internal const string DefaultLayer = "Default";
        internal const string BackpackLayer = "Backpack";
        internal const string BackpackItemLayer = "BackpackItem";

        #endregion

        #region Rules

        public static (string, string) DragableLayerAndTag => (DefaultLayer, InventoryItemTag);

        public static (string, string) DroppableLayerAndTag => (BackpackLayer, InventoryTag);

        #endregion
    }
}