
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;


public class DialogueLine
{
    public string currentId;
    public string currentPersonnage;
    public List<string> currentSentence = new List<string>();

    public DialogueLine(string newCurrentId, string newCurrentPersonnage, string[] all_value)
    {
        currentId = newCurrentId;
        currentPersonnage = newCurrentPersonnage;
        for (int index = 2; index < all_value.Length; index++)
        {
            if (all_value[index] != "")
            {
                currentSentence.Add(all_value[index]);
            }
        }
    }
}

public class DialogueHandler : MonoBehaviour
{
    public MainLoop main;

    //Source
    public TextAsset csvFile;

    int indexReadSentence;
    int indexRowSetence;

    public bool kiramAppear;

    public TextMeshProUGUI printDialogue;

    public CameraAnimationController npcDiag;


    List<DialogueLine> allDialogue = new List<DialogueLine>();

    void Start()
    {
        kiramAppear = true;
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
        ShuffleList();

    }

    public void NextDialogue()
    {
        if (indexRowSetence < allDialogue.Count && indexReadSentence < allDialogue[indexRowSetence].currentSentence.Count)
        {
            if (allDialogue[indexRowSetence].currentPersonnage == "Kiram") StartCoroutine(ApparitionKiram());
            else npcDiag.ChangeAnimation("NPCTalk");

            printDialogue.text = allDialogue[indexRowSetence].currentSentence[indexReadSentence];
            indexReadSentence += 1;
        }
        else
        {
            kiramAppear = true;
            printDialogue.text = "";
            main.UnClope();
            npcDiag.ChangeAnimation("Idle");
            if (indexRowSetence >= allDialogue.Count) Debug.Log("Pas assez de dialogue");
            indexRowSetence += 1;
            indexReadSentence = 0;
        }

    }

    IEnumerator ApparitionKiram()
    {
        if (kiramAppear == true)
        {
            npcDiag.ChangeAnimation("RevealKiram");
            yield return new WaitForSeconds(1);
            kiramAppear = false;
        }
        npcDiag.ChangeAnimation("RevealKiramIdle");
    }

	public string[] SplitCsvLine(string line)
	{
	    return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}


    private void ShuffleList()
    {
        for (int i = 0; i < allDialogue.Count; i++) {
            DialogueLine temp = allDialogue[i];
            int randomIndex = Random.Range(i, allDialogue.Count);
            allDialogue[i] = allDialogue[randomIndex];
            allDialogue[randomIndex] = temp;
        }
    }
}
