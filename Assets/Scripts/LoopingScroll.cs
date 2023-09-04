// used to scroll text on the main menu screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingScroll : MonoBehaviour
{
    // 0 = x, 1 = y, 2 = z
    [SerializeField] int axis;
    // determines looping point
    [SerializeField] float scrollDistance;
    // scroll speed on each axis
    [SerializeField] float scrollSpeed;
    [SerializeField] float offset;

    private Vector3 startPos;
    private Vector3 newPos;

    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        newPos = rt.anchoredPosition;
        newPos[axis] = (startPos[axis]) +  ((Time.time * scrollSpeed + offset) % scrollDistance);
        rt.anchoredPosition = newPos;
    }
}
