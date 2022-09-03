using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainLoop : MonoBehaviour
{
    int nbDay = 1;
    public int maxDay = 5;

    public TextMeshProUGUI DayTxt;
    public Timer MainTimer;
    public MotivationBar motiv;
    public GameObject Dialogue;

    public CameraAnimationController camControl;

    public bool endingDay;
    public bool beginingDay;

    //State variable for animation
    bool isRightPC;
    bool isPauseClope;


    //Animation State
    const string CAMERA_GO_MENU = "Menu";
    const string CAMERA_GO_PAUSE = "Pause";
    const string CAMERA_GO_UN_PAUSE = "Unpause";
    const string CAMERA_GO_PC_RIGHT = "RightPc";
    const string CAMERA_GO_PC_UN_RIGHT = "UnRightPc";
    const string CAMERA_GO_PC_LEFT = "LeftPc";
    const string CAMERA_GO_PC_UN_LEFT = "UnLeftPc";
    const string CAMERA_GO_PLAY = "Play";
    const string CAMERA_GO_IDLE = "Idle";

    //Constante
    List<int> dayWithMiniGame = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        camControl.ChangeAnimation(CAMERA_GO_PLAY);
        dayWithMiniGame.Add(1);
        dayWithMiniGame.Add(3);
        dayWithMiniGame.Add(5);
        camControl.ChangeAnimation(CAMERA_GO_PC_LEFT);
        isRightPC = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (beginingDay == true)
        {
            if (camControl.ReadyNextAnimation() == true)
            {
                beginingDay = false;
                if (nbDay >= maxDay)
                {
                    EndGame();

                }
                else {
                    nbDay += 1;

                    if (dayWithMiniGame.Contains(nbDay % 5)){
                        camControl.ChangeAnimation(CAMERA_GO_PC_LEFT);
                        isRightPC = false;
                    }
                    else{
                        camControl.ChangeAnimation(CAMERA_GO_PC_RIGHT);
                        isRightPC = true;
                    }

                    MainTimer.reset();
                    DayTxt.text = nbDay.ToString();
                }
            }
        }
        if (endingDay == true){
            if (camControl.ReadyNextAnimation() == true)
            {
                beginingDay = true;
                endingDay = false;
            }
        }
    }

    public void NewDay()
    {

        if (isRightPC == true)
        {
            camControl.ChangeAnimation(CAMERA_GO_PC_UN_RIGHT);
        }
        else if (isPauseClope == true)
        {
            camControl.ChangeAnimation(CAMERA_GO_UN_PAUSE);
            isPauseClope = false;
        }
        else
        {
            camControl.ChangeAnimation(CAMERA_GO_PC_UN_LEFT);
        }
        endingDay = true;
    }

    void EndGame()
    {
        Debug.Log("Fin de parti");
    }

    public void GoClope()
    {
        StartCoroutine(UpdateCamClope());
    }

    public void UnClope()
    {
        StartCoroutine(UnUpdateCamClope());
    }

    public void NextDialogue()
    {
        UnClope();
    }

    IEnumerator UnUpdateCamClope()
    {
        Dialogue.SetActive(false);
        camControl.ChangeAnimation(CAMERA_GO_UN_PAUSE);
        yield return new WaitForSeconds(1);
        if (isRightPC){
            camControl.ChangeAnimation(CAMERA_GO_PC_RIGHT);
        }
        else
        {
            camControl.ChangeAnimation(CAMERA_GO_PC_LEFT);
        }
        isPauseClope = false;
        yield return new WaitForSeconds(1);
        motiv.InverseMotivationLose();
        MainTimer.ChangeFreeze();
    }

    IEnumerator UpdateCamClope()
    {
        MainTimer.ChangeFreeze();
        if (isRightPC){
            camControl.ChangeAnimation(CAMERA_GO_PC_UN_RIGHT);
        }
        else
        {
            camControl.ChangeAnimation(CAMERA_GO_PC_UN_LEFT);
        }
        yield return new WaitForSeconds(1);

        camControl.ChangeAnimation(CAMERA_GO_PAUSE);
        isPauseClope = true;
        yield return new WaitForSeconds(2.5f);
        motiv.InverseMotivationLose();
        Dialogue.SetActive(true);
    }
}
