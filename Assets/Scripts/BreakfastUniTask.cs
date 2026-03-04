using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.UI;
using System.Linq;
using TMPro;

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

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI kitchenText;
    [SerializeField] private TextMeshProUGUI eggText;
    [SerializeField] private TextMeshProUGUI hashBrownText;
    [SerializeField] private TextMeshProUGUI toasterText;
    [SerializeField] private TextMeshProUGUI coffeeText;
    [SerializeField] private TextMeshProUGUI ojText;

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
        coffeeText.text="coffee is ready";

        UniTask<Egg> eggsTask = FryEggsAsync(2, token);
        UniTask<HashBrown> hashBrownTask = FryHashBrownsAsync(3, token);
        UniTask<Toast[]> toastTask = ToastBreadAsync(2, token);

        Toast[] toasts = await toastTask;
        ApplyButter(toasts[0]);
        ApplyJam(toasts[1]);
        toasterText.text="toast is ready";

        Juice oj = PourOJ();
        ojText.text="oj is ready";

        Egg eggs = await eggsTask;
        eggText.text="eggs are ready";

        HashBrown hashBrown = await hashBrownTask;
        hashBrownText.text="hash browns are ready";

        kitchenText.text="Breakfast is ready!";
    }

    private Coffee PourCoffee()
    {
        coffeeText.text="Pouring coffee";
        coffeeImage.sprite = coffeeSprites[1];
        return new Coffee();
    }

    private Juice PourOJ()
    {
        ojText.text="Pouring orange juice";
        ojImage.sprite = ojSprites[1];
        return new Juice();
    }

    private void ApplyButter(Toast toast)
    {
        toasterText.text="Putting butter on the toast";
        toastImage1.sprite = toastSprites[1];
    }

    private void ApplyJam(Toast toast)
    {
        toasterText.text="Putting jam on the toast";
        toastImage2.sprite = toastSprites[2];
    }

    private async UniTask<Toast[]> ToastBreadAsync(int slices, CancellationToken token)
    {
        toasterImage.gameObject.SetActive(true);
        toasterImage.sprite = toasterSprites[0]; // 生2枚入り
        toastImage1.gameObject.SetActive(false);
        toastImage2.gameObject.SetActive(false);

        for (int slice = 0; slice < slices; slice++)
            toasterText.text="Putting a slice of bread in the toaster";

        toasterText.text="Start toasting...";
        await UniTask.Delay(3000, cancellationToken: token);

        toasterImage.sprite = toasterSprites[1]; // 焼け2枚入り
        toasterText.text="Remove toast from toaster";
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
        hashBrownText.text=$"putting {patties} hash brown patties in the pan";

        hashBrownText.text="cooking first side of hash browns...";
        hashBrownPanImage.sprite = hashBrownSprites[1]; // 片面焼け
        await UniTask.Delay(3000, cancellationToken: token);
        
        for (int patty = 0; patty < patties; patty++)
            hashBrownText.text="flipping a hash brown patty";

        hashBrownText.text="cooking the second side of hash browns...";
        await UniTask.Delay(3000, cancellationToken: token);

        hashBrownPanImage.sprite = hashBrownSprites[2]; // 両面焼け
        hashBrownText.text="Put hash browns on plate";
        await UniTask.Delay(1000, cancellationToken: token);

        hashBrownPanImage.sprite = hashBrownSprites[3]; // 皿に盛る
        return new HashBrown();
    }

    private async UniTask<Egg> FryEggsAsync(int howMany, CancellationToken token)
    {
        eggPanImage.sprite = eggSprites[0]; // 空フライパン

        for (int i = 0; i < howMany; i++)
        {
            eggText.text="Warming the egg pan...";
            await UniTask.Delay(3000, cancellationToken: token);

            eggText.text=$"cracking {i + 1} eggs";
            eggPanImage.sprite = eggSprites[1]; // 生卵が乗ったフライパン

            eggText.text="cooking the eggs ...";
            await UniTask.Delay(3000, cancellationToken: token);
        }

        eggPanImage.sprite = eggSprites[2]; // 目玉焼きが乗ったフライパン
        eggText.text="Put eggs on plate";
        await UniTask.Delay(1000, cancellationToken: token);

        eggPanImage.sprite = eggSprites[3]; // 皿に盛る
        return new Egg();
    }
}