using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{

    public int zombieBucks;

    public static UpgradeManager current;
    [Header("============Upgrade Costs Settings============")]
    [Space(10)] // 10 pixels of spacing here.
    public int[] PawnUpgradeCosts;
    public int[] RookUpgradeCosts;
    public int[] KnightUpgradeCosts;
    public int[] BishopUpgradeCosts;
    public int[] QueenUpgradeCosts;
    public int[] KingUpgradeCosts;

    [Space(10)] // 10 pixels of spacing here.
    [Header("============Upgrade Status Settings============")]
    [Space(10)] // 10 pixels of spacing here.
    public bool[] PawnPurchaseStatus;
    public bool[] RookPurchaseStatus;
    public bool[] KnightPurchaseStatus;
    public bool[] BishopPurchaseStatus;
    public bool[] QueenPurchaseStatus;
    public bool[] KingPurchaseStatus;

    [Space(10)] // 10 pixels of spacing here.
    [Header("============Upgrade Status Settings============")]
    [Space(10)] // 10 pixels of spacing here.
    public AudioSource upgradeAudioController;
    public AudioClip buyUpgrade;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> ActivatePawnUpgrade;
    public event Action<int> ActivateRookUpgrade;
    public event Action<int> ActivateKnightUpgrade;
    public event Action<int> ActivateBishopUpgrade;
    public event Action<int> ActivateQueenUpgrade;
    public event Action<int> ActivateKingUpgrade;

    public void PawnUpgrade(int UpgradeID)
    {
        zombieBucks -= PawnUpgradeCosts[UpgradeID];
        PawnPurchaseStatus[UpgradeID] = true;
        this.upgradeAudioController.PlayOneShot(this.buyUpgrade, 1.0f);

            if (ActivatePawnUpgrade != null)
            {
                ActivatePawnUpgrade(UpgradeID);
                    
            }
        
    }

    public void RookUpgrade(int UpgradeID)
    {
        zombieBucks -= RookUpgradeCosts[UpgradeID];
        RookPurchaseStatus[UpgradeID] = true;
        this.upgradeAudioController.PlayOneShot(this.buyUpgrade, 1.0f);

        if (ActivateRookUpgrade != null)
        {
            ActivateRookUpgrade(UpgradeID);

        }
    }

    public void KnightUpgrade(int UpgradeID)
    {
        zombieBucks -= KnightUpgradeCosts[UpgradeID];
        KnightPurchaseStatus[UpgradeID] = true;
        this.upgradeAudioController.PlayOneShot(this.buyUpgrade, 1.0f);

        if (ActivateKnightUpgrade != null)
        {
            ActivateKnightUpgrade(UpgradeID);

        }
    }

    public void BishopUpgrade(int UpgradeID)
    {
        zombieBucks -= BishopUpgradeCosts[UpgradeID];
        BishopPurchaseStatus[UpgradeID] = true;
        this.upgradeAudioController.PlayOneShot(this.buyUpgrade, 1.0f);

        if (ActivateBishopUpgrade != null)
        {
            ActivateBishopUpgrade(UpgradeID);

        }
    }

    public void QueenUpgrade(int UpgradeID)
    {
        zombieBucks -= QueenUpgradeCosts[UpgradeID];
        QueenPurchaseStatus[UpgradeID] = true;
        this.upgradeAudioController.PlayOneShot(this.buyUpgrade, 1.0f);

        if (ActivateQueenUpgrade != null)
        {
            ActivateQueenUpgrade(UpgradeID);

        }
    }
    public void KingUpgrade(int UpgradeID)
    {
        zombieBucks -= KingUpgradeCosts[UpgradeID];
        KingPurchaseStatus[UpgradeID] = true;
        this.upgradeAudioController.PlayOneShot(this.buyUpgrade, 1.0f);

        if (ActivateKingUpgrade != null)
        {
            ActivateKingUpgrade(UpgradeID);

        }
    }

    public void addMoney(int moneytoadd)
    {
        zombieBucks += moneytoadd;
    }

}
