using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Presentation.Common;
using ElysiumTest.Scripts.Presentation.Components;

namespace ElysiumTest.Scripts.Presentation.Interfaces
{
    public interface IBackpack
    {
        bool TryGetAttachPosition(Item item, out Position attachPosition);

        void Put(ItemWidget item);
    }
}