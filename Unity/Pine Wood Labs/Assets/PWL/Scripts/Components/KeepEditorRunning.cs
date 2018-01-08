using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepEditorRunning : MonoBehaviour
{
    void Awake()
    {
        if (Application.isEditor)
            Application.runInBackground = true;
    }
}
