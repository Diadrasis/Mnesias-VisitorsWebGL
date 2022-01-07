using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Linq;
using System.IO;

public class StringManager : MonoBehaviour
{
    #region Variables
    //for csv load,all the variables we are going to need
    [Space]
    [Header("Main Objects")]
    public TextAsset mainfileS;
    SInfoCSV infoS;
    List<RowS> rowListS = new List<RowS>();
    RowS rowS = new RowS();
    string[][] grid;

    //the text inputs we are going to change throught the app
    [Space]
    [Header("Instrument Main Input")]
    public TMP_InputField txtBodyMaterial;
    public TMP_InputField txtWayOfPlaying;
    public TMP_InputField txtChordNum;

    //the panel with the group buttons we will change values
    [Space]
    [Header("Change Values Panel Menu")]
    public GameObject pnlChangeValuesScreen;
    public Button btnLengthChange;
    public Button btnTensionChange;
    public Button btnDensityChange;


    [Space]
    [Header("Objects to Output Values")]
    public TMP_InputField[] lengthInputs; //length inputs for both sets and show the values
    public TMP_InputField[] tensionInputs;//tension inputs for both sets and show the values
    public TMP_InputField[] densityInput;//density inputs for both sets and show the values
    public Toggle[] togglesLengthInput; //the toggles for set1 and set2 for length
    public Toggle[] togglesDensityInput;//the toggles for set1 and set2 for density
    public Toggle[] togglesTensionInput;//the toggles for set1 and set2 for tension


    public ToggleGroup group;

    private bool isCorrect;

    public MainPCManager mpm;
    #endregion

    #region UnityMethods
    // Start is called before the first frame update
    void Awake()
    {
        pnlChangeValuesScreen.SetActive(false);
    }

    private void Start()
    {
        //CloseListOfGameObjects();

        infoS = gameObject.AddComponent<SInfoCSV>();
        Load(mainfileS);
        SubscribeToButtons();
        
    }
    #endregion

    #region Methods
    
    //assing methods to specific buttons
    void SubscribeToButtons()
    {
        btnLengthChange.onClick.AddListener(ActivateLengthTogglesAndInputs);
        btnTensionChange.onClick.AddListener(ActivateTensionTogglesAndInputs);
        btnDensityChange.onClick.AddListener(ActivateDensityTogglesAndInputs);
    }

    #region CSV
    /*<summary>
     * CSV region to load the csv and then check for each group combination in order to load the respected sounds
     * </summary>*/

    //load the csv file
    public void Load(TextAsset csv)
    {
        rowListS.Clear();
        grid = CsvParser2.Parse(csv.text);
        for (int i = 1; i < grid.Length; i++)
        {
            rowS.bodyMaterial = grid[i][0];
            rowS.wayOfPlaying = grid[i][1];
            rowS.numOfChords = grid[i][2];
            rowS.chords = grid[i][3];
            rowS.length = grid[i][4];
            rowS.tension = grid[i][5];
            rowS.density = grid[i][6];
            rowS.folderS = grid[i][7];
            rowS.audioFile = grid[i][8];

            rowListS.Add(rowS);
        }
        infoS.isLoaded = true;
    }

