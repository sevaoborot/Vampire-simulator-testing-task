using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomObjectPool
{
    private GameObject _prefab;
    private List<GameObject> _objects;

    public CustomObjectPool(GameObject prefab, int prewarmOnjectsNumber) 
    {
        _prefab = prefab;
        _objects = new List<GameObject>();
        for (int i = 0; i < prewarmOnjectsNumber; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            _objects.Add(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj = _objects.FirstOrDefault(x => !x.activeSelf);
        if (obj == null) obj = Create();
        if (obj != null) obj.gameObject.SetActive(true);
        return obj;
    }

    public void Release(GameObject obj, Action<GameObject> objCleanup = null)
    {
        obj.gameObject.SetActive(false);
        if (objCleanup != null) objCleanup(obj);
    }

    public void ReleaseBy(Func<GameObject, bool> predicate, Action<GameObject> objCleanup = null)
    {
        List<GameObject> objToRelease = new List<GameObject>();
        foreach (GameObject obj in _objects) if (predicate(obj)) objToRelease.Add(obj);
        if (objToRelease.Count > 0) foreach (GameObject obj in objToRelease) Release(obj, objCleanup);
    }

    public void ReleaseAll(Action<GameObject> objCleanup = null)
    {
        foreach (GameObject obj in _objects) Release(obj, objCleanup);
    }

    private GameObject Create()
    {
        GameObject obj = GameObject.Instantiate(_prefab);
        _objects.Add(obj);
        return obj;
    }
}
