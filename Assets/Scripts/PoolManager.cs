using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    List<GameObject> pool = new List<GameObject>();

    public float randomDistance = 1;
    public float timeBetween = 1;
    public static bool isPlay=true;


    // Start is called before the first frame update
    void Start()
    {
        InitPool();
        StartCoroutine(AddToScene());
    }

    void InitPool()
    {
        foreach(GameObject prefab in prefabs)
        {
            GameObject temp;
            temp=Instantiate(prefab);
            temp.SetActive(false);
            pool.Add(temp);
        }
    }

    void GetFromPool(Enums.Fruits type, Vector3 position)
    {
        for(int i=0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy&&pool[i].CompareTag(type.ToString()))
            {
                pool[i].transform.position = position;
                pool[i].transform.rotation =prefabs[(int)type].transform.rotation;
                pool[i].transform.Rotate(Vector3.up, this.transform.rotation.eulerAngles.y, Space.World);
                //pool[i].transform
                pool[i].SetActive(true);
                return;
            }
        }
        AddToPool((int)type,position);
    }

    void AddToPool(/*UIManager.Fruits*/int type,Vector3 position)
    {
        pool.Add(Instantiate(prefabs[(int)type], position, prefabs[type].transform.rotation));
        pool[pool.Count-1].transform.Rotate(Vector3.up, this.transform.rotation.eulerAngles.y,Space.World);
    }

    public static void Delete(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }


    IEnumerator AddToScene()
    {
        while (isPlay) {
            Vector3 tempPosition = transform.position;
            tempPosition.z += Random.Range(-randomDistance, randomDistance);
            int tempType = Random.Range(0, prefabs.Count);
            GetFromPool((Enums.Fruits)tempType, tempPosition);
            yield return new WaitForSeconds(timeBetween); 
        }
    }

}
