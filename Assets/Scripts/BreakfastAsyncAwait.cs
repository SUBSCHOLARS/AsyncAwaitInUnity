using UnityEngine;
using System.Threading.Tasks;

public class BreakfastAsyncAwait : MonoBehaviour
{
    internal class HashBrown {}
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _=Cooking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static async Task Cooking()
    {
        Coffee cup=PourCoffee();
        Debug.Log("coffee is ready");

        Task<Egg> eggsTask=FryEggsAsync(2);
        Task<HashBrown> hashBrownTask=FryHashBrownsAsync(3);
        Task<Toast> toastTask=ToastBreadAsync(2);

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

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Debug.Log("Putting a slice of bread in the toaster");
            }
            Debug.Log("Start toasting...");
            await Task.Delay(3000);
            Debug.Log("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<HashBrown> FryHashBrownsAsync(int patties)
        {
            Debug.Log($"putting {patties} hash brown patties in the pan");
            Debug.Log("cooking first side of hash browns...");
            await Task.Delay(3000);
            for (int patty = 0; patty < patties; patty++)
            {
                Debug.Log("flipping a hash brown patty");
            }
            Debug.Log("cooking the second side of hash browns...");
            await Task.Delay(3000);
            Debug.Log("Put hash browns on plate");

            return new HashBrown();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            for(int i=0; i<howMany; i++)
            {
                Debug.Log("Warming the egg pan...");
                await Task.Delay(3000);
                Debug.Log($"cracking {i+1} eggs");
                Debug.Log("cooking the eggs ...");
                await Task.Delay(3000);
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
