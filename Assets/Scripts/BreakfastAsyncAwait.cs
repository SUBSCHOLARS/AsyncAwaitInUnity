using UnityEngine;
using System.Threading.Tasks;
using System;

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

        var eggsTask=FryEggsAsync(2);
        var hashBrownTask=FryHashBrownsAsync(3);
        var toastTask=MakeToastWithButterAndJamAsync(2);

        var eggs=await eggsTask;
        Debug.Log("eggs are ready");
        var hashBrown=await hashBrownTask;
        Debug.Log("hash browns are ready");
        var toast=await toastTask;
        Debug.Log("toast is ready");

        Juice oj=PourOJ();
        Debug.Log("oj is ready");
        Debug.Log("Breakfast is ready!");
    }
    // ApplyJamやAPplyButterは同期操作であるが、ToastBreadAsyncという非同期操作に続く操作であるために、
    // 全体としては非同期操作となる
    static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
    {
        var toast=await ToastBreadAsync(number);
        ApplyButter(toast);
        ApplyJam(toast);

        return toast;
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
            await Task.Delay(2000);
            // Debug.Log("Fire! Toast is ruined!");
            // throw new InvalidOperationException("The toaster is on fire");
            await Task.Delay(1000);
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
