using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arcadeum.Common
{
	public class Pool : MonoBehaviour
	{
		private static Pool _instance;
		[SerializeField] private List<ObjectPoolItem> _itemsToPool;


		private List<List<GameObject>> _pooledObjectsList;
		private List<GameObject> _pooledObjects;

		private void Start()
		{
			if (_instance != null)
				Destroy(this);

			_instance = this;

			_pooledObjectsList = new List<List<GameObject>>();
			_pooledObjects = new List<GameObject>();


			for (var i = 0; i < _itemsToPool.Count; i++)
			{
				ObjectPoolItemToPooledObject(i);
			}
		}

		/// <summary>Gets the object from the pool, and if all are being used, creates a new one</summary>
		public static GameObject Spawn(GameObject prefab)
		{
			var index = _instance.PrefabToIndex(prefab);
			if (index == -1)
			{
				index = _instance._itemsToPool.Count;
				_instance._itemsToPool.Add(new ObjectPoolItem(prefab, 1, true));
				_instance.ObjectPoolItemToPooledObject(index);
			}
			GameObject gameObject = _instance.GetPooledObject(index);
			gameObject.SetActive(true);
			return gameObject;
		}

		/// <summary>Turns object inactive, additionaly you can provide delay</summary>
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
			for (int i = 0; i < _itemsToPool.Count; i++)
			{
				if (_itemsToPool[i].PooledObject.GetHashCode() == prefab.GetHashCode())
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

			if (_itemsToPool[index].ShouldExpand)
			{
				var obj = Instantiate(_itemsToPool[index].PooledObject, this.transform, true);
				obj.SetActive(false);
				_pooledObjectsList[index].Add(obj);
				return obj;
			}
			return null;
		}
		private void ObjectPoolItemToPooledObject(int index)
		{
			ObjectPoolItem item = _itemsToPool[index];

			_pooledObjects = new List<GameObject>();
			for (int i = 0; i < item.Amount; i++)
			{
				GameObject obj = Instantiate(item.PooledObject, this.transform);
				obj.SetActive(false);
				_pooledObjects.Add(obj);
			}
			_pooledObjectsList.Add(_pooledObjects);
		}
	}

	[System.Serializable]
	public class ObjectPoolItem
	{
		[SerializeField, FormerlySerializedAs("name")] private string _name;
		[SerializeField, FormerlySerializedAs("objectToPool")] private GameObject _objectToPool;
		[SerializeField, FormerlySerializedAs("amountToPool")] private int _amountToPool;
		[SerializeField, FormerlySerializedAs("shouldExpand")] private bool _shouldExpand = true;

		public GameObject PooledObject => _objectToPool;
		public int Amount => _amountToPool;
		public bool ShouldExpand => _shouldExpand;

		public ObjectPoolItem(GameObject obj, int amt, bool exp = true)
		{
			_name = obj.name;
			_objectToPool = obj;
			_amountToPool = Mathf.Max(amt, 1);
			_shouldExpand = exp;
		}
	}
}