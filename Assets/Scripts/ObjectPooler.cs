using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool {
        public string name;
        public GameObject prefab;
        public int maxNumber;
        public Queue<GameObject> objects = new Queue<GameObject>();
    }

    public Pool[] pools;

    //Singleton
    public static ObjectPooler instance;

    private void Awake() {
        instance = this;
    }

    void Start() {

        foreach (Pool pool in pools) {
            for (int i = 0; i < pool.maxNumber; i++) {
                GameObject o = Instantiate(pool.prefab);
                o.SetActive(false);
                pool.objects.Enqueue(o);

            }

        }

    }

    public GameObject InstantiateFromPool(int index, Vector3 position,Quaternion rotation) {
        GameObject o = pools[index].objects.Dequeue();
        o.SetActive(true);
        o.transform.position = position;
        o.transform.rotation = rotation;
        pools[index].objects.Enqueue(o);

        return o;
    }

}
