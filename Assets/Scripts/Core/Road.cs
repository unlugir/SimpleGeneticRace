using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Road : MonoBehaviour
{
    [SerializeField] GameObject finishLine;
    
    public void EnableFinishLine(){
        finishLine.SetActive(true);
    }
    public void DisableFinishLine(){
        finishLine.SetActive(false);
    }
}