    //to load the values of a specific line from the csv file and show them
    public void OpenValues(int num)
    {
        for (int i = num; i < grid.Length; i++)
        {
            rowS.bodyMaterial = grid[i][0];
            rowS.wayOfPlaying = grid[i][1];
            rowS.numOfChords = grid[i][2];
            rowS.chords = grid[i][3];
            rowS.length = grid[i][4];
            rowS.tension = grid[i][5];
            rowS.density = grid[i][6];
            rowS.folderS = grid[i][7];
            rowS.audioFile = grid[i][8];

            rowListS.Add(rowS);
            

            if (grid[i][4] != grid[num][4] && grid[i][5]!= grid[num][5] && grid[i][6] != grid[num][6])
            {
                
                if (rowS.folderS != string.Empty)
                {

                    txtBodyMaterial.text = rowS.bodyMaterial;
                    txtWayOfPlaying.text = rowS.wayOfPlaying;
                    txtChordNum.text = rowS.numOfChords;

                    densityInput[i - (num + 1)].text = rowS.density;
                    tensionInputs[i - (num + 1)].text = rowS.tension;
                    lengthInputs[i - (num + 1)].text = rowS.length;
                    
                    mpm.myAudio.LoadAudio(rowS.folderS + "/");
                    
                }
            }
            if (mpm.isLyra && !mpm.isTrigono)
            {
                if (i == num + 9)
                {
                    Debug.Log("With 9 rows");
                    return;
                }

            }
            if (mpm.isTrigono && !mpm.isLyra)
            {
                if (i == num + 26)
                    return;
            }


        }
    }
    #endregion

    #region Panels
    
    //activate the panel to select which go to change the value
    public void OpenChangeValuesPanelString()
    {
        pnlChangeValuesScreen.SetActive(true);
        RemoveTextEffect();
        CloseListOfGameObjectsTrigono();
        mpm.pnlBottom.SetActive(false);
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();
    }

    #endregion

    #region GameObjects
    /*<summary>
     * Here is for all the game objects we use in our scene and on string instruments. To open, close and change their values according to csv.
     * </summary>*/

    //on load scene all extra go's are inactive and will be open after their respective selection
    public void CloseListOfGameObjectsTrigono()
    {
        CloseToggles();
        if(mpm.isTrigono && !mpm.isLyra)
        {
            for (int j = 0; j < lengthInputs.Length - 26; j++)
            {
                lengthInputs[j + 26].gameObject.SetActive(false);
            }
            for (int k = 0; k < tensionInputs.Length - 26; k++)
            {
                tensionInputs[k + 26].gameObject.SetActive(false);
            }

            for (int m = 0; m < densityInput.Length - 26; m++)
            {
                densityInput[m + 26].gameObject.SetActive(false);
            }
        }
        else if(mpm.isLyra && !mpm.isTrigono)
        {
            for (int j = 35; j < lengthInputs.Length; j++)
            {
                lengthInputs[j].gameObject.SetActive(false);
                lengthInputs[j].transform.parent.gameObject.SetActive(false);

                //to remove the 2nd set of gameObjects on each object type list
                for (int i = 10; i < lengthInputs.Length; i++) lengthInputs[i].gameObject.SetActive(false);

            }
            for (int k = 35; k < tensionInputs.Length; k++)
            {
                tensionInputs[k].gameObject.SetActive(false);
                tensionInputs[k].transform.parent.gameObject.SetActive(false);
                for (int n = 10; n < tensionInputs.Length; n++) tensionInputs[n].gameObject.SetActive(false);

            }

            for (int m = 35; m < densityInput.Length; m++)
            {
                densityInput[m].gameObject.SetActive(false);
                densityInput[m].transform.parent.gameObject.SetActive(false);
                for (int b = 10; b < densityInput.Length; b++) densityInput[b].gameObject.SetActive(false);

            }
        }
        
        
    }

    //when on submit or in the first time, or reset the toggles are closed on each group
    void CloseToggles()
    {
        for (int i = 0; i < togglesLengthInput.Length; i++)
        {
            togglesLengthInput[i].gameObject.SetActive(false);
        }
        for (int n = 0; n < togglesDensityInput.Length; n++)
        {
            togglesDensityInput[n].gameObject.SetActive(false);
        }
        for (int p = 0; p < togglesTensionInput.Length; p++)
        {
            togglesTensionInput[p].gameObject.SetActive(false);
        }
    }
    

