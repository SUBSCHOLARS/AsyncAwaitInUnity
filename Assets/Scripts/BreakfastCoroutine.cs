using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BreakfastCoroutine : MonoBehaviour
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
        StartCoroutine(Cooking());
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

    private IEnumerator Cooking()
    {
        Coffee cup = PourCoffee();
        coffeeText.text="coffee is ready";

        yield return StartCoroutine(FryEggs(2));
        eggText.text="eggs are ready";

        yield return StartCoroutine(FryHashBrowns(3));
        hashBrownText.text="hash browns are ready";

        yield return StartCoroutine(ToastBread(2));

        ApplyButter();
        ApplyJam();
        toasterText.text="toast is ready";

        Juice oj = PourOJ();
        ojText.text="oj is ready";

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

    private void ApplyButter()
    {
        toasterText.text="Putting butter on the toast";
        toastImage1.sprite = toastSprites[1];
    }

    private void ApplyJam()
    {
        toasterText.text="Putting jam on the toast";
        toastImage2.sprite = toastSprites[2];
    }

    private IEnumerator ToastBread(int slices)
    {
        toasterImage.gameObject.SetActive(true);
        toasterImage.sprite = toasterSprites[0]; // 生2枚入り
        toastImage1.gameObject.SetActive(false);
        toastImage2.gameObject.SetActive(false);

        for (int slice = 0; slice < slices; slice++)
            toasterText.text="Putting a slice of bread in the toaster";

        toasterText.text="Start toasting...";
        yield return new WaitForSeconds(3f);

        toasterImage.sprite = toasterSprites[1]; // 焼け2枚入り
        toasterText.text="Remove toast from toaster";
        yield return new WaitForSeconds(3f);

        toasterImage.gameObject.SetActive(false);
        toastImage1.gameObject.SetActive(true);
        toastImage2.gameObject.SetActive(true);
        toastImage1.sprite = toastSprites[0];
        toastImage2.sprite = toastSprites[0];
    }

    private IEnumerator FryHashBrowns(int patties)
    {
        hashBrownPanImage.sprite = hashBrownSprites[0]; // 空フライパン
        hashBrownText.text=$"putting {patties} hash brown patties in the pan";

        hashBrownText.text="cooking first side of hash browns...";
        hashBrownPanImage.sprite = hashBrownSprites[1]; // 片面焼け
        yield return new WaitForSeconds(3f);
        
        for (int patty = 0; patty < patties; patty++)
            hashBrownText.text="flipping a hash brown patty";

        hashBrownText.text="cooking the second side of hash browns...";
        yield return new WaitForSeconds(3f);

        hashBrownPanImage.sprite = hashBrownSprites[2]; // 両面焼け
        hashBrownText.text="Put hash browns on plate";
        yield return new WaitForSeconds(3f);

        hashBrownPanImage.sprite = hashBrownSprites[3]; // 皿に盛る
    }

    private IEnumerator FryEggs(int howMany)
    {
        eggPanImage.sprite = eggSprites[0]; // 空フライパン

        for (int i = 0; i < howMany; i++)
        {
            eggText.text="Warming the egg pan...";
            yield return new WaitForSeconds(3f);

            eggText.text=$"cracking {i + 1} eggs";
            eggPanImage.sprite = eggSprites[1]; // 生卵が乗ったフライパン

            eggText.text="cooking the eggs ...";
            yield return new WaitForSeconds(3f);
        }

        eggPanImage.sprite = eggSprites[2]; // 目玉焼きが乗ったフライパン
        eggText.text="Put eggs on plate";
        yield return new WaitForSeconds(3f);

        eggPanImage.sprite = eggSprites[3]; // 皿に盛る
    }
}