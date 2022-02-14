using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
	public string name;
	public GameObject objectToPool;
	public int amountToPool;
	public bool shouldExpand = true;

	public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
	{
		objectToPool = obj;
		amountToPool = Mathf.Max(amt,2);
		shouldExpand = exp;
	}
}

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler instance;
	public List<ObjectPoolItem> itemsToPool;


	public List<List<GameObject>> pooledObjectsList;
	public List<GameObject> pooledObjects;
	private List<int> _positions;

	private void Awake()
	{
		instance = this;

		pooledObjectsList = new List<List<GameObject>>();
		pooledObjects = new List<GameObject>();
		_positions = new List<int>();


		for (var i = 0; i < itemsToPool.Count; i++)
		{
			ObjectPoolItemToPooledObject(i);
		}

	}


	public GameObject GetPooledObject(int index)
	{

		var curSize = pooledObjectsList[index].Count;
		for (var i = _positions[index] + 1; i < _positions[index] + pooledObjectsList[index].Count; i++)
		{

			if (!pooledObjectsList[index][i % curSize].activeInHierarchy)
			{
				_positions[index] = i % curSize;
				return pooledObjectsList[index][i % curSize];
			}
		}

		if (itemsToPool[index].shouldExpand)
		{
			var obj = (GameObject)Instantiate(itemsToPool[index].objectToPool, this.transform, true);
			obj.SetActive(false);
			pooledObjectsList[index].Add(obj);
			return obj;

		}
		return null;
	}
	
	void ObjectPoolItemToPooledObject(int index)
	{
		ObjectPoolItem item = itemsToPool[index];

		pooledObjects = new List<GameObject>();
		for (int i = 0; i < item.amountToPool; i++)
		{
			GameObject obj = (GameObject)Instantiate(item.objectToPool);
			obj.SetActive(false);
			obj.transform.parent = this.transform;
			pooledObjects.Add(obj);
		}
		pooledObjectsList.Add(pooledObjects);
		_positions.Add(0);

	}
}