    //when selecting length values this is where we enable the gameObjects (toggles, inputs, etc)
    private void ActivateLengthTogglesAndInputs()
    {
        mpm.ButtonsWhenChangeValues();
        if (pnlChangeValuesScreen.activeSelf)
        {
            pnlChangeValuesScreen.SetActive(false);
        }
        for (int i = 0; i < togglesLengthInput.Length; i++)
        {
            togglesLengthInput[i].gameObject.SetActive(true);
            togglesDensityInput[i].gameObject.SetActive(false);
            togglesDensityInput[i].isOn = false;
            togglesTensionInput[i].gameObject.SetActive(false);
            togglesTensionInput[i].isOn = false;
        }
        if (mpm.isTrigono && !mpm.isLyra)
        {
            for (int j = 0; j < lengthInputs.Length - 26; j++)
            {
                lengthInputs[j + 26].gameObject.SetActive(true);

            }
            for (int m = 0; m < densityInput.Length - 26; m++)
            {
                densityInput[m].gameObject.SetActive(true);
                densityInput[m + 26].gameObject.SetActive(false);
                densityInput[m].text = grid[2 + m][6];
            }
            for (int k = 0; k < tensionInputs.Length - 26; k++)
            {
                tensionInputs[k].gameObject.SetActive(true);
                tensionInputs[k + 26].gameObject.SetActive(false);
                tensionInputs[k].text = grid[2 + k][5];
            }

        }
        else if (!mpm.isTrigono && mpm.isLyra)
        {
            
            for (int j = 26; j < lengthInputs.Length - 35; j++)
            {
                lengthInputs[j + 35].gameObject.SetActive(true);
                lengthInputs[j + 35].transform.parent.gameObject.SetActive(true);

            }
            for (int m = 26; m < densityInput.Length - 35; m++)
            {
                densityInput[m].gameObject.SetActive(true);
                densityInput[m + 35].gameObject.SetActive(false);
                densityInput[m].text = grid[2 + m][6];
            }
            for (int k = 26; k < tensionInputs.Length - 35; k++)
            {
                tensionInputs[k].gameObject.SetActive(true);
                tensionInputs[k + 35].gameObject.SetActive(false);
                tensionInputs[k].text = grid[2 + k][5];
            }
        }
        ShowLenghtExtraValues();
        mpm.pnlBottom.SetActive(true);
#if PLATFROM_ANDROID || PLATFROM_IOS
        mpm.ms.btnPlayPanel.gameObject.SetActive(false);
#endif
        mpm.btnChangeValue.gameObject.SetActive(false);
    }

    //when selecting density values this is where we enable the gameObjects (toggles, inputs, etc)
    private void ActivateDensityTogglesAndInputs()
    {
        mpm.ButtonsWhenChangeValues();
        if (pnlChangeValuesScreen.activeSelf)
        {
            pnlChangeValuesScreen.SetActive(false);
        }

        for (int n = 0; n < togglesDensityInput.Length; n++)
        {
            togglesDensityInput[n].gameObject.SetActive(true);
            togglesLengthInput[n].gameObject.SetActive(false);
            togglesLengthInput[n].isOn = false;
            togglesTensionInput[n].gameObject.SetActive(false);
            togglesTensionInput[n].isOn = false;
        }

        if (mpm.isTrigono && !mpm.isLyra)
        {
            

            for (int m = 0; m < densityInput.Length - 26; m++)
            {
                densityInput[m + 26].gameObject.SetActive(true);
            }

            for (int k = 0; k < tensionInputs.Length - 26; k++)
            {
                tensionInputs[k].gameObject.SetActive(true);
                tensionInputs[k + 26].gameObject.SetActive(false);
                tensionInputs[k].text = grid[2 + k][5];
            }
            for (int j = 0; j < lengthInputs.Length - 26; j++)
            {
                lengthInputs[j].gameObject.SetActive(true);
                lengthInputs[j + 26].gameObject.SetActive(false);
                lengthInputs[j].text = grid[2 + j][4];
            }
        }
        else if (!mpm.isTrigono && mpm.isLyra)
        {
           
            for (int j = 26; j < densityInput.Length - 35; j++)
            {
                densityInput[j + 35].gameObject.SetActive(true);
                densityInput[j + 35].transform.parent.gameObject.SetActive(true);

            }
            for (int m = 26; m < lengthInputs.Length - 35; m++)
            {
                lengthInputs[m].gameObject.SetActive(true);
                lengthInputs[m + 35].gameObject.SetActive(false);
                lengthInputs[m].text = grid[2 + m][4];
            }
            for (int k = 26; k < tensionInputs.Length - 35; k++)
            {
                tensionInputs[k].gameObject.SetActive(true);
                tensionInputs[k + 35].gameObject.SetActive(false);
                tensionInputs[k].text = grid[2 + k][5];
            }
        }
        mpm.pnlBottom.SetActive(true);
#if PLATFROM_ANDROID || PLATFROM_IOS
        mpm.ms.btnPlayPanel.gameObject.SetActive(false);
#endif
        ShowDensityExtraValues();
        mpm.btnChangeValue.gameObject.SetActive(false);
    }

