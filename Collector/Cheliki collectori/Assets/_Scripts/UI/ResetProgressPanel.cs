using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetProgressPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text Btn_text;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private Button btn;
    void OnEnable()
    {
        StartCoroutine(TimerCoroutene());
    }

    void OnDisable()
    {
        StopCoroutine(TimerCoroutene());
        timer.text = "";
        Btn_text.alpha = 0;
    }

    IEnumerator TimerCoroutene()
    {
        btn.interactable = false;
        Btn_text.alpha = 0;
        timer.text = "(5)";
        yield return new WaitForSeconds(1);
        timer.text = "(4)";
        yield return new WaitForSeconds(1);
        timer.text = "(3)";
        yield return new WaitForSeconds(1);
        timer.text = "(2)";
        yield return new WaitForSeconds(1);
        timer.text = "(1)";
        yield return new WaitForSeconds(1);
        timer.text = "";
        Btn_text.alpha = 1;
        btn.interactable = true;
        
    }

    public void ResetProgressButton()
    {
        SaveManager.Instance?.ResetProgress();
    }

}
