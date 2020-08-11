using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Michsky.UI.ModernUIPack;

public class CollectGoalEffects : MonoBehaviour
{
    public ProgressBar GoalProgressBar;
    public CollectGoalEffect CollectEffect;
    public Transform Target;
    public Transform ZenTarget;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.ParticlesEnabled)
        {
            GlobalEvents.LeafCollectedInfo.AddListener(handleLeafCollected);
        }
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

        
        if(GameManager.IsZenLevel())
        {
            float x = ZenTarget.position.x;
            float y = ZenTarget.position.y;

            effect.transform.DOMoveX(x, 0.5f).SetEase(Ease.OutQuad);
            effect.transform.DOMoveY(y, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Destroy(effect.gameObject);
            });
        }
        else
        {
            float progress = GoalProgressBar.specifiedValue;
            float x = Target.position.x - 1.2f + (progress * 2.5f / 100f);
            float y = Target.position.y;

            effect.transform.DOMoveX(x, 0.5f).SetEase(Ease.OutQuad);
            effect.transform.DOMoveY(y, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                Destroy(effect.gameObject);
            });
        }
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
