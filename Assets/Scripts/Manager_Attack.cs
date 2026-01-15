using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;

public class Manager_Attack : MonoBehaviour
{
    public Image cross;
    private float waitSpeed = 3f;
    private Node chosenNode;

    public GameObject endImage;
    public TMP_Text timerText;

    private float timer = 0f;

    public void SetupValues()
    {
        Time.timeScale = 1;
        StartCoroutine(waitAttack());
    }

    private IEnumerator waitAttack()
    {
        yield return new WaitForSeconds(waitSpeed);
        Attack();
    }

    public void Attack()
    {
        timer += waitSpeed;
        Node[] aliveNodes = MainManager.grid.FindAliveNodes();

        if(aliveNodes.Length > 0 )
        {
            MainManager.grid.PlayAudio(2);
            int fakeSumAttack = Mathf.Clamp(aliveNodes.Length, 1, 10);

            waitSpeed = fakeSumAttack / 3f;
            float attackSpeed = (float)fakeSumAttack / 2f;

            if(timer < 20f)
            {
                waitSpeed /= 2f;
                attackSpeed /= 2f;
            }
            else if (timer < 50f)
            {
                waitSpeed /= 3f;
                attackSpeed /= 3f;
            }
            else
            {
                waitSpeed /= 4f;
                attackSpeed /= 4f;
            }

            attackSpeed = Mathf.Clamp(attackSpeed, 0.1f, 4f);
            chosenNode = aliveNodes[Random.Range(0, aliveNodes.Length)];

            cross.rectTransform.localScale = new Vector2(0.1f, 0.1f);
            Vector2 attPos = new Vector2(chosenNode.x * 116 + 68, chosenNode.y * 116 + 68);
            cross.rectTransform.anchoredPosition = attPos;
            cross.rectTransform.DOScale(1f, attackSpeed).OnComplete(CheckImpact);

        }
        else
        {
            Time.timeScale = 0;
            timerText.text = "You survived for " + timer.ToString("F1") + " seconds";
            endImage.SetActive(true);
        }
    }

    private void CheckImpact()
    {
        if (MainManager.grid.grid[chosenNode.x, chosenNode.y].isAlive && MainManager.grid.grid[chosenNode.x, chosenNode.y].myTile != null)
            MainManager.grid.KillTile(chosenNode.x, chosenNode.y);
        else
            MainManager.grid.PlayAudio(3);

        cross.rectTransform.anchoredPosition = new Vector2(-200, -200);
        StartCoroutine(waitAttack());
    }
}
