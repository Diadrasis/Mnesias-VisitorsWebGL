//For the csv files, beware to never leave extra spaces! An error will occur (IndexOutOfRangeException)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Linq;
using System.IO;

public class WindManager : MonoBehaviour
{
    #region Variables
    
    public MainPCManager mpm;//we need to access the Main Manager in order to get methods, variables, etc

    //the below variables are used in order to read from a csv file with all the information we ahve about the instruments and their different combinations
    [Space]
    [Header("Main Objects")]
    public TextAsset mainfile;
    InfoCSV info;
    readonly List<Row> rowList = new List<Row>();
    readonly Row row = new Row();
    string[][] grid;

    //the panels we are going to use in order to show the informatin about the instrument. Text objects to show the values in a text.
    [Space]
    [Header("Instrument Panels")]
    public GameObject pnlChooseType;
    public Button pnlChoose;
    public GameObject pnlAulosScreen; //panel with Aulos settings and info
    public TextMeshProUGUI txtChoiceValue;
    public GameObject txtChoiceValueParent;
    public GameObject txtBoreValueParent;
    public TextMeshProUGUI txtBoreValue;
    public GameObject txtMouthpieceValueParent;
    public TextMeshProUGUI txtMouthpieceValue;
    
    //the extra panel that appears when we want to change a specific group
    [Space]
    [Header("Change Values Panel Menu")]
    public GameObject pnlChangeValuesScreen;
    public Button btnBoreChange;
    public Button btnMouthpieceChange;
    public Button btnHolesChange;


    //after we chose the group, to enable that group to change it, and also show the correct values on each input field we use
    [Space]
    [Header("Objects to Output Values")]
    public TMP_Dropdown dropdownBore = null;
    public TMP_Dropdown dropdownMouthpiece = null;
    public TMP_InputField[] distanceInputs; //to show those inputs in case user chooses to enable the holes
    public Toggle[] togglesChangedInput;
    public TMP_InputField inputValueBoreWeigth;
    public TMP_InputField inputValueBoreDiameter;
    public TMP_InputField inputValueNumOfHoles;
    public TMP_InputField[] inputValueDistanceHole;
    public TMP_InputField[] inputValueDiameterHole;
    public GameObject[] btnSounds;
    public ToggleGroup group;
    
    /*<summary>
     * Main buttons that help us with the navigation and choices in the app
     * </summary>*/
    [Space]
    [Header("On Aulos Buttons")]
    public Button btnBarrytone;
    public Button btnLightTone;

    /*<summary>
     * All the values we use on the dropdowns in order to easily change between all athe wind intruments
     * </summary>*/
    readonly List<string> dafnisValuesBore = new List<string> { "0,268","0,258","0,274","0,278"};
    readonly List<string> megaraH64ValuesBore = new List<string> { "0,478","0,468","0,488","0,484"};
    readonly List<string> megaraL64ValuesBore = new List<string> { "0,5099","0,5159","0,5199","0,4999"};
    readonly List<string> dafnisValueMouth = new List<string> { "0,07","0,05","0,09","0,071" };
    readonly List<string> megaraH64ValueMouth = new List<string> { "0,07","0,071","0,458","0,484"};
    readonly List<string> megaraH65ValuesBore = new List<string> { "0,46995","0,47995","0,45995","0,47595" };
    readonly List<string> megaraH65ValueMouth = new List<string> { "0,07", "0,44995", "0,48995", "0,071"};

    [HideInInspector]public bool isBack;
    #endregion

    #region UnityMethods
    // Start is called before the first frame update
    void Awake()
    {
        pnlChangeValuesScreen.SetActive(false);
        isBack = false;
    }

    private void Start()
    {
        CloseListOfGameObjects();

        info = gameObject.AddComponent<InfoCSV>();
        Load(mainfile);
        txtBoreValueParent.SetActive(false);
        txtMouthpieceValueParent.SetActive(false);
        txtChoiceValueParent.SetActive(false);
        SubscribeToButtons();
    }
    #endregion

    #region Methods
    void SubscribeToButtons()
    {
        
        btnHolesChange.onClick.AddListener(ActivateTogglesAndInputs);
        btnLightTone.onClick.AddListener(ActivateLightTonePanel);
        btnBarrytone.onClick.AddListener(ActivateBarrytonePanel);
        
        pnlChoose.onClick.AddListener(mpm.WarningOnFirstChoice);
       
        btnMouthpieceChange.onClick.AddListener(ActivateMouthPieceDropDown);
        btnBoreChange.onClick.AddListener(ActivateBoreDropDown);

       
    }

    #region CSV
    /*<summary>
     * This region is dedicated to get all the info from the csv we have our values saved there. Each row is "named" after the names we used also on our csv file.
     * And each row is dedicated to specific values. Here we load the first time the csv file.
     * </summary>*/
    public void Load(TextAsset csv)
    {
        rowList.Clear();
        //all line elements
        grid = CsvParser2.Parse(csv.text);

        for (int i = 1; i < grid.Length; i++)
        {

            row.mouthpiece = grid[i][0];
            row.boreLength = grid[i][1];
            row.boreDiameter = grid[i][2];
            row.boreWeight = grid[i][3];
            row.numberOfHoles = grid[i][4];
            row.distanceHole = grid[i][5];
            row.diameterHole = grid[i][6];
            row.hole = grid[i][7];
            row.folder = grid[i][8];
            row.soundPiece = grid[i][9];

            rowList.Add(row);

        }

        info.isLoaded = true;
    }

