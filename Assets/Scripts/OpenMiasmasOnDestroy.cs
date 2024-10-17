using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMiasmasOnDestroy : MonoBehaviour
{
    [Header("Miasmas")]
    [SerializeField] private List<LockMiasma> lockMiasmas;
    [SerializeField] private bool activateMiasmasOnStart = true;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var m in lockMiasmas)
            m.Lock();
    }
    private void OnDestroy()
    {
        foreach (var m in lockMiasmas)
            m.Unlock();
    }
}
