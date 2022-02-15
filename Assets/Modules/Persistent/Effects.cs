using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private static Effects _instance;

    void Start()
    {
        if(_instance != null)
            Destroy(this);

        _instance = this;
    }


    public static void FreezeFrame()
    {
        if (_instance == null)
            throw new Exception("There is no effects script active!");

        _instance.StartCoroutine(FreezeFrameEnumerator());
    }

    static IEnumerator FreezeFrameEnumerator()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1;
    }
}