    //here we check how to configure each specific group values and how to load them on each occation. Is used as many times as we have combinations that provide us with sounds.
    void OpenValues(int num)
    {
        for (int i = num; i < grid.Length; i++)
        {
            row.mouthpiece = grid[i][0];
            row.boreLength = grid[i][1];
            row.boreDiameter = grid[i][2];
            row.boreWeight = grid[i][3];
            row.numberOfHoles = grid[i][4];
            row.distanceHole = grid[i][5];
            row.diameterHole = grid[i][6];
            row.hole = grid[i][7];
            row.folder = grid[i][8];
            row.soundPiece = grid[i][9];

            rowList.Add(row);
            
            if (grid[i][0] == grid[num][0] && grid[i][1] == grid[num][1] && grid[i][5] != grid[num][5])
            {

                if (row.folder != string.Empty)
                {
                    dropdownBore.itemText.text = row.boreLength;
                    dropdownMouthpiece.itemText.text = row.mouthpiece;
                    inputValueBoreWeigth.text = row.boreWeight;
                    inputValueBoreDiameter.text = row.boreDiameter;
                    inputValueNumOfHoles.text = row.numberOfHoles;
                   
                    inputValueDiameterHole[i - (num + 1)].text = row.diameterHole;

                    inputValueDistanceHole[i - (num + 1)].text = row.distanceHole;

                 
                    mpm.myAudio.LoadAudio(row.folder + "/");
                }
            }
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
            {
                if (i == num + 6)
                {
                    return;
                }
            }
            if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 || mpm.isMegarwnH65 || mpm.isMegarwnL65)
            {
                if (i == num + 8)
                {
                    return;
                }
            }
            if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
            {
                if (i == num + 7)
                {
                    return;
                }
            }
        }
        

    }
    #endregion

    #region Panels
    /*<summary>
     * This region is to open and close the panel when we want to change a value on a specific group.
     * </summary>*/

    //activate the panel to select which go to change the value
    public void OpenChangeValuesPanel()
    {
        
        txtBoreValueParent.gameObject.SetActive(false);
        txtMouthpieceValueParent.gameObject.SetActive(false);
        LoadDropDowns();
        pnlChangeValuesScreen.SetActive(true);
        mpm.pnlBottom.SetActive(false);
        //OptionValue();
        foreach (Toggle toggle in togglesChangedInput) toggle.isOn = false;
        CloseListOfGameObjects();
        btnHolesChange.gameObject.SetActive(true);
        for (int k = 0; k < inputValueDistanceHole.Length; k++)
        {
            inputValueDistanceHole[k].gameObject.SetActive(true);

        }

        foreach (GameObject goSounds in btnSounds) goSounds.SetActive(false);
        if (mpm.hasSelected) pnlChooseType.SetActive(false);
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();
    }

    #endregion

    #region GameObjects
    /*<summary>
     * For the game objects we use in order to either change values or hide/unhide objects.
     * </summary>*/


    //on load scene all extra go's are inactive and will be open after their respective selection
    private void CloseListOfGameObjects()
    {
        for (int i = 0; i < togglesChangedInput.Length; i++)
        {
            togglesChangedInput[i].gameObject.SetActive(false);
        }

        for (int j = 0; j < distanceInputs.Length; j++)
        {
            distanceInputs[j].gameObject.SetActive(false);
        }
        

        dropdownBore.interactable = false;
        dropdownMouthpiece.interactable = false;


    }
    //activate the toggles and extra inputs when loading specific values
    private void ActivateTogglesAndInputs()
    {
        mpm.ButtonsWhenChangeValues();
        
        if (pnlChangeValuesScreen.activeSelf)
        {
            pnlChangeValuesScreen.SetActive(false);
        }
        for (int i = 0; i < togglesChangedInput.Length; i++)
        {
            togglesChangedInput[i].gameObject.SetActive(true);
        }

        for (int j = 0; j < distanceInputs.Length; j++)
        {
            distanceInputs[j].gameObject.SetActive(true);

        }
        ShowHoleExtraValues();

        if (mpm.isDafnis || mpm.isMegarwnL64 || mpm.isMegarwnH64 || mpm.isMegarwnL65 || mpm.isMegarwnH65)
        {
            dropdownBore.interactable = false;
        }
        if (mpm.isDafnis || mpm.isMegarwnL64 || mpm.isMegarwnH64 || mpm.isMegarwnL65 || mpm.isMegarwnH65)
        {
            dropdownMouthpiece.interactable = false;
        }
        mpm.pnlBottom.SetActive(true);
        foreach (Toggle toggle in togglesChangedInput)
        {
            if (!toggle.isOn)
            {
                Debug.Log("is not on");
                mpm.btnSubmit.gameObject.SetActive(false);
            }
        }
        mpm.btnChangeValue.gameObject.SetActive(false);

        //we should activate the csv file that has all the hole values and will appear only when bore and moutpiece are on default values

        Debug.Log("Holes options open ");
    }

    //load 3d instrument
    public void Load3DIns(GameObject ins)
    {
        ins.SetActive(true);
        mpm.prefab3Dins.SetActive(true);
        mpm.pnlMainWindInstrument.SetActive(false);
        mpm.pnlBottom.SetActive(false);
        mpm.imgMnesiasBack.gameObject.SetActive(false);
        mpm.btnPrevious.gameObject.SetActive(true);
        mpm.btnBackToMain.gameObject.SetActive(false);
        if (mpm.pnlHelp.activeSelf) mpm.CloseHelpPanel();
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();
    }

    //load dropdown values for each instrument, on general dropdowns go's
    void LoadDropDowns()
    {
        if (mpm.isDafnis && !mpm.isMegarwnL64 && !mpm.isMegarwnH64 && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            dropdownBore.ClearOptions();
            dropdownBore.AddOptions(dafnisValuesBore);
            dropdownMouthpiece.ClearOptions();
            dropdownMouthpiece.AddOptions(dafnisValueMouth);
            OpenValues(85);
        }
        else if (mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isDafnis && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            dropdownBore.ClearOptions();
            dropdownBore.AddOptions(megaraH64ValuesBore);
            dropdownMouthpiece.ClearOptions();
            dropdownMouthpiece.AddOptions(megaraH64ValueMouth);
            OpenValues(1089);
        }
        else if (mpm.isMegarwnL64 && !mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            dropdownBore.ClearOptions();
            dropdownBore.AddOptions(megaraL64ValuesBore);
            dropdownMouthpiece.ClearOptions();
            dropdownMouthpiece.AddOptions(dafnisValueMouth);
            OpenValues(477);
        }
        else if (mpm.isMegarwnH65 && !mpm.isMegarwnL65 && !mpm.isMegarwnL64 && !mpm.isDafnis && !mpm.isMegarwnH64)
        {
            dropdownBore.ClearOptions();
            dropdownBore.AddOptions(megaraH65ValuesBore);
            dropdownMouthpiece.ClearOptions();
            dropdownMouthpiece.AddOptions(megaraH65ValueMouth);
            OpenValues(1665);
        }
        else if (mpm.isMegarwnL65 && !mpm.isMegarwnL64 && !mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnH65)
        {
            dropdownBore.ClearOptions();
            dropdownBore.AddOptions(megaraH64ValuesBore);
            dropdownMouthpiece.ClearOptions();
            dropdownMouthpiece.AddOptions(dafnisValueMouth);
            OpenValues(2277);
        }
        
    }


    #endregion

    #region Values

    /*<summary>
     * On this region is to load all the values of our game objects and the combinations with each group we have
     * </summary>*/


    //here load the H default values instrument
    void ActivateBarrytonePanel()
    {
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();
        mpm.pnlBottom.SetActive(true);
        pnlChooseType.SetActive(false);
        txtChoiceValueParent.SetActive(true);
        txtChoiceValue.text = "Βαρύς";
        mpm.isMegarwnH64 = false;
        mpm.isMegarwnH65 = false;
        mpm.imgContainerWindIstrument.gameObject.SetActive(true);

        if (mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH64 && !mpm.isMegarwnH65 && !mpm.isDafnis)
        {
            OpenValues(477);
            for (int i = 0; i < inputValueDiameterHole.Length; i++)
            {
                inputValueDiameterHole[i].gameObject.SetActive(true);
                inputValueDiameterHole[i].transform.parent.gameObject.SetActive(true);
            }
            for (int j = 0; j < inputValueDistanceHole.Length; j++)
            {
                inputValueDistanceHole[j].gameObject.SetActive(true);
            }
            mpm.imgDafnis.SetActive(false);
            mpm.imgMegarwnH.SetActive(false);
            mpm.imgMegarwnL.SetActive(true);
            mpm.imgHoleExplain.SetActive(true);
            LoadDropDowns();

            mpm.imgContainerWindIstrument.GetComponent<Image>().sprite = mpm.imgWindMegarwn64L;

            mpm.btn3DScene.gameObject.SetActive(true);
            OpenClose3DIns(false, false, true, false, false);
            mpm.btn3DScene.onClick.AddListener(() => Load3DIns(mpm.windInsMeg64L3D));

        }
        else if (mpm.isMegarwnL65 && !mpm.isMegarwnL64 && !mpm.isMegarwnH64 && !mpm.isMegarwnH65 && !mpm.isDafnis)
        {
            OpenValues(2277);
            for (int i = 0; i < inputValueDiameterHole.Length; i++)
            {
                inputValueDiameterHole[i].gameObject.SetActive(true);
                inputValueDiameterHole[i].transform.parent.gameObject.SetActive(true);

            }
            for (int j = 0; j < inputValueDistanceHole.Length; j++)
            {
                inputValueDistanceHole[j].gameObject.SetActive(true);
            }
            mpm.imgDafnis.SetActive(false);
            mpm.imgMegarwnH.SetActive(false);
            mpm.imgMegarwnL.SetActive(true);
            mpm.imgHoleExplain.SetActive(true);
            LoadDropDowns();

            mpm.imgContainerWindIstrument.GetComponent<Image>().sprite = mpm.imgWindMegarwn65L;

            mpm.btn3DScene.gameObject.SetActive(true);
            OpenClose3DIns(false, false, false, false, true);
            mpm.btn3DScene.onClick.AddListener(() => Load3DIns(mpm.windInsMeg65L3D));
        }
    }


    //here load the L default values instrument
    void ActivateLightTonePanel()
    {
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();
        mpm.pnlBottom.SetActive(true);
        pnlChooseType.SetActive(false);
        txtChoiceValueParent.SetActive(true);
        txtChoiceValue.text = "Οξύς";
        mpm.isMegarwnL64 = false;
        mpm.isMegarwnL65 = false;
        mpm.imgContainerWindIstrument.gameObject.SetActive(true);


        if (mpm.isDafnis && !mpm.isMegarwnL64 && !mpm.isMegarwnH64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
        {
            OpenValues(85);
            for (int i = 0; i < inputValueDiameterHole.Length; i++)
            {
                inputValueDiameterHole[inputValueDiameterHole.Length - 2].gameObject.SetActive(false);
                inputValueDiameterHole[inputValueDiameterHole.Length - 2].transform.parent.gameObject.SetActive(false);
                inputValueDiameterHole[inputValueDiameterHole.Length - 1].transform.parent.gameObject.SetActive(false);

            }
            for (int j = 0; j < inputValueDistanceHole.Length; j++)
            {
                inputValueDistanceHole[inputValueDistanceHole.Length - 2].gameObject.SetActive(false);
            }

            mpm.imgDafnis.SetActive(true);
            mpm.imgMegarwnH.SetActive(false);
            mpm.imgMegarwnL.SetActive(false);
            mpm.imgHoleExplain.SetActive(true);
            LoadDropDowns();

            mpm.imgContainerWindIstrument.GetComponent<Image>().sprite = mpm.imgWindDafnis;

            mpm.btn3DScene.gameObject.SetActive(true);
            OpenClose3DIns(true, false, false, false, false);
            mpm.btn3DScene.onClick.AddListener(() => Load3DIns(mpm.windInsDafnis3D));

        }
        else if (mpm.isMegarwnH64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65 && !mpm.isDafnis && !mpm.isMegarwnL64)
        {
            OpenValues(1089);
            for (int i = 0; i < inputValueDiameterHole.Length; i++)
            {
                inputValueDiameterHole[inputValueDiameterHole.Length - 1].gameObject.SetActive(false);
                inputValueDiameterHole[inputValueDiameterHole.Length - 1].transform.parent.gameObject.SetActive(false);

            }
            for (int j = 0; j < inputValueDistanceHole.Length; j++)
            {
                inputValueDistanceHole[inputValueDistanceHole.Length - 1].gameObject.SetActive(false);
            }
            mpm.imgDafnis.SetActive(false);
            mpm.imgMegarwnH.SetActive(true);
            mpm.imgMegarwnL.SetActive(false);
            mpm.imgHoleExplain.SetActive(true);
            LoadDropDowns();

            mpm.imgContainerWindIstrument.GetComponent<Image>().sprite = mpm.imgWindMegarwn64H;

            mpm.btn3DScene.gameObject.SetActive(true);
            OpenClose3DIns(false, true, false, false, false);
            mpm.btn3DScene.onClick.AddListener(() => Load3DIns(mpm.windInsMeg64H3D));

        }
        else if (mpm.isMegarwnH65 && !mpm.isMegarwnH64 && !mpm.isMegarwnL65 && !mpm.isDafnis && !mpm.isMegarwnL64)
        {
            OpenValues(1701);
            for (int i = 0; i < inputValueDiameterHole.Length; i++)
            {
                inputValueDiameterHole[i].gameObject.SetActive(true);
                inputValueDiameterHole[i].transform.parent.gameObject.SetActive(true);
            }
            for (int j = 0; j < inputValueDistanceHole.Length; j++)
            {
                inputValueDistanceHole[j].gameObject.SetActive(true);
            }
            mpm.imgDafnis.SetActive(false);
            mpm.imgMegarwnH.SetActive(false);
            mpm.imgMegarwnL.SetActive(true);
            mpm.imgHoleExplain.SetActive(true);
            LoadDropDowns();

            mpm.imgContainerWindIstrument.GetComponent<Image>().sprite = mpm.imgWindMegarwn65H;

            mpm.btn3DScene.gameObject.SetActive(true);
            OpenClose3DIns(false, false, false, true, false);
            mpm.btn3DScene.onClick.AddListener(() => Load3DIns(mpm.windInsMeg65H3D));
        }
        else
        {
            mpm.btn3DScene.gameObject.SetActive(false);
        }
        
    }

    //activate the dropdown for boreDropdown to select values
    void ActivateBoreDropDown()
    {
        mpm.ButtonsWhenChangeValues();
        txtBoreValueParent.gameObject.SetActive(true);
        txtBoreValue.text = "[Καταχωρήστε τιμή]";
        pnlChangeValuesScreen.SetActive(false);
        
        dropdownBore.interactable = true;
        dropdownMouthpiece.interactable = false;
        mpm.pnlBottom.SetActive(true);
        mpm.btnChangeValue.gameObject.SetActive(false);
        
    }

    //activate the dropdown for mouthpieceDropdown to select values
    void ActivateMouthPieceDropDown()
    {
        mpm.ButtonsWhenChangeValues();
        txtMouthpieceValueParent.gameObject.SetActive(true);
        txtMouthpieceValue.text = "[Καταχωρήστε τιμή]";
        pnlChangeValuesScreen.SetActive(false);

        dropdownMouthpiece.interactable = true;
        dropdownBore.interactable = false;

        mpm.pnlBottom.SetActive(true);
        mpm.btnChangeValue.gameObject.SetActive(false);
        
    }

    //activate and deactivate buttons when changing or not the values (slide B.3). Also onSubmit button to check if a pair is correct and we have audio in return or not.
    public void OnSubmit()
    {
        Debug.Log("Wind submit");
        if (OptionValue())
        {
            mpm.ButtonsWhenSubmitOrReset();
            /*here we will also load the respective wav files when user presses
             *the submit button with the values they have selected*/
            if(mpm.isDafnis || mpm.isMegarwnH64 || mpm.isMegarwnL64 || mpm.isMegarwnH65 || mpm.isMegarwnL65)
            {
                if (dropdownMouthpiece.isActiveAndEnabled)
                {
                    dropdownMouthpiece.interactable = false;
                }
                if (dropdownBore.isActiveAndEnabled)
                {
                    dropdownBore.interactable = false;
                }
                Debug.Log("Bore: "+dropdownBore.value+" mouth: "+dropdownMouthpiece.value);
            }
            
            InputValue();
            for (int i = 0; i < togglesChangedInput.Length; i++)
            {
                togglesChangedInput[i].gameObject.SetActive(false);
            }
            
            //OptionValue();
            Debug.Log("Submit press");
        }
        
        if (togglesChangedInput[0].isOn)
        {
            if (mpm.isDafnis)
            {
                row.folder = grid[121][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnH64)
            {
                row.folder = grid[1322][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnL64)
            {
                row.folder = grid[738][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnH65)
            {
                row.folder = grid[1926][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnL65)
            {
                row.folder = grid[2538][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            for (int j = 0; j < distanceInputs.Length - 6; j++)
            {
                distanceInputs[j].gameObject.SetActive(false);
                distanceInputs[j + 6].gameObject.SetActive(false);
            }

            Debug.Log("here");
            mpm.btn3DScene.gameObject.SetActive(false);
        }
        else if (togglesChangedInput[1].isOn)
        {
            if (mpm.isDafnis)
            {
                row.folder = grid[128][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnH64)
            {
                row.folder = grid[1330][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnL64)
            {
                row.folder = grid[747][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnH65)
            {
                row.folder = grid[1935][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnL65)
            {
                row.folder = grid[2547][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }

            for (int k = 0; k < inputValueDistanceHole.Length; k++)
            {
                inputValueDistanceHole[k].gameObject.SetActive(false);
            }
            for (int i = 0; i < distanceInputs.Length - 8; i++)
            {
                distanceInputs[i + 8].gameObject.SetActive(false);
            }
            Debug.Log("here second");
            mpm.btn3DScene.gameObject.SetActive(false);
        }
        else if (togglesChangedInput[2].isOn)
        {
            if (mpm.isDafnis)
            {
                row.folder = grid[135][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnH64)
            {
                row.folder = grid[1338][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnL64)
            {
                row.folder = grid[756][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnH65)
            {
                row.folder = grid[1944][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            else if (mpm.isMegarwnL65)
            {
                row.folder = grid[2556][8];
                mpm.myAudio.LoadAudio(row.folder + "/");
            }
            
            for (int k = 0; k < inputValueDistanceHole.Length; k++)
            {
                inputValueDistanceHole[k].gameObject.SetActive(false);
            }
            for (int i = 0; i < distanceInputs.Length - 8; i++)
            {
                distanceInputs[i].gameObject.SetActive(false);
            }
            Debug.Log("here third");
            mpm.btn3DScene.gameObject.SetActive(false);
        }

        RemoveEffect();

        // }
        mpm.btnChangeValue.gameObject.SetActive(true);
        foreach (GameObject goSounds in btnSounds) goSounds.SetActive(true);
    }

    //check the dropdowns and which pairs can have an audio for all wind instruments
    public bool OptionValue()
    {
        int mouthPieceValue = dropdownMouthpiece.value;
        int boreValue = dropdownBore.value;
        if (mouthPieceValue == 0 && boreValue == 0)
        {
            Debug.Log("Option Value + default values");
            mpm.btn3DScene.gameObject.SetActive(true);
            ShowHoleExtraValues();
            btnHolesChange.interactable = true;

            return true;
        }
        else if (mouthPieceValue == 0 && boreValue == 1)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(57);
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1121);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(585);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                /*OpenValues(2309);*/
                OpenValues(2349);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
                OpenValues(1738);
            Debug.Log("No default values");


            CloseListOfGameObjects();

            return true;
        }
        else if (mouthPieceValue == 0 && boreValue == 2)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1);
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1153);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(549);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                /*OpenValues(2341);*/
                OpenValues(2313);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
                OpenValues(1701);


            CloseListOfGameObjects();
            return true;
        }
        else if (mouthPieceValue == 0 && boreValue == 3)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(29);
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1185);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(513);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                /* OpenValues(2373);*/
                OpenValues(2385);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
                OpenValues(1845);


            CloseListOfGameObjects();
            return true;
        }
        else if (boreValue == 0 && mouthPieceValue == 1)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(365);
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1281);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(621);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                /*OpenValues(2405);*/
                OpenValues(2421);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
                OpenValues(1773);


            CloseListOfGameObjects();
            return true;
        }
        else if (boreValue == 0 && mouthPieceValue == 2)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(253);
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1217);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(657);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                /*OpenValues(2437)*/
                OpenValues(2457);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
                OpenValues(1809);

            CloseListOfGameObjects();
            return true;
        }
        else if (boreValue == 0 && mouthPieceValue == 3)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(141);
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(1249);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                OpenValues(693);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
                /*OpenValues(2469);*/
                OpenValues(2493);
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
                OpenValues(1881);
            CloseListOfGameObjects();
            return true;
        }
        else
        {
            return false;
        }


    }

    //show only values when extra holes selection is on.
    public void ShowHoleExtraValues()
    {
        
        for (int k = 0; k < grid.Length; k++)
        {
            if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
            {
                if (grid[k][0] == grid[121][0] && grid[k][1] == grid[121][1] && grid[k][5] != grid[121][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[121 + i][5];
                    }
                }
                if (grid[k][0] == grid[128][0] && grid[k][1] == grid[128][1] && grid[k][5] != grid[128][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 6; i++)
                    {
                        distanceInputs[i].text = grid[128 + i][5];
                    }
                }
                if (grid[k][0] == grid[133][0] && grid[k][1] == grid[133][1] && grid[k][5] != grid[133][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 6; i++)
                    {
                        distanceInputs[i + 6].text = grid[133 + i][5];
                    }
                }
                if(grid[k][0] == grid[85][0] && grid[k][1] == grid[85][1] && grid[k][5] != grid[85][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[86 + i][5];
                        inputValueDiameterHole[i].text = grid[86 + i][6];
                    }
                }
               
            }
            else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
            {
                if (grid[k][0] == grid[1322][0] && grid[k][1] == grid[1322][1] && grid[k][5] != grid[1322][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[1322 + i][5];
                    }
                }
                if (grid[k][0] == grid[1330][0] && grid[k][1] == grid[1330][1] && grid[k][5] != grid[1330][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 7; i++)
                    {
                        distanceInputs[i].text = grid[1330 + i][5];
                    }
                }
                if (grid[k][0] == grid[1337][0] && grid[k][1] == grid[1337][1] && grid[k][5] != grid[1337][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 7; i++)
                    {
                        distanceInputs[i + 7].text = grid[1337 + i][5];
                    }
                }
                if (grid[k][0] == grid[1089][0] && grid[k][1] == grid[1089][1] && grid[k][5] != grid[1089][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[1090 + i][5];
                        inputValueDiameterHole[i].text = grid[1090 + i][6];
                    }
                }
            }
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnL65 && !mpm.isMegarwnH65)
            {
                if (grid[k][0] == grid[739][0] && grid[k][1] == grid[739][1] && grid[k][5] != grid[739][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[739 + i][5];
                    }
                }
                if (grid[k][0] == grid[748][0] && grid[k][1] == grid[748][1] && grid[k][5] != grid[748][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 8; i++)
                    {
                        distanceInputs[i].text = grid[748 + i][5];
                    }
                }
                if (grid[k][0] == grid[757][0] && grid[k][1] == grid[757][1] && grid[k][5] != grid[757][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 8; i++)
                    {
                        distanceInputs[i + 8].text = grid[757 + i][5];
                    }
                }
                if (grid[k][0] == grid[477][0] && grid[k][1] == grid[477][1] && grid[k][5] != grid[477][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[478 + i][5];
                        inputValueDiameterHole[i].text = grid[478 + i][6];
                    }
                }

            }
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnL65 && !mpm.isMegarwnH65)
            {
                if (grid[k][0] == grid[2538][0] && grid[k][1] == grid[2538][1] && grid[k][5] != grid[2538][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[2539 + i][5];
                    }
                }
                if (grid[k][0] == grid[2547][0] && grid[k][1] == grid[2547][1] && grid[k][5] != grid[2547][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 8; i++)
                    {
                        distanceInputs[i].text = grid[2547 + i][5];
                    }
                }
                if (grid[k][0] == grid[2556][0] && grid[k][1] == grid[2556][1] && grid[k][5] != grid[2556][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 8; i++)
                    {
                        distanceInputs[i + 7].text = grid[2557 + i][5];
                    }
                }
                if (grid[k][0] == grid[2277][0] && grid[k][1] == grid[2277][1] && grid[k][5] != grid[2277][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[2278 + i][5];
                        inputValueDiameterHole[i].text = grid[2278 + i][6];
                    }
                }
            }
            else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnL65 && mpm.isMegarwnH65)
            {
                if (grid[k][0] == grid[1927][0] && grid[k][1] == grid[1927][1] && grid[k][5] != grid[1927][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[1927 + i][5];
                    }
                }
                if (grid[k][0] == grid[1936][0] && grid[k][1] == grid[1936][1] && grid[k][5] != grid[1936][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 8; i++)
                    {
                        distanceInputs[i].text = grid[1936 + i][5];
                    }
                }
                if (grid[k][0] == grid[1945][0] && grid[k][1] == grid[1945][1] && grid[k][5] != grid[1945][5])
                {
                    for (int i = 0; i < distanceInputs.Length - 8; i++)
                    {
                        distanceInputs[i + 8].text = grid[1945 + i][5];
                    }
                }
                if (grid[k][0] == grid[1665][0] && grid[k][1] == grid[1665][1] && grid[k][5] != grid[1665][5])
                {
                    for (int i = 0; i < inputValueDistanceHole.Length; i++)
                    {
                        inputValueDistanceHole[i].text = grid[1666 + i][5];
                        inputValueDiameterHole[i].text = grid[1666 + i][6];
                    }
                }
            }


        }
        //mpm.pnlBottom.SetActive(true);
    }

    //on extra hole change values, when a specific toggle is On the text on inputs change colors both to the selected one and the un-selected ones. Also audio is loaded too.
    public void HoleSelection()
    {
        if (mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            if (togglesChangedInput[0].isOn)
            {

                for (int i = 0; i < inputValueDistanceHole.Length; i++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);
                    }
                }
                for (int j = 0; j < distanceInputs.Length - 6; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < distanceInputs.Length - 6; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k + 6].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
            else if (togglesChangedInput[1].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < inputValueDistanceHole.Length; k++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }

            }
            else if (togglesChangedInput[2].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                }
                for (int j = 0; j < inputValueDistanceHole.Length; j++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
        }
        else if (!mpm.isDafnis && mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            if (togglesChangedInput[0].isOn)
            {

                for (int i = 0; i < inputValueDistanceHole.Length; i++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);
                    }
                }
                for (int j = 0; j < distanceInputs.Length - 7; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < distanceInputs.Length - 7; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k + 7].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
            else if (togglesChangedInput[1].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < inputValueDistanceHole.Length; k++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }

            }
            else if (togglesChangedInput[2].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                }
                for (int j = 0; j < inputValueDistanceHole.Length; j++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
        }
        else if (!mpm.isDafnis && !mpm.isMegarwnH64 && mpm.isMegarwnL64 && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            if (togglesChangedInput[0].isOn)
            {
                for (int i = 0; i < inputValueDistanceHole.Length; i++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);
                    }
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
            else if (togglesChangedInput[1].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }

                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < inputValueDistanceHole.Length; k++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }

            }
            else if (togglesChangedInput[2].isOn)
            {

                //Debug.Log("Is it On: " + togglesChangedInput[2].isOn);
                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                    Debug.Log("I value: " + i);
                }
                for (int j = 0; j < inputValueDistanceHole.Length; j++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
        }
        else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            if (togglesChangedInput[0].isOn)
            {

                for (int i = 0; i < inputValueDistanceHole.Length; i++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);
                    }
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
            else if (togglesChangedInput[1].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                    Debug.Log("I value: " + i);
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < inputValueDistanceHole.Length; k++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }

            }
            else if (togglesChangedInput[2].isOn)
            {

                //Debug.Log("Is it On: " + togglesChangedInput[2].isOn);
                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                    Debug.Log("I value: " + i);
                }
                for (int j = 0; j < inputValueDistanceHole.Length; j++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
        }
        else if (!mpm.isDafnis && !mpm.isMegarwnH64 && !mpm.isMegarwnL64 && !mpm.isMegarwnH65 && mpm.isMegarwnL65)
        {
            if (togglesChangedInput[0].isOn)
            {

                for (int i = 0; i < inputValueDistanceHole.Length; i++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);
                    }
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
            else if (togglesChangedInput[1].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                }
                for (int j = 0; j < distanceInputs.Length - 8; j++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[j + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }
                for (int k = 0; k < inputValueDistanceHole.Length; k++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }

            }
            else if (togglesChangedInput[2].isOn)
            {

                for (int i = 0; i < distanceInputs.Length - 8; i++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[i + 8].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(0, 0, 0, 255);

                    }
                }
                for (int j = 0; j < inputValueDistanceHole.Length; j++)
                {

                    foreach (TextMeshProUGUI myText in inputValueDistanceHole[j].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);
                    }
                }
                for (int k = 0; k < distanceInputs.Length - 8; k++)
                {
                    foreach (TextMeshProUGUI myText in distanceInputs[k].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        myText.color = new Color32(159, 159, 159, 255);

                    }
                }

            }
        }
        else
        {
            RemoveEffect();
        }
        foreach (Toggle toggle in togglesChangedInput)
        {
            if (toggle.isOn)
            {
                mpm.btnSubmit.gameObject.SetActive(true);
                Debug.Log("is on");
            }
        }
    }
    //when we submit or change option on toggles, we remove the text effect on the texts
    void RemoveEffect()
    {
        for (int i = 0; i < inputValueDistanceHole.Length; i++)
        {

            foreach (TextMeshProUGUI myText in inputValueDistanceHole[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                //myText.color = new Color(0, 0, 0, 255);
                myText.color = new Color32(159, 159, 159, 255);
            }
        }
        for (int i = 0; i < distanceInputs.Length - 6; i++)
        {
            foreach (TextMeshProUGUI myText in distanceInputs[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                myText.color = new Color32(159, 159, 159, 255);

            }
        }
        for (int i = 0; i < distanceInputs.Length - 6; i++)
        {
            foreach (TextMeshProUGUI myText in distanceInputs[i + 6].GetComponentsInChildren<TextMeshProUGUI>())
            {
                myText.color = new Color32(159, 159, 159, 255);

            }
        }

    }

    //when reset is selected default values are loaded on inputs etc.
    public void OnReset()
    {
        mpm.ButtonsWhenSubmitOrReset();
        mpm.btnChangeValue.gameObject.SetActive(true);
        CloseListOfGameObjects();
        foreach (Toggle toggle in togglesChangedInput) toggle.isOn = false;
        txtMouthpieceValue.text = "[Καταχωρήστε τιμή]";
        txtBoreValue.text = "[Καταχωρήστε τιμή]";

        dropdownBore.ClearOptions();
        dropdownMouthpiece.ClearOptions();

        txtChoiceValue.text = "[Καταχωρήστε τιμή]";
        txtBoreValueParent.SetActive(false);
        txtMouthpieceValueParent.SetActive(false);
        txtChoiceValueParent.SetActive(false);

        pnlChooseType.SetActive(true);
       

        foreach (TMP_InputField tMP_ in inputValueDiameterHole) { tMP_.gameObject.SetActive(true); tMP_.text = "Διάμετρος"; }
        foreach (TMP_InputField tMP_1 in inputValueDistanceHole) { tMP_1.gameObject.SetActive(true); tMP_1.text = "Απόσταση"; }
        mpm.imgDafnis.SetActive(false);
        mpm.imgMegarwnH.SetActive(false);
        mpm.imgMegarwnL.SetActive(false);
        mpm.imgHoleExplain.SetActive(false);

        mpm.imgContainerWindIstrument.gameObject.SetActive(false);

        //reset booleans here
        if (mpm.isPiraeus &&!mpm.isMegara)
        {
            mpm.isDafnis = true;
            mpm.isMegarwnH64 = false;
            mpm.isMegarwnL64 = false;
            mpm.isMegarwnL65 = false;
            mpm.isMegarwnH65 = false;
        }
        else if (!mpm.isPiraeus && mpm.isMegara && !mpm.isMegarwnH65 && !mpm.isMegarwnL65)
        {
            mpm.isDafnis = false;
            mpm.isMegarwnH64 = true;
            mpm.isMegarwnL64 = true;
            mpm.isMegarwnL65 = false;
            mpm.isMegarwnH65 = false;
            
        }
        else if (!mpm.isPiraeus && mpm.isMegara && !mpm.isMegarwnH64 && !mpm.isMegarwnL64)
        {
            mpm.isDafnis = false;
            mpm.isMegarwnH64 = false;
            mpm.isMegarwnL64 = false;
            mpm.isMegarwnL65 = true;
            mpm.isMegarwnH65 = true;
        }
        if (mpm.pnlExtraMenu.activeSelf) mpm.CloseMenuPanel();
    }
    //to change the text when selecting a value from the dropdown
    void InputValue()
    {
        Debug.Log("Input Value");
        txtBoreValue.text = dropdownBore.options[dropdownBore.value].text;
        txtMouthpieceValue.text = dropdownMouthpiece.options[dropdownMouthpiece.value].text;
    }

    void OpenClose3DIns(bool isDafnis, bool isMeg64H, bool isMeg64L, bool isMeg65H, bool isMeg65L)
    {
        mpm.windInsDafnis3D.SetActive(isDafnis);
        mpm.windInsMeg64H3D.SetActive(isMeg64H);
        mpm.windInsMeg64L3D.SetActive(isMeg64L);
        mpm.windInsMeg65H3D.SetActive(isMeg65H);
        mpm.windInsMeg65L3D.SetActive(isMeg65L);
    }
    #endregion

    #endregion
}
