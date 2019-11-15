using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBase : MonoBehaviour
{
    public Canvas MenuCanvas;
    public RenderMode CanvasRenderMode;
    public int PlaneDistance;
    public int OrderInLayer;
    public CameraType Camera;

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
        }
    }
}
