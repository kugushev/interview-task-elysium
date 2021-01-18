using System.Threading;
using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Presentation.Controllers;

namespace ElysiumTest.Scripts.Presentation.Interfaces
{
    public interface IDragable
    {
        UniTask Drag(IInputInfo inputInfo, CancellationToken token);
    }
}