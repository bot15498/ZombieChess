using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMGrouping : MonoBehaviour
{
    public CinemachineTargetGroup targetgroup;

    void Start()
    {
        targetgroup = GetComponent<CinemachineTargetGroup>();
        
    }

 
    public void CheckNewGroup()
    {
        targetgroup.m_Targets = BoardStateManager.current.board.allPieces.Values.Where(x => x.owner == CurrentTurn.Zombie).Select(x => new CinemachineTargetGroup.Target { target = x.transform, radius = 1f, weight = 0f }).ToArray();


    }



}
