using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PurchaseUpgrade : MonoBehaviour
{
    public bool UpgradePrerequisite;
    public bool UpgradeBranch;
    public GameObject[] subsequentUpgradeButton;
    public GameObject[] rivalupgrades;
     
    UpgradeManager uManager;
    public int purchaseID;
    public GameObject buymark;

    private void Awake()
    {
        uManager = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<UpgradeManager>();
        

    }

    public void PawnUpgrade()
    {
        if (uManager.PawnUpgradeCosts[purchaseID] <= uManager.zombieBucks && uManager.PawnPurchaseStatus[purchaseID] == false)
        {
            uManager.PawnUpgrade(purchaseID);

            if(UpgradeBranch == true)
            {
                foreach(GameObject rivalup in rivalupgrades)
                {
                    rivalup.SetActive(false);
                }
            }
            if(UpgradePrerequisite == true)
            {
                foreach(GameObject subsequentupgrades in subsequentUpgradeButton)
                {
                    subsequentupgrades.SetActive(true);
                }
            }
            //disable purchase button
            buymark.SetActive(true);
        }else if (uManager.PawnUpgradeCosts[purchaseID] > uManager.zombieBucks)
        {
            notEnoughMoney();
        }
    }

    public void RookUpgrade()
    {
        if (uManager.RookUpgradeCosts[purchaseID] <= uManager.zombieBucks && uManager.RookPurchaseStatus[purchaseID] == false)
        {
            uManager.RookUpgrade(purchaseID);
            if (UpgradePrerequisite == true)
            {
                foreach (GameObject subsequentupgrades in subsequentUpgradeButton)
                {
                    subsequentupgrades.SetActive(true);
                }
            }
            if (UpgradeBranch == true)
            {
                foreach (GameObject rivalup in rivalupgrades)
                {
                    rivalup.SetActive(false);
                }
            }


            //disable purchase button
            buymark.SetActive(true);
        }
        else if (uManager.RookUpgradeCosts[purchaseID] > uManager.zombieBucks)
        {
            notEnoughMoney();
        }
    }

    public void KnightUpgrade()
    {
        if (uManager.KnightUpgradeCosts[purchaseID] <= uManager.zombieBucks && uManager.KnightPurchaseStatus[purchaseID] == false)
        {
            uManager.KnightUpgrade(purchaseID);
            if (UpgradePrerequisite == true)
            {
                foreach (GameObject subsequentupgrades in subsequentUpgradeButton)
                {
                    subsequentupgrades.SetActive(true);
                }
            }

            if (UpgradeBranch == true)
            {
                foreach (GameObject rivalup in rivalupgrades)
                {
                    rivalup.SetActive(false);
                }
            }
            //disable purchase button
            buymark.SetActive(true);
        }
        else if (uManager.KnightUpgradeCosts[purchaseID] > uManager.zombieBucks)
        {
            notEnoughMoney();
        }
    }

    public void BishopUpgrade()
    {
        if (uManager.BishopUpgradeCosts[purchaseID] <= uManager.zombieBucks && uManager.BishopPurchaseStatus[purchaseID] == false)
        {
            uManager.BishopUpgrade(purchaseID);
            if (UpgradePrerequisite == true)
            {
                foreach (GameObject subsequentupgrades in subsequentUpgradeButton)
                {
                    subsequentupgrades.SetActive(true);
                }
            }

            if (UpgradeBranch == true)
            {
                foreach (GameObject rivalup in rivalupgrades)
                {
                    rivalup.SetActive(false);
                }
            }
            //disable purchase button
            buymark.SetActive(true);
        }
        else if (uManager.BishopUpgradeCosts[purchaseID] > uManager.zombieBucks)
        {
            notEnoughMoney();
        }
    }

    public void QueenUpgrade()
    {

        
        if (uManager.QueenUpgradeCosts[purchaseID] <= uManager.zombieBucks && uManager.QueenPurchaseStatus[purchaseID] == false)
        {
            uManager.QueenUpgrade(purchaseID);
            if (UpgradePrerequisite == true)
            {
                foreach (GameObject subsequentupgrades in subsequentUpgradeButton)
                {
                    subsequentupgrades.SetActive(true);
                }
            }

            if (UpgradeBranch == true)
            {
                foreach (GameObject rivalup in rivalupgrades)
                {
                    rivalup.SetActive(false);
                }
            }
            //disable purchase button
            buymark.SetActive(true);
        }
        else if (uManager.QueenUpgradeCosts[purchaseID] > uManager.zombieBucks)
        {
            notEnoughMoney();
        }
    }
    public void KingUpgrade()
    {
        if (uManager.KingUpgradeCosts[purchaseID] <= uManager.zombieBucks && uManager.KingPurchaseStatus[purchaseID] == false)
        {
            uManager.KingUpgrade(purchaseID);
            if (UpgradePrerequisite == true)
            {
                foreach (GameObject subsequentupgrades in subsequentUpgradeButton)
                {
                    subsequentupgrades.SetActive(true);
                }
            }

            if (UpgradeBranch == true)
            {
                foreach (GameObject rivalup in rivalupgrades)
                {
                    rivalup.SetActive(false);
                }
            }

            //disable purchase button
            buymark.SetActive(true);
        }
        else if (uManager.KingUpgradeCosts[purchaseID] > uManager.zombieBucks)
        {
            notEnoughMoney();
        }
    }

    void notEnoughMoney()
    {
        // do not enough money stuff
    }

}
