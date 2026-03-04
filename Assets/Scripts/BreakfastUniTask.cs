using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using System.Linq;

public class BreakfastUniTask : MonoBehaviour
{
    internal class HashBrown {}
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }

    [Header("Coffee")]
    [SerializeField] private Image coffeeImage;
    [SerializeField] private Sprite[] coffeeSprites; // 0: 空, 1: 満タン

    [Header("Egg")]
    [SerializeField] private Image eggPanImage;
    [SerializeField] private Sprite[] eggSprites;
    // 0: 空フライパン
    // 1: 生卵が乗ったフライパン
    // 2: 目玉焼きが乗ったフライパン
    // 3: 目玉焼きが乗った皿

    [Header("HashBrown")]
    [SerializeField] private Image hashBrownPanImage;
    [SerializeField] private Sprite[] hashBrownSprites;
    // 0: 空フライパン
    // 1: 片面焼けが乗ったフライパン
    // 2: 両面焼けが乗ったフライパン
    // 3: 両面焼けが乗った皿

    [Header("Toast")]
    [SerializeField] private Image toasterImage;
    [SerializeField] private Sprite[] toasterSprites; // 0: 生2枚入り, 1: 焼け2枚入り
    [SerializeField] private Image toastImage1;       // 取り出し後・バター担当
    [SerializeField] private Image toastImage2;       // 取り出し後・ジャム担当
    [SerializeField] private Sprite[] toastSprites;   // 0: 焼けたトースト全体, 1: バター, 2: ジャム

    [Header("OJ")]
    [SerializeField] private Image ojImage;
    [SerializeField] private Sprite[] ojSprites; // 0: 空グラス, 1: OJ入り

    void Start()
    {
        InitializeImages();
        var token = this.GetCancellationTokenOnDestroy();
        Cooking(token).Forget();
    }

    void Update() { }

    void InitializeImages()
    {
        coffeeImage.sprite = coffeeSprites[0];
        eggPanImage.sprite = eggSprites[0];
        hashBrownPanImage.sprite = hashBrownSprites[0];
        ojImage.sprite = ojSprites[0];

        toasterImage.gameObject.SetActive(true);
        toasterImage.sprite = toasterSprites[0];
        toastImage1.gameObject.SetActive(false);
        toastImage2.gameObject.SetActive(false);
    }

    async UniTask Cooking(CancellationToken token)
    {
        Coffee cup = PourCoffee();
        Debug.Log("coffee is ready");

        UniTask<Egg> eggsTask = FryEggsAsync(2, token);
        UniTask<HashBrown> hashBrownTask = FryHashBrownsAsync(3, token);
        UniTask<Toast[]> toastTask = ToastBreadAsync(2, token);

        Toast[] toasts = await toastTask;
        ApplyButter(toasts[0]);
        ApplyJam(toasts[1]);
        Debug.Log("toast is ready");

        Juice oj = PourOJ();
        Debug.Log("oj is ready");

        Egg eggs = await eggsTask;
        Debug.Log("eggs are ready");

        HashBrown hashBrown = await hashBrownTask;
        Debug.Log("hash browns are ready");

        Debug.Log("Breakfast is ready!");
    }

    private Coffee PourCoffee()
    {
        Debug.Log("Pouring coffee");
        coffeeImage.sprite = coffeeSprites[1];
        return new Coffee();
    }

    private Juice PourOJ()
    {
        Debug.Log("Pouring orange juice");
        ojImage.sprite = ojSprites[1];
        return new Juice();
    }

    private void ApplyButter(Toast toast)
    {
        Debug.Log("Putting butter on the toast");
        toastImage1.sprite = toastSprites[1];
    }

    private void ApplyJam(Toast toast)
    {
        Debug.Log("Putting jam on the toast");
        toastImage2.sprite = toastSprites[2];
    }

    private async UniTask<Toast[]> ToastBreadAsync(int slices, CancellationToken token)
    {
        toasterImage.gameObject.SetActive(true);
        toasterImage.sprite = toasterSprites[0]; // 生2枚入り
        toastImage1.gameObject.SetActive(false);
        toastImage2.gameObject.SetActive(false);

        for (int slice = 0; slice < slices; slice++)
            Debug.Log("Putting a slice of bread in the toaster");

        Debug.Log("Start toasting...");
        await UniTask.Delay(3000, cancellationToken: token);

        toasterImage.sprite = toasterSprites[1]; // 焼け2枚入り
        Debug.Log("Remove toast from toaster");
        await UniTask.Delay(1000, cancellationToken: token);

        toasterImage.gameObject.SetActive(false);
        toastImage1.gameObject.SetActive(true);
        toastImage2.gameObject.SetActive(true);
        toastImage1.sprite = toastSprites[0];
        toastImage2.sprite = toastSprites[0];

        return Enumerable.Range(0, slices).Select(_ => new Toast()).ToArray();
    }

    private async UniTask<HashBrown> FryHashBrownsAsync(int patties, CancellationToken token)
    {
        hashBrownPanImage.sprite = hashBrownSprites[0]; // 空フライパン
        Debug.Log($"putting {patties} hash brown patties in the pan");

        Debug.Log("cooking first side of hash browns...");
        await UniTask.Delay(3000, cancellationToken: token);

        hashBrownPanImage.sprite = hashBrownSprites[1]; // 片面焼け
        for (int patty = 0; patty < patties; patty++)
            Debug.Log("flipping a hash brown patty");

        Debug.Log("cooking the second side of hash browns...");
        await UniTask.Delay(3000, cancellationToken: token);

        hashBrownPanImage.sprite = hashBrownSprites[2]; // 両面焼け
        Debug.Log("Put hash browns on plate");
        await UniTask.Delay(1000, cancellationToken: token);

        hashBrownPanImage.sprite = hashBrownSprites[3]; // 皿に盛る
        return new HashBrown();
    }

    private async UniTask<Egg> FryEggsAsync(int howMany, CancellationToken token)
    {
        eggPanImage.sprite = eggSprites[0]; // 空フライパン

        for (int i = 0; i < howMany; i++)
        {
            Debug.Log("Warming the egg pan...");
            await UniTask.Delay(3000, cancellationToken: token);

            Debug.Log($"cracking {i + 1} eggs");
            eggPanImage.sprite = eggSprites[1]; // 生卵が乗ったフライパン

            Debug.Log("cooking the eggs ...");
            await UniTask.Delay(3000, cancellationToken: token);
        }

        eggPanImage.sprite = eggSprites[2]; // 目玉焼きが乗ったフライパン
        Debug.Log("Put eggs on plate");
        await UniTask.Delay(1000, cancellationToken: token);

        eggPanImage.sprite = eggSprites[3]; // 皿に盛る
        return new Egg();
    }
}