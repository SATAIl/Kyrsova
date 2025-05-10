// File: Assets/Scripts/CollectionUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionUI : MonoBehaviour
{
    [Header("UI-елементи")]
    public TextMeshProUGUI coinsText;    // Текст балансу монет
    public Button           buyButton;    // Кнопка «Купити»
    public Button           backButton;   // Кнопка «Назад»

    [Header("Сцена")]
    public SceneLoader      sceneLoader;  // Посилання на твій SceneLoader (через інспектор)

    [Header("Контейнери слотів")]
    public RectTransform    swipeContainer; // Контейнер для свайп-скінів
    public RectTransform    blockContainer; // Контейнер для блок-скінів

    [Header("Prefabs")]
    public GameObject       skinButtonPrefab; // Префаб однієї кнопки-слоту

    // Тимчасові змінні для покупки
    private int   pendingIndex;
    private bool  pendingSwipe;

    void Start()
    {
        // 1) Показати поточний баланс
        coinsText.text = CurrencyManager.Balance.ToString();

        // 2) Прив’язати обробники кліків
        buyButton.onClick .AddListener(OnBuy);
        backButton.onClick.AddListener(OnBack);

        // 3) Побудувати слоти
        Populate(swipeContainer, true);
        Populate(blockContainer, false);

        // 4) Вимкнути кнопку купити поки нічого не обрано
        buyButton.interactable = false;
    }

    /// <summary>
    /// Створює кнопки-слоти в заданому контейнері.
    /// </summary>
    void Populate(RectTransform container, bool isSwipe)
    {
        var list   = isSwipe
            ? SkinManager.Instance.SwipeSkins
            : SkinManager.Instance.BlockSkins;
        int active = isSwipe
            ? SkinManager.Instance.activeSwipeSkin
            : SkinManager.Instance.activeBlockSkin;

        // Очистити попередні слоти
        foreach (Transform child in container)
            Destroy(child.gameObject);

        // Інстансувати нові слоти
        for (int i = 0; i < list.Count; i++)
        {
            var si  = list[i];
            var btn = Instantiate(skinButtonPrefab, container);

            // Іконка
            btn.transform
               .Find("Icon")
               .GetComponent<Image>()
               .sprite = si.icon;

            // Ціна або USED
            var priceText = btn.transform
                               .Find("PriceText")
                               .GetComponent<TextMeshProUGUI>();
            priceText.text = si.isUnlocked
                ? "USED"
                : si.price.ToString();

            // Підсвітка активного слоту
            var img = btn.GetComponent<Image>();
            img.color = (i == active)
                ? Color.yellow
                : Color.white;

            // Прив’язати обробник кліку
            int idx = i; // щоб уникнути "замикання" змінної
            btn.GetComponent<Button>()
               .onClick
               .AddListener(() => OnSlotClicked(isSwipe, idx));
        }
    }

    /// <summary>
    /// Обробка кліку по одному із слотів.
    /// Якщо розблоковано — одразу застосовує,
    /// якщо ні — готує покупку.
    /// </summary>
    void OnSlotClicked(bool isSwipe, int idx)
    {
        var list = isSwipe
            ? SkinManager.Instance.SwipeSkins
            : SkinManager.Instance.BlockSkins;
        var si   = list[idx];

        if (si.isUnlocked)
        {
            // Змінити активний індекс і застосувати
            if (isSwipe) SkinManager.Instance.activeSwipeSkin = idx;
            else         SkinManager.Instance.activeBlockSkin = idx;

            SkinManager.Instance.SaveData();
            GameSkinApplier.Instance.UpdateSkin();
            Refresh();
            return;
        }

        // Підготувати покупку
        pendingSwipe = isSwipe;
        pendingIndex = idx;
        buyButton.interactable = true;
        buyButton.GetComponentInChildren<TextMeshProUGUI>()
                 .text = $"Купити за {si.price}";
    }

    /// <summary>
    /// Обробка кліку на кнопку «Купити».
    /// Спробує списати кошти та розблокує скін.
    /// </summary>
    void OnBuy()
    {
        var list = pendingSwipe
            ? SkinManager.Instance.SwipeSkins
            : SkinManager.Instance.BlockSkins;
        var si   = list[pendingIndex];

        // Спроба списати кошти
        if (!CurrencyManager.TrySpend(si.price))
            return;

        // Оновити UI-баланс
        coinsText.text = CurrencyManager.Balance.ToString();

        // Розблокувати та застосувати скин
        si.isUnlocked = true;
        if (pendingSwipe) SkinManager.Instance.activeSwipeSkin = pendingIndex;
        else              SkinManager.Instance.activeBlockSkin = pendingIndex;

        SkinManager.Instance.SaveData();
        GameSkinApplier.Instance.UpdateSkin();
        Refresh();
    }

    /// <summary>
    /// Обробка кліку на кнопку «Назад».
    /// Викликає твій існуючий SceneLoader через інспектор.
    /// </summary>
    void OnBack()
    {
        sceneLoader.LoadScene("MainMenu");
    }

    /// <summary>
    /// Перемалювати UI та вимкнути кнопку «Купити».
    /// </summary>
    void Refresh()
    {
        Populate(swipeContainer, true);
        Populate(blockContainer, false);
        buyButton.interactable = false;
    }
}
