using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuBase : MonoBehaviour
{
    public Canvas MenuCanvas;
    public CanvasGroup CanvasGroup;
    public RenderMode CanvasRenderMode = RenderMode.ScreenSpaceCamera;
    public int PlaneDistance = 100;
    public int OrderInLayer;
    public CameraType Camera;

    public bool AnimateIn;

    private const string SORTING_LAYER = "UI";

    public enum CameraType
    {
        WORLD,
        UI
    }

    private void Awake()
    {
        if (AnimateIn && CanvasGroup != null)
        {
            CanvasGroup.alpha = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(MenuCanvas != null)
        {
            MenuCanvas.renderMode = CanvasRenderMode;
            MenuCanvas.planeDistance = PlaneDistance;
            MenuCanvas.sortingOrder = OrderInLayer;

            if (Camera == CameraType.WORLD)
            {
                MenuCanvas.worldCamera = CameraManager.instance.World;
            }
            else if(Camera == CameraType.UI)
            {
                MenuCanvas.worldCamera = CameraManager.instance.UI;
            }

            MenuCanvas.sortingLayerName = SORTING_LAYER;
        }

        if(AnimateIn && CanvasGroup != null)
        {
            CanvasGroup.DOFade(1f, 0.25f);
        }
    }

    public void Close()
    {
        MenuManager.PopMenu(gameObject);
    }
}
