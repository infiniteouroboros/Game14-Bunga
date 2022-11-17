using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{

    public CardPlayer player;
    public CardGameManager CardgameManager;

    public BotStats stats;

   

    private float timer = 0;

    int lastSelected = 0;

    Card[] cards;
    public bool IsReady = false;

    public void SetStats(BotStats newStats, bool restoreFullHealth = false)
    {
        this.stats = newStats;

        var newPlayerStats = new PlayerStats
        {
            MaxHealth = this.stats.MaxHealth,
            RestoreValue = this.stats.RestoreValue,
            DamageValue = this.stats.DamageValue,

        };

        player.SetStats(newPlayerStats, restoreFullHealth);
    }

    IEnumerator Start()
    {
        cards = player.GetComponentsInChildren<Card>();

        yield return new WaitUntil(() => player.IsReady);
        SetStats(this.stats);
        this.IsReady = true;
    }

    void Update()
    {
        if (CardgameManager.State != CardGameManager.GameState.ChooseAttack)
        {
            timer = 0;
            return;
        }
        if (timer < stats.ChoosingInterval)
        {
            timer += Time.deltaTime;
            return;
        }

        timer = 0;
        ChooseAttack();
    }

    public void ChooseAttack()
    {
        var random = Random.Range(1, cards.Length);
        var selection = (lastSelected + random) % cards.Length;
        lastSelected = selection;
        player.SetChosenCard(cards[selection]);
    }
}
