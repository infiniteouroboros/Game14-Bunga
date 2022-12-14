using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CardPlayer : MonoBehaviour
{
    public Transform atkPosRef;
    public Card chosenCard;
    public HealthBar healthbar;
    [SerializeField] private TMP_Text nameText;
    public TMP_Text healthText;
    public float Health;
    public PlayerStats stats = new PlayerStats
    {
        MaxHealth = 100,
        RestoreValue = 5,
        DamageValue = 10
    };
    private Tweener animationTweener;

    public TMP_Text NickName { get => nameText; }

    public bool IsReady = false;
    private void Start()
    {
        Health = stats.MaxHealth;
    }

    public void SetStats(PlayerStats newStats, bool restoreFullHealth = false)
    {
        this.stats = newStats;
        if (restoreFullHealth)
            Health = stats.MaxHealth;

        UpdateHealthBar();
    }
    public Attack? AttackValue
    {
        get => chosenCard== null ? null : chosenCard.AttackValue;

    //    get
    //    {
    //        if (chosenCard == null)
    //            return null;
    //        else
    //            return chosenCard.AttackValue;
    //    }
    }

    public void Reset()
    {
        if(chosenCard != null)
        {
            chosenCard.Reset();
        }

        chosenCard = null;
    }
    public void SetChosenCard(Card newCard)
    {
        if(chosenCard != null)
        {
            chosenCard.transform.DOKill();
            chosenCard.Reset();
        }

        chosenCard = newCard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.2f, 0.5f);
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, stats.MaxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // health bar
        healthbar.UpdateBar(Health/stats.MaxHealth);
        // text
        healthText.text = Health + "/" + stats.MaxHealth;

    }
    public void AnimateAttack()
    {
        animationTweener = chosenCard.transform
            .DOMove(atkPosRef.position, 0.5f);
    }

    public void AnimateDamage()
    {
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(0.2f);
    }

    public void AnimateDraw()
    {
        animationTweener = chosenCard.transform
            .DOMove(chosenCard.OriginalPosition, 1)
            .SetEase(Ease.InBack)
            .SetDelay(0.2f);
    }
    public bool IsAnimating()
    {
        return animationTweener.IsActive(); 
    }

    public void IsClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetClickable(value);
        }
    }
}
