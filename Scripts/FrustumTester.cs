using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FrustumTester : MonoBehaviour
{
    [SerializeField] float spawnRange = 3f;
    [SerializeField] float spawnYmaxRange = 0.5f;
    [SerializeField] float observerGap = 1f;

    [SerializeField] FrustrumTrackerBase frustrumTracker;

    private List<Transform> spawned;

    private void Start()
    {
        spawned = new List<Transform>();
    }

    private Transform CreateTarget()
    {
        var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Vector3 pos = new Vector3()
        {
            x = Random.Range(observerGap, spawnRange) * RandomSign(),
            y = Random.Range(-spawnYmaxRange, spawnYmaxRange),
            z = Random.Range(observerGap, spawnRange) * RandomSign()
        };
       
        t.position = transform.position + pos;

        return t;
    }

    int RandomSign()
    {
        return Random.value < .5 ? 1 : -1;
    }

    public void SpawnNew()
    {
        var n = CreateTarget();
        spawned.Add(n);
        frustrumTracker.AddTrackedObject(n, new TrackedObjectData
        {
            name = Time.time.ToString()
        });
    }

    public void RemoveRandom()
    {
        if (spawned.Count == 0) return;

        int index = Random.Range(0, spawned.Count);

        Transform t = spawned[index];

        frustrumTracker.RemoveTrackedObject(t);

        spawned.RemoveAt(index);

        Destroy(t.gameObject);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnNew();
        }

        if(Input.GetMouseButtonDown(1))
        {
            RemoveRandom();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FrustumTester))]
public class FrustumTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FrustumTester ft = (FrustumTester)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn"))
        {
            ft.SpawnNew();
        }
        if (GUILayout.Button("Remove"))
        {
            ft.RemoveRandom();
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("Also press LMB to spawn and RMB to remove random cube");
    }
}
#endif