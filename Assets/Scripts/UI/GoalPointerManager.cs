using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPointerManager : MonoBehaviour
{
    public Image ArrowClone;
    private Camera _cam;
    private List<Leaf> _targets = new List<Leaf>();
    private List<Image> _arrows = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        _cam = CameraManager.instance.World;
        GlobalEvents.StartLevel.AddListener(handleStart);
    }

    void handleStart()
    {
        Leaf leaf = GameManager.instance.Level.GetRandomLeaf();
        _targets.Add(leaf);
        var arrow = Instantiate(ArrowClone, ArrowClone.transform.parent);
        arrow.gameObject.SetActive(true);
        _arrows.Add(arrow);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _targets.Count; i++)
        {
            var arrow = _arrows[i];
            var target = _targets[i];

            if (arrow != null && target != null)
            {
                float offset = 100;
                Vector3 targetScreenPoint = _cam.WorldToScreenPoint(target.transform.position);
                bool isOffscreen = targetScreenPoint.x < offset || targetScreenPoint.y < offset || targetScreenPoint.x > Screen.width - offset || targetScreenPoint.y > Screen.height - offset;
                if (isOffscreen)
                {
                    arrow.gameObject.SetActive(true);
                    Vector3 cappedScreenPoint = targetScreenPoint;
                    if (cappedScreenPoint.x < offset) cappedScreenPoint.x = offset;
                    if (cappedScreenPoint.y < offset) cappedScreenPoint.y = offset;
                    if (cappedScreenPoint.x > Screen.width - offset) cappedScreenPoint.x = Screen.width - offset;
                    if (cappedScreenPoint.y > Screen.height - offset) cappedScreenPoint.y = Screen.height - offset;
                    var worldPoint = CameraManager.instance.UI.ScreenToWorldPoint(cappedScreenPoint);
                    var worldPoint2 = CameraManager.instance.UI.ScreenToWorldPoint(targetScreenPoint);
                    arrow.rectTransform.position = worldPoint;

                    Vector3 to = worldPoint2;
                    Vector3 from = CameraManager.instance.UI.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                    Vector3 dir = (to - from).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    arrow.rectTransform.localEulerAngles = new Vector3(0, 0, angle + 180);
                }
                else
                {
                    arrow.gameObject.SetActive(false);
                }
            }
            else
            {
                if(arrow != null)
                {
                    Destroy(arrow.gameObject);
                    _arrows.RemoveAt(i);
                    _targets.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
