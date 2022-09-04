
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class IntrOutro : MonoBehaviour
{
    public MainLoop main;
    public QcmHandler qcm;

    //Source
    public TextAsset csvFile;
    public GameObject currentIntroutro;

    bool isIntro;

    int indexReadSentence;
    int indexRowSetence;

    public TextMeshProUGUI printDialogue;

    public CameraAnimationController npcDiag;


    List<DialogueLine> allDialogue = new List<DialogueLine>();

    void Start()
    {
        indexReadSentence = 0;
        indexRowSetence = 0;
	    bool header = true;
        string[] linesInFile = csvFile.text.Split('\n');

        foreach (string line in linesInFile)
        {
            if (line != "")
            {
                string[] all_value = SplitCsvLine(line);

                if (header == false)
                {
                    allDialogue.Add(new DialogueLine(all_value[0], all_value[1], all_value));
                }
                else header=false;
            }
        }
    }

    public void NextDialogue()
    {
        if (qcm.currentScore == qcm.maxScore && indexRowSetence == 1)
        {
            Debug.Log(main.nbDay);
            Debug.Log( main.maxDay);
            if (main.nbDay != main.maxDay) indexRowSetence = 3;
            else indexRowSetence = 2;
        }

        if (indexRowSetence < allDialogue.Count && indexReadSentence < allDialogue[indexRowSetence].currentSentence.Count)
        {
            npcDiag.ChangeAnimation("KiramStandart");
            printDialogue.text = allDialogue[indexRowSetence].currentSentence[indexReadSentence];
            indexReadSentence += 1;
        }
        else
        {
            if (indexRowSetence == 0)
                main.LaunchGame();
            else
                main.EndGame();
            indexRowSetence += 1;
            HidePannel();
        }

    }

    void HidePannel()
    {
        printDialogue.text = "";
        npcDiag.ChangeAnimation("Idle");
        indexReadSentence = 0;
        currentIntroutro.GetComponent<Image>().enabled = false;
        currentIntroutro.GetComponent<Button>().enabled = false;
    }

    public void ActivePannel()
    {
        currentIntroutro.GetComponent<Image>().enabled = true;
        currentIntroutro.GetComponent<Button>().enabled = true;
        NextDialogue();
    }

	public string[] SplitCsvLine(string line)
	{
	    return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}

}
