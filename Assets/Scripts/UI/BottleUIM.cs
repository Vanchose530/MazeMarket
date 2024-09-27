using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottleUIM : MonoBehaviour
{
    [Header("Health Poision")]
    [SerializeField] private GameObject healthPoitionGameObject;
    [SerializeField] private TextMeshProUGUI healthPoitionCountTMP;

    [Header("Demons Blood Grenade")]
    [SerializeField] private GameObject demonsBloodGrenadeGameObject;
    [SerializeField] private TextMeshProUGUI demonsBloodGrenadeCountTMP;

    public void SetHealthPoisionCount(int count)
    {
        healthPoitionGameObject.SetActive(count != 0);
        healthPoitionCountTMP.text = count.ToString();
    }

    public void SetDemonsBloodGrenadeCount(int count)
    {
        demonsBloodGrenadeGameObject.SetActive(count != 0);
        demonsBloodGrenadeCountTMP.text = count.ToString();
    }
}
