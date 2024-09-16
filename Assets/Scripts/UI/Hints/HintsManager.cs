using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsManager : MonoBehaviour
{
    // Loot Hint
    public void ShowLootHint(string lootText, float time = 2)
    {
        HintsUIM.instance.lootHint?.Show(lootText);
        StartCoroutine(HideLootHintAfterTime(time));
    }

    IEnumerator HideLootHintAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        HintsUIM.instance.lootHint?.Hide();
    }

    // Default Notice

    public void ShowDefaultNotice(string hintText)
    {
        HintsUIM.instance.defaultNotice?.Show(hintText);
    }

    public void ShowDefaultNotice(string hintText, float time)
    {
        HintsUIM.instance.defaultNotice?.Show(hintText);
        StartCoroutine(HideDefaultNoticeAfterTime(time));
    }

    IEnumerator HideDefaultNoticeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        HideDefaultHint();
    }

    public void HideDefaultHint()
    {
        HintsUIM.instance.defaultNotice?.Hide();
    }

    // Pleasure Notice

    public void ShowPleasureNotice(string hintText)
    {
        HintsUIM.instance.pleasureNotice?.Show(hintText);
    }

    public void ShowPleasureNotice(string hintText, float time)
    {
        HintsUIM.instance.pleasureNotice?.Show(hintText);
        StartCoroutine(HidePleasureNoticeAfterTime(time));
    }

    IEnumerator HidePleasureNoticeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        HidePleasureNotice();
    }

    public void HidePleasureNotice()
    {
        HintsUIM.instance.pleasureNotice?.Hide();
    }

    // Warning Notice

    public void ShowWarningNotice(string hintText)
    {
        HintsUIM.instance.warningNotice?.Show(hintText);
    }

    public void ShowWarningNotice(string hintText, float time)
    {
        HintsUIM.instance.warningNotice?.Show(hintText);
    }

    IEnumerator HideWarningNoticeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        HideWarningNotice();
    }

    public void HideWarningNotice()
    {
        HintsUIM.instance.warningNotice?.Hide();
    }
}
