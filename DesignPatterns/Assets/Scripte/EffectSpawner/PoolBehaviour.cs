using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public interface IPooledObject
{
	void Reawake();
}

public class GameObjectPool : IDisposable
{
	bool disposed = false;
	GameObject prefab;
	Quaternion orgRot;
	Vector3 orgSize;
	Queue<GameObject> pool = new Queue<GameObject>();
	List<GameObject> usingObjects = new List<GameObject>();

	public GameObjectPool(GameObject _prefab)
	{
		prefab = _prefab;
		orgSize = _prefab.transform.localScale;
		orgRot = _prefab.transform.localRotation;
	}

	List<IPooledObject> cachedPoolList_ = new List<IPooledObject>();

	public GameObject Create(GameObject parent = null)
	{
		if (disposed) throw new Exception ("Already disposed");
		GameObject obj;
		if( pool.Count > 0)
		{
			obj = pool.Dequeue();
		}
		else
		{
			obj = GameObject.Instantiate(prefab);
			obj.GetComponent<PoolBehaviour>().parentPool_ = this;
		}

		obj.GetComponents<IPooledObject>(cachedPoolList_);
		for (int i = 0; i < cachedPoolList_.Count; i++)
		{
			cachedPoolList_[i].Reawake();
		}

		usingObjects.Add (obj);

		Transform parentTransform;
		if (parent == null)
		{
			parentTransform = prefab.transform.parent;
		}
		else
		{
			parentTransform = parent.transform;
		}
		obj.transform.SetParent(parentTransform, false);
		obj.SetActive (true);
		return obj;
	}

	public void Release(GameObject obj)
	{
		if (disposed) throw new Exception ("Already disposed");

		// フォーカスしていた際に、その後処理を行う
		if( EventSystem.current != null)
		{
			if( EventSystem.current.currentSelectedGameObject == obj)
			{
				EventSystem.current.SetSelectedGameObject(null);
				var animator = obj.GetComponent<Animator>();
				if( animator != null)
				{
					animator.Update(0); // Animatorが中途半端な状態にならないように、クリアする
				}
			}
		}

		var releaseTwice = usingObjects.Remove (obj);
		if (!releaseTwice)
		{
			// TODO: 一時的に握りつぶす
			//Debug.LogError("hoge");
			return;
		}
		pool.Enqueue(obj);
		obj.transform.localScale = orgSize;
		obj.transform.localRotation = orgRot;
		obj.transform.SetParent (prefab.transform, false);
	}

	public void ReleaseAll()
	{
		if (disposed) throw new Exception ("Already disposed");
		foreach (var obj in usingObjects.ToArray())
		{
			Release (obj);
		}
	}

	public void DestroyAll()
	{
		if (disposed) throw new Exception("Already disposed");
		foreach (var obj in usingObjects.ToArray())
		{
			GameObject.Destroy(obj);
		}
		usingObjects.Clear();
	}

	public void Dispose()
	{
		if (disposed) return;
		disposed = true;
		foreach (var obj in pool)
		{
			GameObject.Destroy (obj);
		}
	}
}

public class PoolBehaviour : MonoBehaviour
{
	GameObjectPool pool;
	public bool disableOnAwake = true;

	[NonSerialized]
	public GameObjectPool parentPool_;

	void Awake()
	{
		init();
	}

	void init()
	{
		if (pool == null)
		{
			pool = new GameObjectPool(gameObject);
			if (disableOnAwake)
			{
				gameObject.SetActive(false);
				disableOnAwake = false;
			}
		}
	}

	public GameObject Create(GameObject parent = null)
	{
		init();
		return pool.Create (parent);
	}

	public T Create<T>(GameObject parent = null)
	{
		init();
		return pool.Create (parent).GetComponent<T>();
	}

	public void Release(GameObject obj)
	{
		init();
		pool.Release (obj);
	}

	public void ReleaseAll()
	{
		init();
		pool.ReleaseAll ();
	}

	public void DestroyAll()
	{
		init();
		pool.DestroyAll();
	}

	public void OnDestroy()
	{
		init();
		if( pool != null ) pool.Dispose();
	}

	public void Release()
	{
		parentPool_.Release(gameObject);
	}
}

