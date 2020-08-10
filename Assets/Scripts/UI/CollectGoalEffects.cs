using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectGoalEffects : MonoBehaviour
{
    public CollectGoalEffect CollectEffect;
    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        GlobalEvents.LeafCollectedInfo.AddListener(handleLeafCollected);
    }

    // Update is called once per frame
    void handleLeafCollected(Leaf leaf)
    {
        var worldCam = CameraManager.instance.World;
        var uiCam = CameraManager.instance.UI;

        Vector3 screenPos = worldCam.WorldToScreenPoint(leaf.transform.position);
        Vector3 uiPos = uiCam.ScreenToWorldPoint(screenPos);

        var effect = Instantiate(CollectEffect, CollectEffect.transform.parent);
        effect.Init(colorFromInt(leaf.ColorInt));
        effect.gameObject.SetActive(true);
        effect.transform.position = uiPos;
        effect.transform.DOMove(Target.position, 0.5f).OnComplete(() =>
        {
            Destroy(effect.gameObject);
        });
    }

    private Color colorFromInt(int colorInt)
    {
        Color c = Color.red;
        if(colorInt == 0)
        {
            c = Color.red;
        }
        else if(colorInt == 1)
        {
            c = new Color(1, 0.66f, 0);
        }
        else if (colorInt == 2)
        {
            c = Color.yellow;
        }
        return c;
    }
}
