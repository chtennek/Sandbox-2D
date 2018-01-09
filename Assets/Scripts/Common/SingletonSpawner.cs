using UnityEngine;
using System.Collections;

public class SingletonSpawner : MonoBehaviour
{
	public Transform[] singletonPrefabs;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		if (GameObject.FindObjectsOfType(typeof(SingletonSpawner)).Length > 1) {
			return;
		}

		foreach (var prefab in singletonPrefabs) {
			if (GameObject.Find(prefab.name) == null) {
				var singleton = Instantiate(prefab);
				singleton.parent = transform;
			}
		}
	}
}
