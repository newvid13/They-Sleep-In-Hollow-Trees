using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public int currentX, currentY;
    Image myImg;
    bool isAlive = true;

    public Sprite[] liveImgs;
    private int liveIndex;

    private void Start()
    {
        myImg = GetComponent<Image>();

        float startRotAdd = Random.Range(-6f, 6f);
        Quaternion startRot = Quaternion.identity;
        startRot = Quaternion.Euler(0f, 0f, -startRotAdd);

        myImg.rectTransform.rotation = startRot;
        myImg.rectTransform.DORotate(new Vector3(0f, 0f, startRotAdd), Random.Range(1f, 2f)).SetLoops(-1, LoopType.Yoyo).SetId("LightShake" + gameObject.name);

        StartCoroutine(animateTile());
    }

    private IEnumerator animateTile()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.8f));

        if (isAlive)
        {
            liveIndex = (liveIndex + 1) % liveImgs.Length;
            myImg.sprite = liveImgs[liveIndex];
            StartCoroutine(animateTile());
        }
    }

    public void TileClicked()
    {
        Node nodeToMove = MainManager.grid.FindEmptyNode(currentX, currentY);

        if (nodeToMove == null)
            Shake();
        else
        {
            MainManager.grid.PlayAudio(0);
            MainManager.grid.MoveTile(currentX, currentY, nodeToMove);
        }
    }

    private void Shake()
    {
        MainManager.grid.PlayAudio(1);
        myImg.rectTransform.DOShakeRotation(0.3f, 90f).SetId("Shake" + gameObject.name);
    }

    public void KillTile(Sprite deadSprite)
    {
        MainManager.grid.PlayAudio(4);
        isAlive = false;
        myImg.sprite = deadSprite;
        DOTween.Kill("LightShake" + gameObject.name);
        DOTween.Kill("Shake" + gameObject.name);
        myImg.rectTransform.rotation = Quaternion.identity;
        myImg.raycastTarget = false;
    }
}
