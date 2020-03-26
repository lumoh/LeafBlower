using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPointerManager : MonoBehaviour
{

    public const int UP_LEFT    = 0;
    public const int UP_RIGHT   = 1;
    public const int RIGHT_UP   = 2;
    public const int RIGHT_DOWN = 3;
    public const int DOWN_RIGHT = 4;
    public const int DOWN_LEFT  = 5;
    public const int LEFT_DOWN  = 6;
    public const int LEFT_UP    = 7;
    public const float SCREEN_OFFSET = 70;

    // Only have 1 arrow max per quadrant
    public ArrowAndTarget UL_arrow;
    public ArrowAndTarget UR_arrow;
    public ArrowAndTarget RU_arrow;
    public ArrowAndTarget RD_arrow;
    public ArrowAndTarget DR_arrow;
    public ArrowAndTarget DL_arrow;
    public ArrowAndTarget LD_arrow;
    public ArrowAndTarget LU_arrow;

    public Image ArrowClone;

    private Camera _worldCam;
    private Camera _uiCam;
    private ArrowAndTarget[] _targets;
    private List<Leaf> _pendingLeaves = new List<Leaf>();

    public static GoalPointerManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.GoalPointers)
            Destroy(gameObject);
            
        instance = this;
        _worldCam = CameraManager.instance.World;
        _uiCam = CameraManager.instance.UI;

        UL_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        UR_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        RU_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        RD_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        DR_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        DL_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        LD_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);
        LU_arrow = new ArrowAndTarget(Instantiate(ArrowClone, ArrowClone.transform.parent), null);

        _targets = new ArrowAndTarget[8];
        _targets[UP_LEFT] = UL_arrow;
        _targets[UP_RIGHT] = UR_arrow;
        _targets[RIGHT_UP] = RU_arrow;
        _targets[RIGHT_DOWN] = RD_arrow;
        _targets[DOWN_RIGHT] = DR_arrow;
        _targets[DOWN_LEFT] = DL_arrow;
        _targets[LEFT_DOWN] = LD_arrow;
        _targets[LEFT_UP] = LU_arrow;

        GlobalEvents.LoseLevel.AddListener(removeTargets);
        GlobalEvents.WinLevel.AddListener(removeTargets);        
    }

    void removeTargets()
    {
        foreach(var t in _targets)
        {
            if (t != null)
            {
                t.ShowArrow(false);
            }
        }
    }

    public void AddTarget(Leaf target)
    {
        _pendingLeaves.Add(target);
    }

    public void setTargets()
    {
        foreach (var leaf in _pendingLeaves)
        {
            if (leaf != null)
            {
                int zoneIndex = getZoneIndex(leaf.transform.position);
                if(zoneIndex != -1)
                {
                    addTarget(zoneIndex, leaf);
                }                
            }
        }
    }

    private int getZoneIndex(Vector3 worldPos)
    {
        int index = -1;

        Vector3 targetScreenPoint = _worldCam.WorldToScreenPoint(worldPos);
        bool isOffscreen = targetScreenPoint.x < 0 || targetScreenPoint.y < 0 || targetScreenPoint.x > Screen.width || targetScreenPoint.y > Screen.height;
        if (isOffscreen)
        {
            Vector3 cappedScreenPoint = targetScreenPoint;
            if (cappedScreenPoint.x < SCREEN_OFFSET)
            {
                if (cappedScreenPoint.y > Screen.height / 2)
                {
                    index = LEFT_UP;
                }
                else
                {
                    index = LEFT_DOWN;
                }
            }
            else if (cappedScreenPoint.y < SCREEN_OFFSET)
            {
                if (cappedScreenPoint.x > Screen.width / 2)
                {
                    index = DOWN_RIGHT;
                }
                else
                {
                    index = DOWN_LEFT;
                }
            }
            else if (cappedScreenPoint.x > Screen.width - SCREEN_OFFSET)
            {
                if (cappedScreenPoint.y > Screen.height / 2)
                {
                    index = RIGHT_UP;
                }
                else
                {
                    index = RIGHT_DOWN;
                }
            }
            else if (cappedScreenPoint.y > Screen.height - SCREEN_OFFSET)
            {
                if (cappedScreenPoint.x > Screen.width / 2)
                {
                    index = UP_RIGHT;
                }
                else
                {
                    index = UP_LEFT;
                }
            }
        }

        return index;
    }

    private void addTarget(int index, Leaf leaf)
    {
        if(_targets[index].Target == null && leaf != null)
        {
            _targets[index].Target = leaf;            
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (_targets == null)
            return;


        for(int i = 0; i < _targets.Length; i++)
        {
            if (_targets[i] != null)
            {
                var arrow = _targets[i].Arrow;
                var target = _targets[i].Target;

                if (target != null && target.IsResting() && getZoneIndex(target.transform.position) == i)
                {
                    Vector3 targetScreenPoint = _worldCam.WorldToScreenPoint(target.transform.position);
                    bool isOffscreen = targetScreenPoint.x < 0 || targetScreenPoint.y < 0 || targetScreenPoint.x > Screen.width || targetScreenPoint.y > Screen.height;
                    if (isOffscreen)
                    {
                        _targets[i].ShowArrow(true);
                        Vector3 cappedScreenPoint = targetScreenPoint;
                        cappedScreenPoint.z = 0;
                        targetScreenPoint.z = 0;
                        if (cappedScreenPoint.x < SCREEN_OFFSET) cappedScreenPoint.x = SCREEN_OFFSET;
                        if (cappedScreenPoint.y < SCREEN_OFFSET) cappedScreenPoint.y = SCREEN_OFFSET;
                        if (cappedScreenPoint.x > Screen.width - SCREEN_OFFSET) cappedScreenPoint.x = Screen.width - SCREEN_OFFSET;
                        if (cappedScreenPoint.y > Screen.height - SCREEN_OFFSET) cappedScreenPoint.y = Screen.height - SCREEN_OFFSET;
                        var worldPoint = _uiCam.ScreenToWorldPoint(cappedScreenPoint);
                        worldPoint.z = 0;
                        arrow.rectTransform.position = worldPoint;

                        if(i == UP_LEFT || i == UP_RIGHT)
                        {
                            arrow.rectTransform.localEulerAngles = new Vector3(0, 0, -90);
                        }
                        else if (i == LEFT_UP || i == LEFT_DOWN)
                        {
                            arrow.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        else if (i == RIGHT_UP || i == RIGHT_DOWN)
                        {
                            arrow.rectTransform.localEulerAngles = new Vector3(0, 0, 180);
                        }
                        else if (i == DOWN_LEFT || i == DOWN_RIGHT)
                        {
                            arrow.rectTransform.localEulerAngles = new Vector3(0, 0, 90);
                        }
                    }
                    else
                    {
                        _targets[i].ShowArrow(false);
                        _targets[i].Target.RemovePointer();
                        _targets[i].Target = null;
                        setTargets();
                    }
                }
                else
                {
                    _targets[i].ShowArrow(false);
                    setTargets();
                }
            }
            else
            {
                setTargets();
            }
        }
    }
}

public class ArrowAndTarget
{
    public Image Arrow;
    public Leaf Target;

    public ArrowAndTarget(Image arrow, Leaf target)
    {
        Arrow = arrow;
        Target = target;
    }

    public void ShowArrow(bool show)
    {
        if (Arrow != null)
        {
            Arrow.gameObject.SetActive(show);
        }
    }
}
