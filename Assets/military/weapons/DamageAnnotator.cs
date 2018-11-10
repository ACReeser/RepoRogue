using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnnotator : MonoBehaviour
{
    public Transform textPrefab;
    public Transform PoolParent;

    private const int MaxText = 100;
    private float[] TextLifetimes = new float[MaxText];
    private Queue<int> inactiveTextIndexes = new Queue<int>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        foreach (Transform child in PoolParent)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                MoveText(child, index);
            }
            index++;
        }
    }

    private void MoveText(Transform bullet, int bulletIndex)
    {
        if (TextLifetimes[bulletIndex] < 0f)
        {
            //bullet.GetChild(0).GetComponent<TrailRenderer>().Clear();
            bullet.gameObject.SetActive(false);
            inactiveTextIndexes.Enqueue(bulletIndex);
        }
        else
        {
            TextLifetimes[bulletIndex] -= Time.deltaTime;
            bullet.Translate(Vector3.up * Time.deltaTime, Space.Self);
        }
    }

    internal void ShowText(Vector3 pos, string text)
    {
        int textIndex;
        Transform newText = null;
        if (inactiveTextIndexes.Count > 0)
        {
            textIndex = inactiveTextIndexes.Dequeue();
            newText = PoolParent.GetChild(textIndex);
            //UnityEngine.Debug.Log("reusing bullet number "+bulletIndex);
        }
        else
        {
            newText = GameObject.Instantiate(textPrefab, PoolParent);
            textIndex = PoolParent.childCount;
            //newText.GetComponent<Bullet>().OnBulletCollide += this.OnBulletCollide;
        }
        TextLifetimes[textIndex] = 3f;
        //newText.gameObject.SetActive(false);
        newText.position = pos + new Vector3(UnityEngine.Random.Range(0f, .75f), 0, UnityEngine.Random.Range(0f, .75f));
        newText.SetSiblingIndex(textIndex);
        newText.GetComponent<TextMesh>().text = text;
        newText.gameObject.SetActive(true);
    }
}
