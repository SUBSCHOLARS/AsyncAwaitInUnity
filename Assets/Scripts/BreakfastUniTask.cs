using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BreakfastUniTask : MonoBehaviour
{
    internal class HashBrown {}
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var token=this.GetCancellationTokenOnDestroy();
        Cooking(token).Forget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    static async UniTask Cooking(CancellationToken token)
    {
        Coffee cup=PourCoffee();
        Debug.Log("coffee is ready");

        // 各非同期メソッドにtokenは渡すこと。そうしないとUniTaskの最適化の恩恵を受けられない可能性がある。
        UniTask<Egg> eggsTask=FryEggsAsync(2, token);
        UniTask<HashBrown> hashBrownTask=FryHashBrownsAsync(3, token);
        UniTask<Toast> toastTask=ToastBreadAsync(2, token);

        Toast toast=await toastTask;
        ApplyButter(toast);
        ApplyJam(toast);
        Debug.Log("toast is ready");

        Juice oj=PourOJ();
        Debug.Log("oj is ready");

        Egg eggs=await eggsTask;
        Debug.Log("eggs are ready");
        HashBrown hashBrown=await hashBrownTask;
        Debug.Log("hash browns are ready");

        Debug.Log("Breakfast is ready!");
    }
    private static Juice PourOJ()
        {
            Debug.Log("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Debug.Log("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Debug.Log("Putting butter on the toast");

        private static async UniTask<Toast> ToastBreadAsync(int slices, CancellationToken token)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Debug.Log("Putting a slice of bread in the toaster");
            }
            Debug.Log("Start toasting...");
            await UniTask.Delay(3000, cancellationToken: token);
            Debug.Log("Remove toast from toaster");

            return new Toast();
        }

        private static async UniTask<HashBrown> FryHashBrownsAsync(int patties, CancellationToken token)
        {
            Debug.Log($"putting {patties} hash brown patties in the pan");
            Debug.Log("cooking first side of hash browns...");
            await UniTask.Delay(3000, cancellationToken: token);
            for (int patty = 0; patty < patties; patty++)
            {
                Debug.Log("flipping a hash brown patty");
            }
            Debug.Log("cooking the second side of hash browns...");
            await UniTask.Delay(3000, cancellationToken: token);
            Debug.Log("Put hash browns on plate");

            return new HashBrown();
        }

        private static async UniTask<Egg> FryEggsAsync(int howMany, CancellationToken token)
        {
            for(int i=0; i<howMany; i++)
            {
                Debug.Log("Warming the egg pan...");
                await UniTask.Delay(3000, cancellationToken: token);
                Debug.Log($"cracking {i+1} eggs");
                Debug.Log("cooking the eggs ...");
                await UniTask.Delay(3000, cancellationToken: token);
                Debug.Log("Put eggs on plate");
            }

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Debug.Log("Pouring coffee");
            return new Coffee();
        }
}
