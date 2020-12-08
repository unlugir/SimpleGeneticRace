using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CanvasController : MonoBehaviour {
    [SerializeField] Text iterationText;
    [SerializeField] Text aliveText;
    [SerializeField] Text bestTimeText;
    [SerializeField] Text raceModeText;
    [SerializeField] Image bestCarImage;
    [SerializeField] Color successColor;
    public void SetIteration(int iteration){
        iterationText.text = $"Iteration: {iteration}";
    }
    public void SetAlive(int alive){
        aliveText.text = $"Alive: {alive}";
    }
    public void SetBestColor(Color c){
        bestCarImage.color = c;
    }
    public void SetBestTime(float time, bool failed){
        bestTimeText.text = $"Best Time: {System.Math.Round(time,3)}";
        if (failed) bestTimeText.color = Color.red;
        else bestTimeText.color = successColor;
    }
    public void RaceMode_Click(){
        bool mode = GameManager.Instance.carsController.SwitchRaceMode();
        if (mode){
            raceModeText.text = "Disable Race Mode";
        }
        else{
            raceModeText.text = "Enable Race Mode";
        }
    }
}