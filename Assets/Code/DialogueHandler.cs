
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;


public class DialogueLine
{
    public string currentId;
    public string currentPersonnage;
    public string currentSentence;

    public DialogueLine(string newCurrentId, string newCurrentPersonnage, string newCurrentSetences)
    {
        currentId = newCurrentId;
        currentPersonnage = newCurrentPersonnage;
        currentSentence = newCurrentSetences;
    }
}

public class DialogueHandler : MonoBehaviour
{
    public MainLoop main;

    //Source
    public TextAsset csvFile;

    List<DialogueLine> allDialogue = new List<DialogueLine>();
    //TODO comment randomiser les dialogues ?

    void Start()
    {
	    bool header = true;
        string[] linesInFile = csvFile.text.Split('\n');

        foreach (string line in linesInFile)
        {
            if (line != "")
            {
                string[] all_value = SplitCsvLine(line);

                if (header == false)
                {
                    allDialogue.Add(new DialogueLine(all_value[0], all_value[1], all_value[2]));
                }
                else{header=false;}
            }
        }
    }

    public void NextDialogue()
    {
        main.UnClope();
    }

	public string[] SplitCsvLine(string line)
	{
	    return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}

}