    //when selecting tension values this is where we enable the gameObjects (toggles, inputs, etc)
    private void ActivateTensionTogglesAndInputs()
    {
        mpm.ButtonsWhenChangeValues();
        if (pnlChangeValuesScreen.activeSelf)
        {
            pnlChangeValuesScreen.SetActive(false);
        }
        for (int p = 0; p < togglesTensionInput.Length; p++)
        {
            togglesTensionInput[p].gameObject.SetActive(true);
            togglesDensityInput[p].gameObject.SetActive(false);
            togglesDensityInput[p].isOn = false;
            togglesLengthInput[p].gameObject.SetActive(false);
            togglesLengthInput[p].isOn = false;
        }
        if (mpm.isTrigono && !mpm.isLyra)
        {

            for (int k = 0; k < tensionInputs.Length - 26; k++)
            {
                tensionInputs[k + 26].gameObject.SetActive(true);
            }
            for (int j = 0; j < lengthInputs.Length - 26; j++)
            {
                lengthInputs[j].gameObject.SetActive(true);
                lengthInputs[j + 26].gameObject.SetActive(false);
                lengthInputs[j].text = grid[2 + j][4];
            }
            for (int m = 0; m < densityInput.Length - 26; m++)
            {
                densityInput[m].gameObject.SetActive(true);
                densityInput[m + 26].gameObject.SetActive(false);
                densityInput[m].text = grid[2 + m][6];
            }
        }
        else if (!mpm.isTrigono && mpm.isLyra)
        {
           
            for (int j = 26; j < tensionInputs.Length - 35; j++)
            {
                tensionInputs[j + 35].gameObject.SetActive(true);
                tensionInputs[j + 35].transform.parent.gameObject.SetActive(true);

            }
            for (int m = 26; m < densityInput.Length - 35; m++)
            {
                densityInput[m].gameObject.SetActive(true);
                densityInput[m + 35].gameObject.SetActive(false);
                densityInput[m].text = grid[2 + m][6];
            }
            for (int k = 26; k < lengthInputs.Length - 35; k++)
            {
                lengthInputs[k].gameObject.SetActive(true);
                lengthInputs[k + 35].gameObject.SetActive(false);
                lengthInputs[k].text = grid[2 + k][4];
            }
        }
        mpm.pnlBottom.SetActive(true);
#if PLATFROM_ANDROID || PLATFROM_IOS
        mpm.ms.btnPlayPanel.gameObject.SetActive(false);
#endif
        ShowTensionExtraValues();
        mpm.btnChangeValue.gameObject.SetActive(false);
    }

    //load the 3d model of the string instrument
    public void Load3DIns(GameObject ins)
    {
        ins.SetActive(true);
        mpm.prefab3Dins.SetActive(true);
        mpm.pnlMainStringInstrument.SetActive(false);
        mpm.imgMnesiasBack.gameObject.SetActive(false);


        if (mpm.pnlHelp.activeSelf) mpm.CloseHelpPanel();
        if (mpm.pnlExtraMenu.activeSelf)mpm.CloseMenuPanel();


        mpm.pnlBottom.SetActive(false);
    }
#endregion

#region Values

