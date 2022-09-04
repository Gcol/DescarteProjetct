using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainLoop : MonoBehaviour
{
    int nbDay = 1;
    public int maxDay = 5;
    public int currentNbClope ;

    public bool FreezeTimer;

    public TextMeshProUGUI DayTxt;
    public Timer MainTimer;
    public MotivationBar motiv;
    public GameObject Dialogue;

    public DialogueHandler dialHandler;

    // Game Object as desactiver en fonction des Zoom de Camera
    public GameObject AffichageQCM;
    public GameObject motivationBar;


    public QcmHandler currentQcm;

    // Animator utiliser pour lancer les animation
    public CameraAnimationController camControl;
    public CameraAnimationController fadeController;
    public CameraAnimationController cadreController;
    public CameraAnimationController textCadreController;


    //Variable pour gerer les cas d overlap d animation peux etre remplacer par des co Routines
    public bool endingDay;
    public bool beginingDay;

    //State variable for animation
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

    //Constate
    const int maxClope = 3;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoRoutineStart());
    }

    IEnumerator CoRoutineStart()
    {
        currentNbClope = 1;
        Dialogue.GetComponent<Image>().enabled = false;
        Dialogue.GetComponent<Button>().enabled = false;
        fadeController.ChangeAnimation("FadeIn");
        cadreController.ChangeAnimation("TextFadeIn");
        textCadreController.ChangeAnimation("TextFadeIn");
        yield return new WaitForSeconds(2);
        motivationBar.SetActive(true);
        camControl.ChangeAnimation(CAMERA_GO_PLAY);
        yield return new WaitForSeconds(1);
        StartCoroutine(CoRoutineActiveLeftPc(true));
        yield return new WaitForSeconds(1);
        MainTimer.SetTimer(true);
    }

    IEnumerator CoRoutineActiveLeftPc(bool starting)
    {
        camControl.ChangeAnimation(CAMERA_GO_PC_LEFT);
        if (starting == true)
        {
            currentQcm.ShuffleList();
            currentQcm.StartMiniGame();
        }
        yield return new WaitForSeconds(1);
        AffichageQCM.SetActive(true);
        motiv.unfreezeMotivation();

    }

    IEnumerator CoRoutineUnActiveLeftPc()
    {
        motiv.freezeMotivation();
        AffichageQCM.SetActive(false);
        camControl.ChangeAnimation(CAMERA_GO_PC_UN_LEFT);
        yield return new WaitForSeconds(1);
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
                    // On va toujours sur le PC de gauche
                    StartCoroutine(CoRoutineActiveLeftPc(true));
                    MainTimer.SetTimer(true);
                }
            }
        }
    }

    public void NewDay()
    {
        StartCoroutine(CoRoutineNewDay());
    }

    IEnumerator CoRoutineNewDay()
    {
        currentNbClope = 1;
        endingDay = true;
        if (isPauseClope == true)
        {
            camControl.ChangeAnimation(CAMERA_GO_UN_PAUSE);
            isPauseClope = false;
        }
        else
        {
            StartCoroutine(CoRoutineUnActiveLeftPc());
        }
        yield return new WaitForSeconds(2);
        nbDay += 1;
        DayTxt.text = nbDay.ToString();
        motivationBar.SetActive(false);
        fadeController.ChangeAnimation("FadeOut");
        cadreController.ChangeAnimation("TextFadeOut");
        textCadreController.ChangeAnimation("TextFadeOut");
        MainTimer.reset();
        yield return new WaitForSeconds(3);
        fadeController.ChangeAnimation("FadeIn");
        cadreController.ChangeAnimation("TextFadeIn");
        textCadreController.ChangeAnimation("TextFadeIn");
        motivationBar.SetActive(true);
        yield return new WaitForSeconds(1);
        endingDay = false;
        beginingDay = true;
    }

    void EndGame()
    {
        Debug.Log("Fin de parti");
    }

    public void GoClope()
    {
        if (endingDay == false && currentNbClope < maxClope){StartCoroutine(UpdateCamClope());}
    }

    public void UnClope()
    {
        currentNbClope += 1;
        StartCoroutine(UnUpdateCamClope());
        motiv.pozClopeMotivBoost();
    }

    IEnumerator UnUpdateCamClope()
    {
        AffichageQCM.SetActive(false);
        Dialogue.GetComponent<Image>().enabled = false;
        Dialogue.GetComponent<Button>().enabled = false;
        camControl.ChangeAnimation(CAMERA_GO_UN_PAUSE);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(CoRoutineActiveLeftPc(false));
        isPauseClope = false;
        yield return new WaitForSeconds(1);
        motiv.unfreezeMotivation();
        MainTimer.SetTimer(true);
    }

    IEnumerator UpdateCamClope()
    {
        MainTimer.SetTimer(false);
        StartCoroutine(CoRoutineUnActiveLeftPc());
        yield return new WaitForSeconds(1);

        camControl.ChangeAnimation(CAMERA_GO_PAUSE);
        isPauseClope = true;
        yield return new WaitForSeconds(2.5f);
        dialHandler.NextDialogue();
        Dialogue.GetComponent<Image>().enabled = true;
        Dialogue.GetComponent<Button>().enabled = true;

    }
}
