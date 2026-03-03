using UnityEngine;
using UnityEngine.UI;

public class BreakfastUniTaskView : MonoBehaviour
{
    [Header("Coffee")]
    [SerializeField] private Image coffeeImage;
    [SerializeField] private Sprite[] coffeeSprites; // 0: 空, 1: 満タン

    [Header("Egg")]
    [SerializeField] private Image eggPanImage;
    [SerializeField] private Sprite[] eggSprites; // 0: 空フライパン、1: 生卵, 2: 目玉焼き, 3: 空皿

    [Header("HashBrown")]
    [SerializeField] private Image hashBrownPanImage;
    [SerializeField] private Sprite[] hasBrwonSprites; // 0: 空フライパン、1: 片面, 2: 両面, 3: 空皿

    [Header("Toast")]
    [SerializeField] private Image toastImage1; // バター担当
    [SerializeField] private Image toastImage2; // ジャム担当
    [SerializeField] private Sprite[] toastSprites; // 0: トースター生、1: トースター焼け, 2: 全体, 3: バター, 4: ジャム

    [Header("OJ")]
    [SerializeField] private Image ojImage;
    [SerializeField] private Sprite[] ojSprites; // 0: 空グラス, 1: OJ入り

}
