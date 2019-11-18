using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeafSpawner : MonoBehaviour
{
    public Leaf LeafPrefab;
    public int NumLeaves;
    public Vector2Int Dimensions;
    public GameObject GroundObj;

    private List<Leaf> leaves;
    private List<int> removeIndices;

    private bool _levelWon;
    private bool _levelLost;

    public static UnityEvent LeafCollectedEvent = new UnityEvent();
    public static MyIntEvent LeavesSpawned = new MyIntEvent();

    private void Awake()
    {
        GlobalEvents.StartLevel.AddListener(handleLevelStart);
        GlobalEvents.RetryLevel.AddListener(handleRetryLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        GroundObj.transform.localScale = new Vector3(Dimensions.x, 6, Dimensions.y);
        removeIndices = new List<int>();
    }

    private void handleLoseLevel()
    {
        _levelLost = true;
    }

    private void handleRetryLevel()
    {
        _levelWon = false;
        _levelLost = false;

        removeAllLeaves();
        spawnLeaves();
    }

    private void handleLevelStart()
    {
        _levelWon = false;
        _levelLost = false;

        removeAllLeaves();
        spawnLeaves();
    }

    void spawnLeaves()
    {
        LeavesSpawned.Invoke(NumLeaves);

        leaves = new List<Leaf>();
        for (int i = 0; i < NumLeaves; i++)
        {
            Leaf newLeaf = Instantiate(LeafPrefab, transform);
            newLeaf.transform.position = new Vector3(Random.Range(Dimensions.x / -2f, Dimensions.x / 2f), 1f, Random.Range(Dimensions.y / -2f, Dimensions.y / 2f));
            leaves.Add(newLeaf);
        }
    }

    void removeAllLeaves()
    {
        if (leaves != null)
        {
            for (int i = leaves.Count - 1; i >= 0; i--)
            {
                Leaf leaf = leaves[i];
                Destroy(leaf.gameObject);
            }

            leaves.Clear();
        }
    }

    private void Update()
    {
        if (leaves != null)
        {
            if (leaves.Count > 0)
            {
                for (int i = leaves.Count - 1; i >= 0; i--)
                {
                    Leaf leaf = leaves[i];
                    if (leaf.transform.position.y < -3f)
                    {
                        Destroy(leaf.gameObject);
                        removeIndices.Add(i);

                        if (!_levelLost)
                        {
                            LeafCollectedEvent.Invoke();
                        }
                    }
                }

                for (int i = 0; i < removeIndices.Count; i++)
                {
                    int index = removeIndices[i];
                    leaves.RemoveAt(index);
                }

                removeIndices.Clear();

            }
            else
            {
                if(!_levelWon)
                {
                    _levelWon = true;
                    GlobalEvents.WinLevel.Invoke();
                    StartCoroutine(respawnLeaves());
                }
            }
        }
    }

    private IEnumerator respawnLeaves()
    {
        yield return new WaitForSeconds(1f);
        spawnLeaves();
    }
}
