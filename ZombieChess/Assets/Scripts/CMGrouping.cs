using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMGrouping : MonoBehaviour
{
    public CinemachineTargetGroup targetgroup;

    void Start()
    {
        targetgroup = GetComponent<CinemachineTargetGroup>();
        
    }

    public void addZombietoGroup(Transform zombie)
    {
        targetgroup.AddMember(zombie, 1f, 0f);
    }

    public void emptytargetgroup(Transform zombie)
    {
        targetgroup.RemoveMember(zombie);

    }

}
