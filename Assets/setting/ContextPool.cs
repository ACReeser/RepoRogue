using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextPool : MonoBehaviour
{
    public Transform textPrefab;
    public float BobHeight;

    private const int MaxContexts = 5;
    private float[] TextPhase = new float[MaxContexts];
    private Queue<int> inactiveTexts = new Queue<int>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        foreach (Transform child in this.transform)
        {
            if (child != null && child.gameObject.activeInHierarchy)
            {
                BobText(child, index);
            }
            index++;
        }
    }

    private void BobText(Transform text, int index)
    {
        //int bulletIndex = bullet.GetSiblingIndex();
        if (TextPhase[index] > 1f)
        {
            TextPhase[index] = 0;
        } else
        {
            TextPhase[index] += Time.deltaTime;
        }
        text.Translate(Vector3.up * BobHeight * Mathf.Sin(Mathf.Lerp(0, Mathf.PI * 2f, Mathf.Clamp(TextPhase[index], 0f, 1f))));
    }

    internal int ShowContext(Vector3 startPos)
    {
        int index;
        Transform newText = null;
        if (inactiveTexts.Count > 0)
        {
            index = inactiveTexts.Dequeue();
            newText = this.transform.GetChild(index);
        }
        else
        {
            index = this.transform.childCount;
            newText = GameObject.Instantiate(textPrefab, this.transform);
        }
        TextPhase[index] = 0f;
        newText.position = startPos;
        //newText.rotation = startRot;
        newText.SetSiblingIndex(index);
        newText.gameObject.SetActive(true);

        return index;
    }

    internal void HideContext(int index)
    {
        if (index < transform.childCount)
        {
            var text = transform.GetChild(index);
            text.gameObject.SetActive(false);
            inactiveTexts.Enqueue(index);
        }
    }
}
