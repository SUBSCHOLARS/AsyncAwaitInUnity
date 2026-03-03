using System;
using System.Threading.Tasks;
using UnityEngine;

public class Breakfast : MonoBehaviour
{
    internal class HashBrown {}
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Coffee cup=PourCoffee();
        Debug.Log("coffee is ready");

        Egg eggs=FryEggs(2);
        Debug.Log("eggs are ready");

        HashBrown hashBrown=FryHashBrowns(3);
        Debug.Log("has browns are ready");

        Toast toast=ToastBread(2);
        ApplyButter(toast);
        ApplyJam(toast);
        Debug.Log("toast is ready");

        Juice oj=PourOJ();
        Debug.Log("oj is ready");
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

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Debug.Log("Putting a slice of bread in the toaster");
            }
            Debug.Log("Start toasting...");
            Task.Delay(3000).Wait();
            Debug.Log("Remove toast from toaster");

            return new Toast();
        }

        private static HashBrown FryHashBrowns(int patties)
        {
            Debug.Log($"putting {patties} hash brown patties in the pan");
            Debug.Log("cooking first side of hash browns...");
            Task.Delay(3000).Wait();
            for (int patty = 0; patty < patties; patty++)
            {
                Debug.Log("flipping a hash brown patty");
            }
            Debug.Log("cooking the second side of hash browns...");
            Task.Delay(3000).Wait();
            Debug.Log("Put hash browns on plate");

            return new HashBrown();
        }

        private static Egg FryEggs(int howMany)
        {
            for(int i=0; i<howMany; i++)
            {
                Debug.Log("Warming the egg pan...");
                Task.Delay(3000).Wait();
                Debug.Log($"cracking {i+1} eggs");
                Debug.Log("cooking the eggs ...");
                Task.Delay(3000).Wait();
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
