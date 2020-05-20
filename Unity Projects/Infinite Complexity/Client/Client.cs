using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
// connect 127.0.0.1 6321 note
public class Client : MonoBehaviour {


    public GameObject terminalButtonPrefab, mapButtonPrefab, mapPiecePrefab, mapColumbPrefab, mapRowPrefab, factoryPrefab, OptionsManager, mainCamera, emptyImage;
    public LoginScript loginPage;
    public LandManager landManager;
    public string clientName, clientPassword;

    public int mapLength;
    public int mapWidth;

    [SerializeField]
    private Text status;
    [SerializeField]
    private Text credits;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string terminalLocation = "DESKTOP";
    private string targetUser;

    private int mathAnswer;
    private bool mathAnswered = true;
    private string mathQuestion;


    private InputField terminalInput; // the terminal Input
    private GameObject terminal;
    private GameObject toolbarButtons;
    [SerializeField]
    private String IP = "127.0.0.1";
    [SerializeField]
    private String PORT = "6321";
    private Text terminalText;
    private GameObject map;
    private GameObject place;
    [SerializeField]
    private Text landText, resourceText, ownerText;

    private bool gotMapFromServer;

    private string mouseOptionLocation = "Desktop";
    private bool optionsOn;
    private GameObject[][] wholeMap;
    //  mapPieceServer[,] wholeMap;
    private bool register;
    [SerializeField]
    private int heavyStrength, mediumStrength, lightStrength;
    [SerializeField]
    private Sprite[] frontlines;
    private mapPiece currentLand;





    public void Start()
    {
        terminal = GameObject.FindGameObjectWithTag("terminal");
        toolbarButtons = GameObject.FindGameObjectWithTag("toolbarButtons");
        terminalInput = terminal.GetComponentInChildren<InputField>(); // getting the correct thing
        terminalInput.ActivateInputField(); //activate when the game starts, later change to when terminal is opened/switched to/ un-minimized
        terminalText = terminal.GetComponentInChildren<ScrollRect>().GetComponentInChildren<Text>(); // gets terminal text

        map = GameObject.FindGameObjectWithTag("map");
        place = GameObject.FindGameObjectWithTag("mapPlace");
        closeMap();
        closeTerminal();
    }

