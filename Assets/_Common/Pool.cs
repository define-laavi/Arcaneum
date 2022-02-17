using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcadeum.Common
{
	[System.Serializable]
	public class ObjectPoolItem
	{
		public string name;
		public GameObject objectToPool;
		public int amountToPool;
		public bool shouldExpand = true;

		public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
		{
			name = obj.name;
			objectToPool = obj;
			amountToPool = Mathf.Max(amt, 1);
			shouldExpand = exp;
		}
	}

	public class Pool : MonoBehaviour
	{
		private static Pool _instance;
		public List<ObjectPoolItem> itemsToPool;


		private List<List<GameObject>> _pooledObjectsList;
		private List<GameObject> _pooledObjects;

		private void Awake()
		{
			if (_instance != null)
				Destroy(this);

			_instance = this;

			_pooledObjectsList = new List<List<GameObject>>();
			_pooledObjects = new List<GameObject>();


			for (var i = 0; i < itemsToPool.Count; i++)
			{
				ObjectPoolItemToPooledObject(i);
			}
		}

		public static GameObject Spawn(GameObject prefab)
		{
			var index = _instance.PrefabToIndex(prefab);
			if (index == -1)
			{
				index = _instance.itemsToPool.Count;
				_instance.itemsToPool.Add(new ObjectPoolItem(prefab, 1, true));
				_instance.ObjectPoolItemToPooledObject(index);
			}
			GameObject gameObject = _instance.GetPooledObject(index);
			gameObject.SetActive(true);
			return gameObject;
		}

		public static void Despawn(GameObject gameObject, float delay = 0f)
		{
			if (delay <= 0)
			{
				gameObject.SetActive(false);
			}
			else
			{
				_instance.StartCoroutine(DelayedDespawn(gameObject, delay));
			}
		}
		private static IEnumerator DelayedDespawn(GameObject gameObject, float delay)
		{
			yield return new WaitForSecondsRealtime(delay);
			gameObject.SetActive(false);
		}
		private int PrefabToIndex(GameObject prefab)
		{
			for (int i = 0; i < itemsToPool.Count; i++)
			{
				if (itemsToPool[i].objectToPool.GetHashCode() == prefab.GetHashCode())
					return i;
			}
			return -1;
		}

		private GameObject GetPooledObject(int index)
		{
			for (int i = 0; i < _pooledObjectsList[index].Count; i++)
			{
				if (!_pooledObjectsList[index][i].activeInHierarchy)
				{
					return _pooledObjectsList[index][i];
				}
			}

			if (itemsToPool[index].shouldExpand)
			{
				var obj = Instantiate(itemsToPool[index].objectToPool, this.transform, true);
				obj.SetActive(false);
				_pooledObjectsList[index].Add(obj);
				return obj;

			}
			return null;
		}
		private void ObjectPoolItemToPooledObject(int index)
		{
			ObjectPoolItem item = itemsToPool[index];

			_pooledObjects = new List<GameObject>();
			for (int i = 0; i < item.amountToPool; i++)
			{
				GameObject obj = Instantiate(item.objectToPool);
				obj.SetActive(false);
				obj.transform.parent = this.transform;
				_pooledObjects.Add(obj);
			}
			_pooledObjectsList.Add(_pooledObjects);
		}
	}
}