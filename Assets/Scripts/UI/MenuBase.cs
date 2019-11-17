using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour
{
    public Canvas MenuCanvas;
    public RenderMode CanvasRenderMode = RenderMode.ScreenSpaceCamera;
    public int PlaneDistance = 100;
    public int OrderInLayer;
    public CameraType Camera;

    private const string SORTING_LAYER = "UI";

    public enum CameraType
    {
        WORLD,
        UI
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
    }
}