    /*here we will also load the respective wav files when user presses
       *the submit button with the values they have selected*/
    public void OnSubmitString()
    {
        Debug.Log("String submit");
        mpm.ButtonsWhenSubmitOrReset();
       
        if (togglesLengthInput[0].isOn && mpm.isTrigono)
        {
            rowS.folderS = grid[54][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < lengthInputs.Length-26; i++)
            {
                lengthInputs[i+26].gameObject.SetActive(false);
            }
        }
        else if (togglesLengthInput[1].isOn && mpm.isTrigono)
        {
            rowS.folderS = grid[80][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {
                lengthInputs[i].gameObject.SetActive(false);
            }
        }
        else if(togglesLengthInput[0].isOn && mpm.isLyra)
        {
            rowS.folderS = grid[221][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {
                lengthInputs[i+26].gameObject.SetActive(false);
            }
            Debug.Log("Lyra length set 1");
        }
        else if (togglesLengthInput[1].isOn && mpm.isLyra)
        {
            rowS.folderS = grid[231][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {
                lengthInputs[i].gameObject.SetActive(false);
            }
            
        }

        if (togglesTensionInput[0].isOn && mpm.isTrigono)
        {
            rowS.folderS = grid[106][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for(int i = 0; i < tensionInputs.Length - 26; i++)
            {
                tensionInputs[i+26].gameObject.SetActive(false);
            }
        }
        else if (togglesTensionInput[1].isOn && mpm.isTrigono)
        {
            rowS.folderS = grid[132][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {
                tensionInputs[i].gameObject.SetActive(false);
            }
        }
        else if (togglesTensionInput[0].isOn && mpm.isLyra)
        {
            rowS.folderS = grid[241][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {
                tensionInputs[i + 26].gameObject.SetActive(false);
            }
            Debug.Log("Lyra tension set 1");
        }
        else if (togglesTensionInput[1].isOn && mpm.isLyra)
        {
            rowS.folderS = grid[251][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {
                tensionInputs[i].gameObject.SetActive(false);
            }
            Debug.Log("Lyra tension set 2");
        }

        if (togglesDensityInput[0].isOn && mpm.isTrigono)
        {
            rowS.folderS = grid[158][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < densityInput.Length - 26; i++)
            {
                densityInput[i+26].gameObject.SetActive(false);
            }
        }
        else if (togglesDensityInput[1].isOn && mpm.isTrigono)
        {
            rowS.folderS = grid[184][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < densityInput.Length - 26; i++)
            {
                densityInput[i].gameObject.SetActive(false);
            }
        }
        else if (togglesDensityInput[0].isOn && mpm.isLyra)
        {
            rowS.folderS = grid[261][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < densityInput.Length - 26; i++)
            {
                densityInput[i + 26].gameObject.SetActive(false);
            }
        }
        else if (togglesDensityInput[1].isOn && mpm.isLyra)
        {
            rowS.folderS = grid[271][7];
            mpm.myAudio.LoadAudio(rowS.folderS + "/");
            isCorrect = true;
            for (int i = 0; i < densityInput.Length - 26; i++)
            {
                densityInput[i].gameObject.SetActive(false);
            }
        }

        //to close all toggles
        for (int i = 0; i < togglesLengthInput.Length; i++)
        {
            togglesLengthInput[i].gameObject.SetActive(false);
            togglesDensityInput[i].gameObject.SetActive(false);
            togglesTensionInput[i].gameObject.SetActive(false);
        }
        RemoveTextEffect();
        mpm.btnChangeValue.gameObject.SetActive(true);
        Debug.Log("Submit press");
    }

    //is used on each toggle from the editor, when a toggle is selected (on) the text changes from gray to black.
    public void HoleSelection()
    {

        if (togglesLengthInput[0].isOn)
        {

            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {

                foreach (TextMeshProUGUI myText in lengthInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(0, 0, 0, 255);
                }

            }
            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in lengthInputs[i + 26].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(159, 159, 159, 255);

                }
            }

        }
        else if (togglesLengthInput[1].isOn)
        {

            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in lengthInputs[i + 26].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(0, 0, 0, 255);

                }
            }
            for (int i = 0; i < lengthInputs.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in lengthInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(159, 159, 159, 255);

                }
            }
        }
        if (togglesTensionInput[0].isOn)
        {
            //Debug.Log("Is it On: " + togglesChangedInput[2].isOn);
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in tensionInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(0, 0, 0, 255);

                }
            }
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {

                foreach (TextMeshProUGUI myText in tensionInputs[i + 26].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(159, 159, 159, 255);
                }
            }
        }
        else if (togglesTensionInput[1].isOn)
        {
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in tensionInputs[i + 26].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(0, 0, 0, 255);

                }
            }
            for (int i = 0; i < tensionInputs.Length - 26; i++)
            {

                foreach (TextMeshProUGUI myText in tensionInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(159, 159, 159, 255);
                }
            }
        }
        if (togglesDensityInput[0].isOn)
        {
            //Debug.Log("Is it On: " + togglesChangedInput[2].isOn);
            for (int i = 0; i < densityInput.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in densityInput[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(0, 0, 0, 255);

                }
            }
            for (int i = 0; i < densityInput.Length - 26; i++)
            {

                foreach (TextMeshProUGUI myText in densityInput[i + 26].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(159, 159, 159, 255);
                }
            }
        }
        else if (togglesDensityInput[1].isOn)
        {
            for (int i = 0; i < densityInput.Length - 26; i++)
            {
                foreach (TextMeshProUGUI myText in densityInput[i + 26].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(0, 0, 0, 255);

                }
            }
            for (int i = 0; i < densityInput.Length - 26; i++)
            {

                foreach (TextMeshProUGUI myText in densityInput[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    myText.color = new Color32(159, 159, 159, 255);
                }
            }
        }
    }

    //when pressing the submit button, the text effect on the values will be removed also the same goes on reset
    public void RemoveTextEffect()
    {
        //to remove the effect on the selected text
        for (int i = 0; i < tensionInputs.Length; i++)
        {

            foreach (TextMeshProUGUI myText in tensionInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                //myText.color = new Color(0, 0, 0, 255);
                myText.color = new Color32(159, 159, 159, 255);
            }
        }
        for (int i = 0; i < lengthInputs.Length; i++)
        {
            foreach (TextMeshProUGUI myText in lengthInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                myText.color = new Color32(159, 159, 159, 255);

            }
        }
        for (int i = 0; i < densityInput.Length; i++)
        {
            foreach (TextMeshProUGUI myText in densityInput[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                myText.color = new Color32(159, 159, 159, 255);

            }
        }
    }
   
    //show tension extra values, from selecting one of the two toggles (set1, set2)
    public void ShowTensionExtraValues()
    {
        if (mpm.isTrigono)
        {
            for (int k = 0; k < grid.Length; k++)
            {
                if (grid[k][7] != grid[106][7])
                {

                    for (int i = 0; i < tensionInputs.Length - 26; i++)
                    {
                        tensionInputs[i].text = grid[106 + i][5];
                        tensionInputs[i + 26].text = grid[132 + i][5];
                    }
                }

            }
        }
        else
        {
            for (int k = 0; k < grid.Length; k++)
            {
                if (grid[k][7] != grid[240][7])
                {

                    for (int i = 0; i < tensionInputs.Length - 26; i++)
                    {
                        tensionInputs[i].text = grid[241 + i][5];

                        tensionInputs[i + 26].text = grid[251 + i][5];
                        tensionInputs[i + 26].gameObject.SetActive(true);
                        if (i == 8) return;
                    }

                }
            }
           
        }

    }

    //show length extra values, from selecting one of the two toggles (set1, set2)
    public void ShowLenghtExtraValues()
    {
        if (mpm.isTrigono)
        {
            for (int k = 0; k < grid.Length; k++)
            {
                if (grid[k][7] != grid[54][7])
                {
                    for (int i = 0; i < lengthInputs.Length - 26; i++)
                    {
                        lengthInputs[i].text = grid[54 + i][4];
                        lengthInputs[i + 26].text = grid[80 + i][4];
                    }
                }
            }
        }
        else
        {
            for (int k = 0; k < grid.Length; k++)
            {
                if (grid[k][7] != grid[220][7])
                {
                    
                    for (int i = 0; i < lengthInputs.Length - 26; i++)
                    {
                        lengthInputs[i].text = grid[221 + i][4];
                        
                        lengthInputs[i + 26].gameObject.SetActive(true);
                        lengthInputs[i + 26].text = grid[231 + i][4];
                        if (i == 8) return;
                    }

                }
            }
            Debug.Log("Lyra tension set");
        }
        

    }
    
    //show density extra values, from selecting one of the two toggles (set1, set2)
    public void ShowDensityExtraValues()
    {
        if (mpm.isTrigono)
        {
            for (int k = 0; k < grid.Length; k++)
            {
                if (grid[k][7] != grid[158][7])
                {
                    for (int i = 0; i < densityInput.Length - 26; i++)
                    {
                        densityInput[i].text = grid[158 + i][6];
                        densityInput[i + 26].text = grid[184 + i][6];
                    }
                }
            }
        }
        else
        {
            for (int k = 0; k < grid.Length; k++)
            {
                if (grid[k][7] != grid[260][7])
                {

                    for (int i = 0; i < densityInput.Length - 26; i++)
                    {
                        densityInput[i].text = grid[261 + i][6];
                        densityInput[i + 26].gameObject.SetActive(true);
                        densityInput[i+26].text = grid[271 + i][6];
                            
                        if (i == 8) return;
                    }

                }
            }
            Debug.Log("Lyra tension set");
        }
        

    }
    
    //reset values on string instrument
    public void OnResetString()
    {
        mpm.ButtonsWhenSubmitOrReset();

        
        foreach (Toggle toggle in togglesLengthInput) toggle.isOn = false;
        foreach (Toggle tg in togglesDensityInput) tg.isOn = false;
        foreach (Toggle tt in togglesTensionInput) tt.isOn = false;
        foreach(TMP_InputField tMP_ in densityInput) { tMP_.gameObject.SetActive(true);tMP_.transform.parent.gameObject.SetActive(true); tMP_.text = "Πυκνότητα"; }
        foreach(TMP_InputField tMP_1 in tensionInputs) { tMP_1.gameObject.SetActive(true); tMP_1.transform.parent.gameObject.SetActive(true); tMP_1.text = "Τάση"; }
        foreach(TMP_InputField tMP_2 in lengthInputs) { tMP_2.gameObject.SetActive(true); tMP_2.transform.parent.gameObject.SetActive(true); tMP_2.text = "Μήκος"; }
        if(mpm.isTrigono && mpm.isPiraeus && !mpm.isMegara)
        {
            OpenValues(1);
            mpm.isTrigono = true;
            mpm.isLyra = false;
            CloseListOfGameObjectsTrigono();
            Debug.Log("reset trigono");
        }
        else if (mpm.isPiraeus && mpm.isLyra && !mpm.isMegara)
        {
            OpenValues(210);
            mpm.isLyra = true;
            mpm.isTrigono = false;
            CloseListOfGameObjectsTrigono();
            Debug.Log("reset helys");
        }
        RemoveTextEffect();
        
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();

    }

#endregion

#endregion

    
}
