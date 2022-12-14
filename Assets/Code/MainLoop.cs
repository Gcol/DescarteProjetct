using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainLoop : MonoBehaviour
{

    public int nbDay = 1;
    public int maxDay = 5;
    int currentNbClope;

    public bool FreezeTimer;


    public TextMeshProUGUI DayTxt;
    public Timer MainTimer;
    public MotivationBar motiv;
    public GameObject Dialogue;

    public DialogueHandler dialHandler;
    public IntrOutro introutro;
    public GameObject Intro;
    public GameObject Extro;

    // Game Object as desactiver en fonction des Zoom de Camera
    public GameObject AffichageQCM;
    public GameObject motivationBar;

    public QcmHandler currentQcm;

    // Animator utiliser pour lancer les animation d'entre niveau
    public CameraAnimationController camControl;
    public CameraAnimationController fadeController;
    public CameraAnimationController cadreController;
    public CameraAnimationController textCadreController;

    public CameraAnimationController maxScoreController;
    public CameraAnimationController scoreInfoController;
    public CameraAnimationController currentScoreController;

    // Animator utiliser pour lancer les animations des objets sur la scene
    public CameraAnimationController clopAnimation;


    // Animator pour l'extro
    public CameraAnimationController fadeExtroController;
    public CameraAnimationController cadreExtroController;
    public CameraAnimationController textExtroCadreController;


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
    public int maxClope = 3;

    // Start is called before the first frame update
    void Start()
    {
        Extro.SetActive(false);
        endingDay = true;
        introutro.ActivePannel();
    }

    public void LaunchGame()
    {
        StartCoroutine(CoRoutineStart());
    }

    IEnumerator CoRoutineStart()
    {
        Intro.SetActive(true);
        FadeOutAnimation();
        yield return new WaitForSeconds(2);
        clopAnimation.ChangeAnimation("Clope3");
        currentNbClope = maxClope;
        Dialogue.SetActive(true);
        Dialogue.GetComponent<Image>().enabled = false;
        Dialogue.GetComponent<Button>().enabled = false;
        FadeInAnimation();
        yield return new WaitForSeconds(2);
        motivationBar.SetActive(true);
        camControl.ChangeAnimation(CAMERA_GO_PLAY);
        yield return new WaitForSeconds(1);
        StartCoroutine(CoRoutineActiveLeftPc(true));
        yield return new WaitForSeconds(1);
        MainTimer.SetTimer(true);
        endingDay = false;
    }

    void FadeInAnimation()
    {
        fadeController.ChangeAnimation("FadeIn");
        cadreController.ChangeAnimation("TextFadeIn");
        maxScoreController.ChangeAnimation("TextFadeIn");
        scoreInfoController.ChangeAnimation("TextFadeIn");
        textCadreController.ChangeAnimation("TextFadeIn");
        currentScoreController.ChangeAnimation("TextFadeIn");
    }

    void FadeOutAnimation()
    {
        fadeController.ChangeAnimation("FadeOut");
        cadreController.ChangeAnimation("TextFadeOut");
        maxScoreController.ChangeAnimation("TextFadeOut");
        scoreInfoController.ChangeAnimation("TextFadeOut");
        textCadreController.ChangeAnimation("TextFadeOut");
        currentScoreController.ChangeAnimation("TextFadeOut");
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
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (beginingDay == true)
        {
            if (camControl.ReadyNextAnimation() == true)
            {
                beginingDay = false;
                if (nbDay >= maxDay || currentQcm.currentScore == currentQcm.maxScore)
                    StartCoroutine(PreparingOutro());
                else {
                    // On va toujours sur le PC de gauche
                    StartCoroutine(CoRoutineActiveLeftPc(true));
                    MainTimer.SetTimer(true);
                }
            }
        }
    }

    IEnumerator PreparingOutro()
    {
        Dialogue.SetActive(false);
        motivationBar.SetActive(false);
        camControl.ChangeAnimation(CAMERA_GO_MENU);
        yield return new WaitForSeconds(2);
        introutro.ActivePannel();
    }

    public void NewDay()
    {
        StartCoroutine(CoRoutineNewDay());
    }

    IEnumerator CoRoutineNewDay()
    {
        currentNbClope = maxClope;
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
        FadeOutAnimation();
        yield return new WaitForSeconds(3);
        MainTimer.reset();
        FadeInAnimation();
        motivationBar.SetActive(true);
        clopAnimation.ChangeAnimation("Clope3");
        yield return new WaitForSeconds(1);
        endingDay = false;
        beginingDay = true;
    }

    public void EndGame()
    {
        StartCoroutine(CloseGame());
    }

    IEnumerator CloseGame(){
        Extro.SetActive(true);
        fadeExtroController.ChangeAnimation("FadeOut");
        cadreExtroController.ChangeAnimation("TextFadeOut");
        textExtroCadreController.ChangeAnimation("TextFadeOut");
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

    public void GoClope()
    {
        if (endingDay == false && currentNbClope >= 0){
            currentNbClope -= 1;
            if (currentNbClope == 2) clopAnimation.ChangeAnimation("Clope2");
            if (currentNbClope == 1) clopAnimation.ChangeAnimation("Clope1");
            if (currentNbClope == 0) clopAnimation.ChangeAnimation("Clope0");
            StartCoroutine(UpdateCamClope());
        }
    }

    public void UnClope()
    {
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
