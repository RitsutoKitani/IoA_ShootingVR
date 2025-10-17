using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDel : MonoBehaviour
{
    public void DataDelete()
    {
        DataManager.instance.DataReset();
        StageManager.instance.Fade.StartFadeIn(0);
    }
}
