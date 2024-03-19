using System.Collections.Generic;
using UnityEngine;
using Utils;
using UnityEngine.UI;
using TMPro;
using Utils.PoolSystem;
using Unity.Mathematics;
using DG.Tweening;

public class LootManager : Singleton<LootManager>
{
    public GameData data;
    public GamePresets gamePresets;
    public GameObject lootPanel, chestOpen, chestClose, step1, step2, step3, deck, spawnedObj;
    public Transform spawnPos, moveToPos, tempObj, gridView;
    public TMP_Text deckCount;
    public int remainingLootCount;
    public List<LootItem> currentLoot = new();
    public List<Transform> spawnedLootItems = new();

    void Start()
    {
        data = (GameData)DataPersistenceController.Instance.GetData("game", new GameData());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            NewLoot();
        }
    }

    public void NewLoot()
    {
        gridView.gameObject.SetActive(false);
        lootPanel.SetActive(true);
        TaptoCollect(false);
        currentLoot = gamePresets.LootList[data.lootCount].loot;
        remainingLootCount = currentLoot.Count;
        deckCount.text = remainingLootCount + "";
    }
    public void Skip()
    {
        gridView.gameObject.SetActive(true);
        step1.SetActive(false);
        step2.SetActive(false);
        chestClose.transform.parent.gameObject.SetActive(false);
        deck.SetActive(false);
        for (int i = 0; i < spawnedLootItems.Count; i++)
        {
            var obj = spawnedLootItems[i];
            obj.gameObject.SetActive(true);
            obj.SetParent(gridView);
            var rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 200);
        }
        step3.SetActive(true);
    }
    public void TaptoCollect(bool open)
    {
        step1.SetActive(!open);
        step2.SetActive(open);
        chestClose.SetActive(!open);
        chestOpen.SetActive(open);
        deck.SetActive(open);
        if (open) SpawnLootObject();
        else ClearList();
    }
    public void TapForNextItem()
    {
        if (remainingLootCount <= 0) { Skip(); return; }
        tempObj.gameObject.SetActive(false);
        SpawnLootObject();
    }
    public void Claim()
    {
        for (int i = 0; i < currentLoot.Count; i++)
        {

        }
        lootPanel.SetActive(false);
    }
    private void ClearList()
    {
        // if (spawnedLootItems.Count <= 0) return;
        for (int i = 0; i < spawnedLootItems.Count; i++)
        {
            spawnedLootItems[0].gameObject.Despawn();
            spawnedLootItems.RemoveAt(0);
        }
        // spawnedLootItems.Clear();
        if (spawnedLootItems.Count > 0)
            ClearList();
    }
    private void SpawnLootObject()
    {
        remainingLootCount--;
        deckCount.text = remainingLootCount + "";
        tempObj = spawnedObj.Spawn(spawnPos.position, quaternion.identity, lootPanel.transform).transform;
        tempObj.localScale = Vector3.one * 0.2f;
        var rectTransform = tempObj.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0);
        spawnedLootItems.Add(tempObj);
        var amountText = tempObj.GetChild(1).GetComponent<TMP_Text>();
        amountText.text = "";
        tempObj.DOScale(1.2f * Vector3.one, 1.6f).SetEase(Ease.InOutBounce);
        tempObj.DOMove(moveToPos.position, 1.6f, true).SetEase(Ease.InBounce).OnComplete(() => amountText.text = currentLoot[remainingLootCount].amount + "");
        tempObj.GetChild(0).GetComponent<Image>().sprite = currentLoot[remainingLootCount].sprite;

    }
}


// for (int i = 0; i < currentLoot.Count; i++)
// {
//     Debug.Log(currentLoot[i].name);
// }