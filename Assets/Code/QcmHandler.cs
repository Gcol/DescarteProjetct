
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class QCMLine
{
    public string difficulty;
    public string question;
    public string reponse1;
    public string reponse2;
    public string reponse3;
    public string reponse4;

    public QCMLine(string new_difficulty, string new_question, string new_reponse1, string new_reponse2, string new_reponse3, string new_reponse4)
    {
        difficulty = new_difficulty;
        question = new_question;
        reponse1 = new_reponse1;
        reponse2 = new_reponse2;
        reponse3 = new_reponse3;
        reponse4 = new_reponse4;
    }
}

public class QcmHandler : MonoBehaviour
{
    //Source
    public TextAsset csvFile;

    //Default_gestion
    public int maxQuestion = 3;
    public int currentDifficulty = 3;

    //pour reset en cas de fin d activite
    public MainLoop main;


    //Text pour modifier les questions
    public TextMeshProUGUI Question;
    public TextMeshProUGUI Rep1;
    public TextMeshProUGUI Rep2;
    public TextMeshProUGUI Rep3;
    public TextMeshProUGUI Rep4;

    //GameObject as cacher si pas assez de réponse
    public GameObject objectRep1;
    public GameObject objectRep2;
    public GameObject objectRep3;
    public GameObject objectRep4;

    //Boolean pour verifier l etat des checkBox
    public bool activeResponse1;
    public bool activeResponse2;
    public bool activeResponse3;
    public bool activeResponse4;

    //CheckBox pour les caché
    public GameObject textResponse1;
    public GameObject textResponse2;
    public GameObject textResponse3;
    public GameObject textResponse4;

    //Pour éviter de nombreuse boucle de recherche je met en dur les difficulté
    List<QCMLine> List_QCMLine_3 = new List<QCMLine>();
    List<QCMLine> List_QCMLine_2 = new List<QCMLine>();
    List<QCMLine> List_QCMLine_1 = new List<QCMLine>();


    //Variable utilitaire
    int indexQuestion;
    public int currentScore = 0;
    public int maxScore = 3;
    List<QCMLine> currentListQcm;

	public void Start()
	{
	    bool header = true;
	    List<QCMLine> List_QCMLine = new List<QCMLine>();

        string[] linesInFile = csvFile.text.Split('\n');

        foreach (string line in linesInFile)
        {
            if (line != "")
            {
                string[] all_value = SplitCsvLine(line);

                if (header == false)
                {
                    string curr_rep2;
                    string curr_rep3;
                    string curr_rep4;
                    string difficulty = all_value[0];

                    if (all_value.Length < 4){curr_rep2 = "";} else {curr_rep2 =  all_value[3];}
                    if (all_value.Length < 5){curr_rep3 = "";} else {curr_rep3 =  all_value[4];}
                    if (all_value.Length < 6){curr_rep4 = "";} else {curr_rep4 =  all_value[5];}

                    if (difficulty == "1")
                    {
                        List_QCMLine_1.Add( new QCMLine(difficulty, all_value[1], all_value[2], curr_rep2, curr_rep3, curr_rep4));
                    }
                    else if (difficulty == "2")
                    {
                        List_QCMLine_2.Add( new QCMLine(difficulty, all_value[1], all_value[2], curr_rep2, curr_rep3, curr_rep4));
                    }
                    else if (difficulty == "3")
                    {
                        List_QCMLine_3.Add( new QCMLine(difficulty, all_value[1], all_value[2], curr_rep2, curr_rep3, curr_rep4));
                    }
                }
                else{header=false;}
            }
        }

	    if (currentDifficulty == 1){currentListQcm = List_QCMLine_1;}
	    if (currentDifficulty == 2){currentListQcm = List_QCMLine_2;}
	    if (currentDifficulty == 3){currentListQcm = List_QCMLine_3;}
	}

	public void StartMiniGame()
	{
	    indexQuestion = 0;
        currentScore = 0;
        updateQuestion();
	}

	void updateQuestion()
	{

       List<int> positionRep= new List<int>{655, 585};


        if (currentListQcm[indexQuestion].reponse3 == ""){
            objectRep3.SetActive(false);
        }
        else{
            objectRep3.SetActive(true);
            positionRep.Add(515);
        }
        if (currentListQcm[indexQuestion].reponse4 == ""){objectRep4.SetActive(false);}
        else
        {
            objectRep4.SetActive(true);
            positionRep.Add(445);
        }

        ShufflePosition(positionRep);

        ChangePositionRep(objectRep1, positionRep[0]);
        ChangePositionRep(objectRep2, positionRep[1]);

        if (currentListQcm[indexQuestion].reponse3 != "") ChangePositionRep(objectRep3, positionRep[2]);
        if (currentListQcm[indexQuestion].reponse4 != "") ChangePositionRep(objectRep4, positionRep[3]);


        Question.text = currentListQcm[indexQuestion].question;
        Rep1.text = currentListQcm[indexQuestion].reponse1;
        Rep2.text = currentListQcm[indexQuestion].reponse2;

        Rep3.text = currentListQcm[indexQuestion].reponse3;
        Rep4.text = currentListQcm[indexQuestion].reponse4;
        resetCheckBox();
	}

    void ChangePositionRep(GameObject currentRep, int newYPosition)
    {
        Vector3 pos = currentRep.transform.position;
        currentRep.transform.position = new Vector3(pos.x, newYPosition, pos.z);
    }

	void resetCheckBox()
	{
        activeResponse1 = false;
        activeResponse2 = false;
        activeResponse3 = false;
        activeResponse4 = false;
        textResponse1.SetActive(false);
        textResponse2.SetActive(false);
        textResponse3.SetActive(false);
        textResponse4.SetActive(false);
	}

	public void CloseMiniGame()
	{
        ShuffleList();
	}

	public void ShuffleList()
	{
        for (int i = 0; i < currentListQcm.Count; i++) {
            QCMLine temp = currentListQcm[i];
            int randomIndex = Random.Range(i, currentListQcm.Count);
            currentListQcm[i] = currentListQcm[randomIndex];
            currentListQcm[randomIndex] = temp;
        }
	}

	public void ShufflePosition(List<int> positionRep)
	{
        for (int i = 0; i < positionRep.Count; i++) {
            int temp = positionRep[i];
            int randomIndex = Random.Range(i, positionRep.Count);
            positionRep[i] = positionRep[randomIndex];
            positionRep[randomIndex] = temp;
        }
	}

	public string[] SplitCsvLine(string line)
	{
	    return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}

    public void setRep1(){
	    activeResponse1 = !activeResponse1;
	    textResponse1.SetActive(activeResponse1);
	}

    public void setRep2(){
	    activeResponse2 = !activeResponse2;
	    textResponse2.SetActive(activeResponse2);
	}

    public void setRep3(){
	    activeResponse3 = !activeResponse3;
	    textResponse3.SetActive(activeResponse3);
	}

    public void setRep4(){
	    activeResponse4 = !activeResponse4;
	    textResponse4.SetActive(activeResponse4);
	}

	public void checkReponse(){
	    if (activeResponse1 == true && activeResponse2 == false && activeResponse3 == false && activeResponse4 == false){currentScore += 1;}
	    indexQuestion += 1;

	    if (indexQuestion < maxQuestion){updateQuestion();}
	    else{main.NewDay();}
	    //TODO voir avec Yann quoi faire sinon
	}
}