    private void ConnectToServer(string[] command)
    {
        //     Debug.Log(command[1] + " : " + command[2] + " : " + command[3]);
        //If already connected, ignore this function
        if (socketReady)
            return;

        // Default host/ port values
        string host = IP;
        int port = Int32.Parse(PORT); ;
        try
        {
            host = command[1];
            port = Int32.Parse(command[2]); 
            clientName = command[3];
            clientPassword = command[4];
        }
        catch(Exception e)
        {
            Debug.Log("Incorrect number of arguments | "+e);
        }

  //      Debug.Log("We got this far - 2");


        // Create the Socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
            status.text = "Online";
            status.color = Color.green;
        }
        catch(Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
            OnIncomingData("Failed to connect because " +e.Message);

        }
   //     Debug.Log(socketReady);
    }

    private void Update()
    {
        if (socketReady)
        {
            try
            {
                if (stream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(stream, true);
                    string data;
      //              Debug.Log("Got here! - 1");

                    //     while ((data = reader.ReadLine()) != null)
                    //           {
                    data = reader.ReadLine();
        //            Debug.Log(data);
      /*              try
                    {
            //            string data2;
     //                   bool thing = (data2 = reader.ReadLine())==null;
    //                    Debug.Log(thing);
                    }catch(Exception e)
                    {
                        Debug.Log(e);
                    } */
                    
                    int selection = Int32.Parse(data.Substring(0, 3));
                    Debug.Log(selection);
         //           Debug.Log(data.Substring(3));
                    switch (selection)
                    {
                        case 0:
                            OnIncomingData(data.Substring(3));
                            break;
                        case 10:
                            MultiLineInput(data.Substring(3));
                            break;
                        case 1:
                            ChangeCredits(data.Substring(3));
                            break;
                        case 3:
                            BeginHack();
                            break;
                        case 4:
                            EnterComputer();
                            break;
                        case 6:
                            CreateMap(data.Substring(3));
                            gotMapFromServer = true;
                            break;
                        case 16:
                            UpdateMapPiece(data.Substring(3));
                            break;
                        case 99:
                   //         Debug.Log(clientName + " was changed to " + data.Substring(3));
                            clientName = data.Substring(3);
                            loginPage.Login();
                            UpdateCurrency(0);
                            break;
                        case 199:
                            loginPage.LoginError(1);
                            CloseSocket();
                            break;
                        case 299:
                            loginPage.LoginError(2);
                            CloseSocket();
                            break;
                        case 399:
                            loginPage.LoginError(3);
                            CloseSocket();
                            break;
                        case 999:
                            CloseSocket();
                            break;
                    }
             //       }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
         //   Debug.Log(mouseOptionLocation);
            CloseOptionMenus();
            if (mouseOptionLocation != null && !mouseOptionLocation.Equals("Untagged"))
            {
                OptionsManager.GetComponent<OptionsManager>().CreateOption(mouseOptionLocation);
                optionsOn = true;
            }
        }
        if (optionsOn && Input.GetMouseButtonDown(0))
        {
            Invoke("CloseOptionMenus", 0.2f);
        }
    }


    private void OnIncomingData(string data)
    {
        if (data == "%NAME")
        {
            if(register)
                Send("&&NAME|" + clientName + "|" + clientPassword);
            else
                Send("&NAME|" + clientName+"|"+clientPassword);
            register = false;
            return;
        }

        terminalText.text = data+"\n"+ terminalText.text;

    }

    public void setRegister(bool choice)
    {
        register = choice;
    }

    private void MultiLineInput(string data)
    {
        string[] inputs = data.Split('|');
        foreach(string input in inputs)
        {
            OnIncomingData(input);
        }
    }

    public void Login(string _user, string _pass)
    {
        clientName = _user;
        clientPassword = _pass;
        Debug.Log("Login with Username: "+ _user + " and Password: "+_pass);
        ConnectToServer(("THROWAWAY " + IP + " " + PORT + " " + clientName + " " + clientPassword).Split(' '));
    }

    public void startTerminal()
    {
        if (!terminal.activeSelf)
        {
            terminal.SetActive(true);
            terminalInput.ActivateInputField();
        }  
        else
            return; // stop early if terminal is already active
        Button[] allToolbarButtons = toolbarButtons.GetComponentsInChildren<Button>();
        bool hasButton = false;
   //     Debug.Log(allToolbarButtons.Length);
        foreach(Button element in allToolbarButtons)
        {
    //        Debug.Log(element.tag);
            if (element.tag.Equals("terminalButton"))
            {
    //            Debug.Log("Turned True");
                hasButton = true;
            }
   //         else
   //             Debug.Log(element.tag + "!= terminalButton");
        }
        if (!hasButton)
        {
            GameObject go = Instantiate(terminalButtonPrefab, toolbarButtons.transform) as GameObject;
        }
    }

    public void closeTerminal()
    {
        terminalInput.text = "";
        terminalText.text = "Type HELP for a list of commands.";
        terminal.SetActive(false);
        Button[] allToolbarButtons = toolbarButtons.GetComponentsInChildren<Button>();
        foreach (Button element in allToolbarButtons)
        {
            if (element.tag.Equals("terminalButton"))
            {
                Destroy(element.gameObject);
                return;
            }
        }
        terminalLocation = "DESKTOP";
    }

    public void minTerminalToggle()
    {
        
        terminal.SetActive(!terminal.activeSelf);
        if(terminal.activeSelf)
            terminalInput.ActivateInputField();
    }

    public void startMap()
    {

        if (!map.activeSelf)
        {
            map.SetActive(true);
        }
        else
            return; // stop early if map is already active
        // get map from server ect.
        Send("006");

        Button[] allToolbarButtons = toolbarButtons.GetComponentsInChildren<Button>();
        bool hasButton = false;
 //       Debug.Log(allToolbarButtons.Length);
        foreach (Button element in allToolbarButtons)
        {
  //          Debug.Log(element.tag);
            if (element.tag.Equals("mapButton"))
            {
  //              Debug.Log("Turned True");
                hasButton = true;
            }
  //          else
  //              Debug.Log(element.tag + "!= maplButton");
        }
        if (!hasButton)
        {
            GameObject go = Instantiate(mapButtonPrefab, toolbarButtons.transform) as GameObject;
        }
    }

    private void CreateMap(string data)
    {
   //     Debug.Log(data);
        string[] realData = data.Split(':');
  //      Debug.Log(realData.Length);
        string[] mapPart = new string[4];
  //      Debug.Log(mapPart.Length);
        GameObject go = null;
        GameObject mapPlace = null;
        wholeMap = new GameObject[mapWidth][];
        for (int i = 0; i < wholeMap.Length; i++)
        {
            wholeMap[i] = new GameObject[mapLength];
        }
        /* // CODE FOR MAKING A MAPPIECESERVER MATRIX
        string[] _IDs = new string[2];
        wholeMap = new mapPieceServer[mapWidth, mapLength];
        foreach (string item in realData)
        {
            if (item.Split(' ').Length == 4)
            {
                _IDs = item.Split(' ')[3].Split('-');
                wholeMap[Int32.Parse(_IDs[0]), Int32.Parse(_IDs[1])] = new mapPieceServer(item.Split(' '));
            }
        }
        */
        for (int i = 0; i < mapWidth; i++)
        {
            mapPlace = Instantiate(mapColumbPrefab, place.transform) as GameObject;
            for (int j = 0; j < mapLength; j++)
            {
                mapPart = realData[(i* mapWidth) +j].Split(' ');
             /*   foreach (string element in mapPart)
                {
                    Debug.Log(element);
                }*/
                go = Instantiate(mapPiecePrefab, mapPlace.transform) as GameObject;
                wholeMap[i][j] = go;
                go.GetComponent<mapPiece>().create(mapPart);
                go.GetComponent<doubleClick>().setClient(this);
                go.name = "MapPiece-" + go.GetComponent<mapPiece>().getID();
                //             Debug.Log(mapPart[0]+ " Number we are on: "+ ((i * mapLength) + j));
                UpdateMapColor(mapPart[0],go);
                CreateMapPartExtras(go);
            }
           
        }
        place.transform.localPosition = new Vector3(1500,-1500,0);
        gotMapFromServer = false;

    }

    private void CreateMapPartExtras(GameObject go)
    {
    //    Debug.Log("Made map part extras");
        mapPiece mapPart = go.GetComponent<mapPiece>();
        if (mapPart.getResourceCount() > 0)
        {
            GameObject factory = Instantiate(factoryPrefab, go.transform) as GameObject;
        }
        // Create algorithm to find out which factory piece to use.
        bool hasRight = mapPart.rightStrength() > 0;
        bool hasTop = mapPart.topStrength() > 0;
        bool hasLeft = mapPart.leftStrength() > 0;
        bool hasBottom = mapPart.bottomStrength() > 0;

        if (!(hasRight || hasTop || hasLeft || hasBottom))
            return;

        int frontNum = 0;
        int rot = 0;
        
        if (hasRight)
            frontNum++;
        if (hasTop)
            frontNum++;
        if (hasLeft)
            frontNum++;
        if (hasBottom)
            frontNum++;
        if (frontNum == 1)
            frontNum = 0;
        if (frontNum == 2 && ((hasRight && hasLeft) || (hasTop && hasBottom)))
            frontNum--;
        if (frontNum == 1 && (hasTop && hasBottom))
            rot = 1;
        if (frontNum == 2)
        {
            if (!hasTop)
            {
                rot += 2;
                if (hasRight)
                    rot++;
            }
            else
            {
                if (!hasRight)
                    rot++;
            }
        }
        if (frontNum == 3)
        {
            if (!hasRight)
                rot++;
            if (!hasTop)
                rot += 2;
            if (hasLeft)
                rot += 3;
        }
        if (!clientName.Equals(mapPart.getOwner())) // is Enemy
            frontNum += 5;

        SetImage(go, frontlines[frontNum], rot);
    }

    private void SetImage(GameObject go, Sprite sp, int rot)
    {
        GameObject frontline = Instantiate(emptyImage, go.transform) as GameObject;
        frontline.GetComponent<Image>().sprite = sp;
        frontline.transform.Rotate(new Vector3(0,0,90*rot));
    }

    public void closeMap()
    {
        Transform tempTrans = place.GetComponentInParent<Transform>().parent; // Get parent
    //    Debug.Log(tempTrans.gameObject.name);
        Destroy(place); // Destroy Object to quickly remove all map Pieces
        place = Instantiate(mapRowPrefab, tempTrans) as GameObject; //remake mapPlace
   //     tempTrans.parent.parent.gameObject.GetComponent<ScrollRect>().content = place.GetComponent<RectTransform>(); // make scroll still work
        map.SetActive(false);

        Button[] allToolbarButtons = toolbarButtons.GetComponentsInChildren<Button>();
        foreach (Button element in allToolbarButtons)
        {
       //     Debug.Log(element.tag);
            if (element.tag.Equals("mapButton"))
            {
                Destroy(element.gameObject);
                return;
            }
        }
        
    }

    public void setMapInfo(string data) // sets all the labels
    {
        string[] realData = data.Split(' ');
        landText.text = realData[0];
        resourceText.text = realData[1];
        ownerText.text = realData[2];
    }

    public void UpdateServerMapPiece(string data)
    {
        Send("016" + data);
    }
    public void UpdateMapPiece(string data)
    {
        //    Debug.Log(data);
        try
        {
            setMapInfo(data);
            string[] realData = data.Split(' ');
            GameObject go = GameObject.Find("MapPiece-" + realData[3]);
            go.GetComponent<mapPiece>().create(realData);
            UpdateMapColor(realData[0], go);
            CreateMapPartExtras(go);
        }
        catch (Exception e) { Debug.Log(e); }
        
    }

    public void UpdateMapColor(string type, GameObject go)
    {
        switch (type)
        {
            case "Grassland":
                go.GetComponent<Image>().color = new Color(8f / 255, 141f / 255, 17f / 255); // Change these colors later to be more exact
                break;
            case "Water":
                go.GetComponent<Image>().color = new Color(15.0f / 255, 200f / 255, 197f / 255);
                break;
            case "Wasteland":
                go.GetComponent<Image>().color = new Color(173f / 255, 114f / 255, 46f / 255);
                break;
            case "Mountain":
                go.GetComponent<Image>().color = new Color(99f / 255, 87f / 255, 81f / 255);
                break;

        }
    }

    public void minBasicToggle(String targetName)
    {
        GameObject target = null;
        switch (targetName)
        {
            case "map": target = map;
                break;
        }
        target.SetActive(!target.activeSelf);
    }

    public void CreateFactory(Factory fact, mapPiece land)
    {
        land.AddFactory(fact, clientName);
        UpdateServerMapPiece(land.DataInfo());
    }
    
    public void CreateFactory()
    {
        try
        {
            Debug.Log("Creating factory");
            Debug.Log(currentLand.getID());
            currentLand.AddFactory(new Factory(), clientName);
            UpdateServerMapPiece(currentLand.DataInfo());
        }
        catch (Exception e) { Debug.Log("Can't create factory \n" + e.StackTrace); }
    }

    public void SetCurrentMapPiece(mapPiece land)
    {
        currentLand = land;
    }
    
    public void ViewFrontlines()
    {
        // test for map not existing, or currentland not existing
        landManager.UpdateImages(wholeMap, currentLand);
    }

    public void SetMouseOptionLocation(string loc)
    {
        mouseOptionLocation = loc; // When mouse enters a right-clickable area, store which area that is.
    }
    public string GetMouseOptionLocation()
    {
        return mouseOptionLocation; // gives the option to get this private variable
    }
    private void CloseOptionMenus()
    {
        GameObject[] options = GameObject.FindGameObjectsWithTag("OptionMenu");
        foreach(GameObject option in options)
        {
            option.SetActive(false);
        }
    }

    private void ChangeCredits(string data)
    {
        credits.text = string.Format("{0:n0}", Int32.Parse(data));
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        status.text = "Offline";
        status.color = Color.red;
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    public void DecideCommand()
    {
        if (!Input.GetKeyDown("return"))
            return;
        

        Debug.Log("Decide Command");
        string[] command = terminalInput.text.Split();
        command[0] = command[0].ToUpper();
        if (command[0].Equals("CLEAR")) // COmmands to clear, in the wrong spot ofc. all these commands will be changed.
        {
            terminalInput.text = "";
            terminalInput.ActivateInputField();
            terminalText.text = "";
            return;
        }

        switch (terminalLocation)
        {
            case "DESKTOP":
                switch (command[0])
                {
                    case "CONNECT":
                        ConnectToServer(command); Debug.Log("connected!");
                        break;
                    case "DISCONNECT":
                        CloseSocket(); ; Debug.Log("disconnected!");
                        break;
                    case "HELP":
                        OnIncomingData(" ");
                        OnIncomingData("List of Commands:");
                        OnIncomingData("Connect IP PORT NAME");
                        OnIncomingData("Disconnect");
                        OnIncomingData("Run (MATH, EMAIL)");
                        OnIncomingData("Hack USERNAME");
                        break;
                    case "RUN":
                        if (command.Length < 2)
                        {
                            OnIncomingData(" ");
                            OnIncomingData("List of RUN Commands:");
                            OnIncomingData("MATH");
                            OnIncomingData("EMAIL");
                            OnIncomingData("Ex. run email");
                        }
                        else
                        {
                            command[1] = command[1].ToUpper();
                            switch (command[1])
                            {
                                case "MATH":
                                    RunMath();
                                    break;
                                case "EMAIL":
                                    RunEmail();
                                    break;
                            }
                        }
                        break;
                    case "HACK":
                        if (command.Length < 2)
                        {
                            OnIncomingData(" ");
                            OnIncomingData("To hack another user, find their username, then use the hack command.");
                            OnIncomingData("Ex. hack Test_Server");
                        }
                        else
                        {
                            Send("003" + command[1]);
                            targetUser = command[1];
                        }
                        break;
                    default:
                        OnIncomingData("That is not a valid command.");
                        break;
                }
                break;
            case "MATHSESSION":
                switch (command[0])
                {
                    case "HELP":
                        OnIncomingData(" ");
                        OnIncomingData("List of Commands:");
                        OnIncomingData("Answer ANSWER   (Ex. Answer 7)");
                        OnIncomingData("Question   (Get the question repeated)");
                        OnIncomingData("Close   (Close the Math program)");
                        break;
                    case "QUESTION":
                        OnIncomingData(mathQuestion);
                        break;
                    case "ANSWER":
                        if (command.Length < 2)
                            OnIncomingData("You need to input an answer. Ex. Answer 12");
                        else {
                            int answer = 0;
                            if (Int32.TryParse(command[1],out answer)){
                                if(answer == mathAnswer)
                                {
                                    OnIncomingData("Correct! You earned {c}10");
                                    UpdateCurrency(10);
                                    NewMathQuestion();
                                }
                                else
                                {
                                    OnIncomingData("Incorrect. You lost {c}5");
                                    UpdateCurrency(-5);
                                    NewMathQuestion();
                                }
                            }
                            else
                            {
                                OnIncomingData("Make sure your answer is a whole number. Ex. Answer 134");
                            }
                        }
                        break;
                    case "CLOSE":
                        OnIncomingData(" ");
                        OnIncomingData("-Desktop-");
                        terminalLocation = "DESKTOP";
                        break;
                    default:
                        OnIncomingData("That is not a valid command.");
                        break;
                }
                break;
            case "HACKING_USER":
                ///// INSERT COMMANDS FOR HACKING A USER HERE! ex. Extract logs, password
                switch (command[0])
                {
                    case "HELP":
                        OnIncomingData(" ");
                        OnIncomingData("List of Commands:");
                        OnIncomingData("Password PASSWORD");
                        OnIncomingData("Extract (PASSWORD, LOGS)");
                        OnIncomingData("Close   (Return to Desktop)");
                        break;
                    case "PASSWORD":
                        if (command.Length < 2)
                        {
                            OnIncomingData("ERROR: No Password entered");
                            OnIncomingData("ex. Password IDPEAS");
                        }
                        else
                        {
                            TestPassword(command[1].ToUpper());
                        }
                        break;
                    case "EXTRACT":
                        if (command.Length < 2)
                        {
                            OnIncomingData(" ");
                            OnIncomingData("List of Extract Commands:");
                            OnIncomingData("Password (Extract their Encrypted Password)");
                            OnIncomingData("Logs (Extract their Encryption Logs)");
                        }
                        else
                        {
                            switch (command[1].ToUpper())
                            {
                                case "PASSWORD":
                                    GetEncryptedPasword();
                                    break;
                                case "LOGS":
                                    GetEncryptionLogs();
                                    break;
                                default:
                                    OnIncomingData("That is not a valid command.");
                                    break;
                            }
                        }
                        break;
                    case "CLOSE":
                        OnIncomingData(" ");
                        OnIncomingData("-Desktop-");
                        terminalLocation = "DESKTOP";
                        break;
                    default:
                        OnIncomingData("That is not a valid command.");
                        break;
                }
                break;
            default:
                OnIncomingData("ERROR: RESTART SYSTEM (Restart your game please)");
                break;
        }

        terminalInput.text = "";
        terminalInput.ActivateInputField();


    }


    private void RunMath()
    {
        terminalLocation = "MATHSESSION";
        OnIncomingData(" ");
        OnIncomingData("-Math4Cash-");
        OnIncomingData("Welcome to Math4Cash!");
        if (mathAnswered)
        {
            NewMathQuestion();
        }
        else
        {
            OnIncomingData(mathQuestion); 
        }
        
    }

    private void NewMathQuestion()
    {
        System.Random random = new System.Random();
        int number1 = random.Next(1, 100);
        int number2 = random.Next(1, 100);
        mathQuestion = "What is " + number1.ToString() + " + " + number2.ToString() + "?";

        mathAnswer = number1 + number2;
        mathAnswered = false;
        OnIncomingData(mathQuestion);
    }

    private void UpdateCurrency(int c)
    {
        Send("005" + c.ToString()); 
    }

    private void RunEmail()
    {
        Debug.Log("Ran EMAIL");
    }

    private void BeginHack()
    {
        terminalLocation = "HACKING_USER";
        OnIncomingData(" ");
        OnIncomingData("You have connected to "+targetUser+"'s System.");
        OnIncomingData("Username : Admin");
        OnIncomingData("Password : ******");
    }
    private void EnterComputer()
    {
        terminalLocation = "BREACHED_USER";
        OnIncomingData(" ");
        OnIncomingData("You have breached " + targetUser + "'s System.");
        OnIncomingData("You now have ADMIN access");
    }

    private void GetEncryptedPasword()
    {
        Send("004P"+targetUser);
    }

    private void GetEncryptionLogs()
    {
        Send("004L"+targetUser);
    }
    private void TestPassword(string guess)
    {
        Send("004T" +guess +targetUser);
    }

    public void Wifi()
    {
        if (socketReady)
        {
            OnIncomingData("You have disconnected from the server.");
            CloseSocket();
        }
        else
            ConnectToServer(("THROWAWAY " + IP + " " + PORT + " " + clientName + " " + clientPassword).Split(' '));

    }
}
