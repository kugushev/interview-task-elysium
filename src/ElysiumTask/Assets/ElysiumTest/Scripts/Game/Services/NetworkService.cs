using System.Collections.Concurrent;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ElysiumTest.Scripts.Game.Enums;
using ElysiumTest.Scripts.Game.Interfaces;
using ElysiumTest.Scripts.Game.Models;
using ElysiumTest.Scripts.Game.Services.Dto;
using UnityEngine;
using UnityEngine.Networking;

namespace ElysiumTest.Scripts.Game.Services
{
    [CreateAssetMenu(menuName = Constants.RootMenu + "/Network")]
    public class NetworkService : ScriptableObject, INetworkService
    {
        // todo: move it to config
        [SerializeField] private int transactionId;
        [SerializeField] private string url = "https://dev3r02.elysium.today/inventory/status";
        [SerializeField] private string authHeader = "auth";
        [SerializeField] private string authToken; // todo: don't store token in source code
        [SerializeField] private int timeoutSeconds = 1;
        [SerializeField] private int retryCount = 2;

        private readonly ConcurrentQueue<ItemToBackpackInfo> _pool = new ConcurrentQueue<ItemToBackpackInfo>();

        private void Awake() => transactionId = 0;

        public void SendPutItem(Item item)
        {
            var payload = PayloadCreateOrPool();
            payload.Event = ItemToBackpackEvent.Put;
            payload.ItemId = item.ID;
            payload.TransactionId = transactionId++;

            ExecuteSendPutItem(payload);
        }

        public void SendTakeItem(Item item)
        {
            var payload = PayloadCreateOrPool();
            payload.Event = ItemToBackpackEvent.Take;
            payload.ItemId = item.ID;
            payload.TransactionId = transactionId++;

            ExecuteSendPutItem(payload);
        }

        private async void ExecuteSendPutItem(ItemToBackpackInfo payload)
        {
            // async void is not a good idea, but in this case it's ok

            int attempt = 0;

            var body = JsonUtility.ToJson(payload);

            await SendRequest(body, attempt);

            PayloadReturnPool(payload);
        }

        private async Task SendRequest(string body, int attempt)
        {
            bool success;
            using (var request = UnityWebRequest.Post(url, body))
            {
                request.SetRequestHeader(authHeader, authToken);
                request.timeout = timeoutSeconds;
                var result = request.SendWebRequest();

                success = await ExecuteSendRequest(result);
            }
            
            if (!success)
            {
                attempt++;
                if (attempt > retryCount)
                    TroubleshootNetwork(attempt);
                else
                    await SendRequest(body, attempt);
            }
        }

        private static async Task<bool> ExecuteSendRequest(UnityWebRequestAsyncOperation result)
        {
            bool success = false;
            try
            {
                await result.ToUniTask();
                success = true;
            }
            catch (UnityWebRequestException e)
            {
                // todo: if I had an information about server I'd be able to process error properly :)
                Debug.LogWarning($"Network error: {e.Message}");
            }

            return success;
        }

        private static void TroubleshootNetwork(int attempt)
        {
            // todo: basically we should suspend the application and wait response from the server
            Debug.LogError($"Unable to process request after {attempt} attempts");
        }

        #region Pooling

        // todo: use more generic pool (https://github.com/kugushev/games-prototypes-vr/blob/master/Template_Upd/Assets/Kugushev/Scripts/Common/Utils/Pooling/ObjectsPool.cs)

        private ItemToBackpackInfo PayloadCreateOrPool()
        {
            if (_pool.TryDequeue(out var payload))
            {
                payload.Clean();
                return payload;
            }

            return new ItemToBackpackInfo();
        }

        private void PayloadReturnPool(ItemToBackpackInfo payload)
        {
            payload.Clean();
            _pool.Enqueue(payload);
        }

        #endregion
    }
}