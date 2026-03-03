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
    [SerializeField] private Sprite[] eggSprites; // 0: 空フライパン、1: 生卵, 2: 目玉焼き, 3: 空皿
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private Transform eggPanTransform;

    [Header("HashBrown")]
    [SerializeField] private Image hashBrownPanImage;
    [SerializeField] private Sprite[] hashBrownSprites; // 0: 空フライパン, 1: 両面焼け, 2: 空皿
    [SerializeField] private GameObject hashBrownPrefab;
    [SerializeField] private Sprite[] hashBrownPrefabSprites; // 0: 片面, 1: 両面（Prefab内のImageに適用）
    [SerializeField] private Transform hashBrownPanTransform;

    [Header("Toast")]
    [SerializeField] private Image toasterImage;   // トースター全体（2枚入り）
    [SerializeField] private Sprite[] toasterSprites; // 0: 生2枚入り, 1: 焼け2枚入り
    [SerializeField] private Image toastImage1;    // 取り出し後・バター担当
    [SerializeField] private Image toastImage2;    // 取り出し後・ジャム担当
    [SerializeField] private Sprite[] toastSprites; // 0: 焼けたトースト全体, 1: バター, 2: ジャム

    [Header("OJ")]
    [SerializeField] private Image ojImage;
    [SerializeField] private Sprite[] ojSprites; // 0: 空グラス, 1: OJ入り
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeImages();
        var token=this.GetCancellationTokenOnDestroy();
        Cooking(token).Forget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeImages()
    {
        coffeeImage.sprite = coffeeSprites[0];
        eggPanImage.sprite = eggSprites[0];
        hashBrownPanImage.sprite = hashBrownSprites[0];
        ojImage.sprite = ojSprites[0];

        // トースターは最初から表示、取り出し後のトーストは非表示
        toasterImage.gameObject.SetActive(true);
        toasterImage.sprite = toasterSprites[0];
        toastImage1.gameObject.SetActive(false);
        toastImage2.gameObject.SetActive(false);
    }
    async UniTask Cooking(CancellationToken token)
    {
        Coffee cup=PourCoffee();
        Debug.Log("coffee is ready");

        // 各非同期メソッドにtokenは渡すこと。そうしないとUniTaskの最適化の恩恵を受けられない可能性がある。
        UniTask<Egg> eggsTask=FryEggsAsync(2, token);
        UniTask<HashBrown> hashBrownTask=FryHashBrownsAsync(3, token);
        UniTask<Toast[]> toastTask=ToastBreadAsync(2, token);

        Toast[] toasts=await toastTask;
        ApplyButter(toasts[0]);
        ApplyJam(toasts[1]);
        Debug.Log("toast is ready");

        Juice oj=PourOJ();
        Debug.Log("oj is ready");

        Egg eggs=await eggsTask;
        Debug.Log("eggs are ready");
        HashBrown hashBrown=await hashBrownTask;
        Debug.Log("hash browns are ready");

        Debug.Log("Breakfast is ready!");
    }
    private Juice PourOJ()
    {
        Debug.Log("Pouring orange juice");
        ojImage.sprite=ojSprites[1];
        return new Juice();
    }

    private void ApplyJam(Toast toast)
    {
        Debug.Log("Putting jam on the toast");
        toastImage2.sprite=toastSprites[2];
    }

    private void ApplyButter(Toast toast)
    {
        Debug.Log("Putting butter on the toast");
        toastImage1.sprite=toastSprites[1];
    }

        private async UniTask<Toast[]> ToastBreadAsync(int slices, CancellationToken token)
        {
            // トースターに生トーストを入れた絵に変更
            toasterImage.gameObject.SetActive(true);
            toasterImage.sprite = toasterSprites[0]; // 生2枚入り
            toastImage1.gameObject.SetActive(false);
            toastImage2.gameObject.SetActive(false);

            for (int slice = 0; slice < slices; slice++)
            {
                Debug.Log("Putting a slice of bread in the toaster");
            }
            Debug.Log("Start toasting...");
            await UniTask.Delay(3000, cancellationToken: token);

            // 焼けた状態に変更
            toasterImage.sprite = toasterSprites[1]; // 焼け2枚入り
            Debug.Log("Remove toast from toaster");

            // トースターを非表示にして取り出したトーストを表示
            toasterImage.gameObject.SetActive(false);
            toastImage1.gameObject.SetActive(true);
            toastImage2.gameObject.SetActive(true);
            toastImage1.sprite = toastSprites[0]; // 焼けたトースト全体
            toastImage2.sprite = toastSprites[0];

            return Enumerable.Range(0, slices).Select(_ => new Toast()).ToArray();
        }

        private async UniTask<HashBrown> FryHashBrownsAsync(int patties, CancellationToken token)
        {
            hashBrownPanImage.sprite = hashBrownSprites[0]; // 空フライパン

            Debug.Log($"putting {patties} hash brown patties in the pan");
            // パティをInstantiate
            var pattyObjects = new GameObject[patties];
            for (int i = 0; i < patties; i++)
            {
                pattyObjects[i] = Object.Instantiate(hashBrownPrefab, hashBrownPanTransform);
                pattyObjects[i].transform.localPosition = new Vector3(i * 80f, 0f, 0f);
                // 片面スプライトを設定
                pattyObjects[i].GetComponent<Image>().sprite = hashBrownPrefabSprites[0];
            }
            Debug.Log("cooking first side of hash browns...");
            await UniTask.Delay(3000, cancellationToken: token);

            // 裏返し：全パティのスプライトを両面焼けに差し替え
            foreach (var patty in pattyObjects)
                patty.GetComponent<Image>().sprite = hashBrownPrefabSprites[1];

            Debug.Log("cooking the second side of hash browns...");
            await UniTask.Delay(3000, cancellationToken: token);
            // 皿に盛る：Instantiateしたオブジェクトを削除、フライパンを空皿に
            foreach (var patty in pattyObjects)
                Object.Destroy(patty);
            hashBrownPanImage.sprite = hashBrownSprites[2];
            Debug.Log("Put hash browns on plate");

            return new HashBrown();
        }

        private async UniTask<Egg> FryEggsAsync(int howMany, CancellationToken token)
        {
            eggPanImage.sprite = eggSprites[0]; // 空フライパン
            for(int i=0; i<howMany; i++)
            {
                Debug.Log("Warming the egg pan...");
                await UniTask.Delay(3000, cancellationToken: token);

                // 生卵をフライパン上にInstantiate
                Debug.Log($"cracking {i+1} eggs");
                var eggObj = Object.Instantiate(eggPrefab, eggPanTransform);
                // 卵を横に並べるためにX座標をずらす
                eggObj.transform.localPosition = new Vector3(i * 80f, 0f, 0f);

                Debug.Log("cooking the eggs ...");
                await UniTask.Delay(3000, cancellationToken: token);
            }
            // 全卵が焼けたら目玉焼きスプライトに差し替え、Instantiateした卵を削除
            eggPanImage.sprite = eggSprites[1];
            foreach (Transform child in eggPanTransform)
                Object.Destroy(child.gameObject);

        Debug.Log("Put eggs on plate");
        eggPanImage.sprite = eggSprites[2]; // 皿に盛る

            return new Egg();
        }

        private Coffee PourCoffee()
        {
            Debug.Log("Pouring coffee");
            coffeeImage.sprite=coffeeSprites[1];
            return new Coffee();
        }
}
