using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    [Header("Win Game")]
    [SerializeField] bool win;
    [SerializeField] int numberOfShuffleCommands;


    [SerializeField] List<CubeCommands> cubeUndoCommands = new List<CubeCommands>();
    [SerializeField] List<CubeCommands> cubeRedoCommands = new List<CubeCommands>();
    [SerializeField] List<CubeCommands> shuffleCommands = new List<CubeCommands>();
    [SerializeField] List<CubeCommands> solveCommands = new List<CubeCommands>();
    [SerializeField] RaycastersHandler[] faceRaycasters;
    [SerializeField] SORotateObject[] commands = new SORotateObject[4];
    [SerializeField] Transform[] cubeAndFaces;
    [SerializeField] GameObject winPanel;


    [SerializeField] Vector3 newRotation;

    [SerializeField] float intervalsBetweenUndoCommands;
    [SerializeField] bool lerping,solving, shuffled;


    // Events
    [SerializeField] UnityEvent rotateCubies;
    public static GameManager Instance { get => instance; set => instance = value; }
    public List<CubeCommands> SolveCommands { get => solveCommands; set => solveCommands = value; }
    public List<CubeCommands> CubeUndoCommands { get => cubeUndoCommands; set => cubeUndoCommands = value; }
    public List<CubeCommands> CubeRedoCommands { get => cubeRedoCommands; set => cubeRedoCommands = value; }
    public bool Lerping
    {
        get => lerping; set
        {
            if (solving == false)
                lerping = value;
        }
    }

    public bool Win { get => win; }
    public bool Shuffled { get => shuffled; }

    private void Awake()
    {
        //instance ??= this;
        instance = this;
        faceRaycasters = FindObjectsOfType<RaycastersHandler>();
        
    }


    public void SetWinBoolean()
    {
       StartCoroutine(DisplayWinScreen());
    }

    IEnumerator DisplayWinScreen()
    {

        bool temp = true;

        for (int i = 0; i < faceRaycasters.Length; i++)
            faceRaycasters[i].gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < faceRaycasters.Length; i++)
        {
            if (faceRaycasters[i].WinFace == false) temp = false;
        }

        if (cubeUndoCommands.Count>0 && solving==false || shuffled==true)
            win = temp;

        if (win == true) winPanel.SetActive(true);

        for (int i = 0; i < faceRaycasters.Length; i++)
            faceRaycasters[i].gameObject.SetActive(false);
    }

    public void RestartGame(GameObject winPanel)
    {
        SceneManager.LoadScene("Rubik Cube");
        //cubeRedoCommands.Clear();
        //cubeUndoCommands.Clear();
        //shuffleCommands.Clear();
        //solveCommands.Clear();
        //shuffled = false;
        //win = false;
        //winPanel.SetActive(false);

    }

    public void Solve()
    {
        if (lerping == false)
        {
            StartCoroutine(SolveCoroutine(intervalsBetweenUndoCommands, solveCommands));
        }
  
    }

    public void Shuffle()
    {
        if (lerping == false)
        {
      
            shuffleCommands.Clear();   
            for (int i = 0; i< numberOfShuffleCommands; i++)
            {
                // 0 is the CUBE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                int transformNumber = Random.Range(0, 4);
                int commandNumber=0;

                CubeOrFace cubeState = CubeOrFace.Cube;

                if (transformNumber == 0)
                {
                    commandNumber = Random.Range(0, 4);
                    cubeState = CubeOrFace.Cube;

                }
                else
                {
                    commandNumber = Random.Range(2, 4);
                    cubeState = CubeOrFace.Face;
                }



                CubeCommands command = new CubeCommands(cubeAndFaces[transformNumber], commands[commandNumber], cubeState);
                shuffleCommands.Add(command);   
            }

            StartCoroutine(ExcuteCommandsInList(intervalsBetweenUndoCommands,shuffleCommands));

        }
    }

    public void Undo()
    {
        if (lerping == false)
        {
            if (cubeUndoCommands.Count > 0)
            {
                cubeUndoCommands[cubeUndoCommands.Count - 1].Command.UndoRotation(cubeUndoCommands[cubeUndoCommands.Count - 1].Trans, cubeUndoCommands[cubeUndoCommands.Count - 1].Shape);
                if (cubeUndoCommands[cubeUndoCommands.Count - 1].Shape == CubeOrFace.Cube)
                    rotateCubies.Invoke();
                CubeRedoCommands.Add(cubeUndoCommands[cubeUndoCommands.Count - 1]);
                cubeUndoCommands.Remove(cubeUndoCommands[cubeUndoCommands.Count - 1]);
                solveCommands.Remove(solveCommands[solveCommands.Count - 1]);
            }
        }
    }


    public void Redo()
    {
        if (lerping == false)
        {


            if (CubeRedoCommands.Count > 0)
            {
                CubeRedoCommands[CubeRedoCommands.Count - 1].Command.DoRotation(CubeRedoCommands[CubeRedoCommands.Count - 1].Trans, CubeRedoCommands[CubeRedoCommands.Count - 1].Shape);
                if (CubeRedoCommands[CubeRedoCommands.Count - 1].Shape == CubeOrFace.Cube)
                    rotateCubies.Invoke();
                cubeUndoCommands.Add(CubeRedoCommands[CubeRedoCommands.Count - 1]);
                solveCommands.Add(CubeRedoCommands[CubeRedoCommands.Count - 1]);
                CubeRedoCommands.Remove(CubeRedoCommands[CubeRedoCommands.Count - 1]);
            }
        }
    }


    IEnumerator SolveCoroutine(float intervalBetweenActions, List<CubeCommands> ListOfCommands)
    {

        lerping = true;
        solving = true;

        Debug.Log(ListOfCommands.Count);
        yield return new WaitForSeconds(intervalBetweenActions);

        if (ListOfCommands.Count > 0)
            for (int i = ListOfCommands.Count - 1; i >= 0; i--)
            {
                ListOfCommands[i].Command.UndoRotation(ListOfCommands[i].Trans, ListOfCommands[i].Shape);
                if (ListOfCommands[i].Shape == CubeOrFace.Cube)
                    rotateCubies.Invoke();
                ListOfCommands.Remove(ListOfCommands[i]);
                yield return new WaitForSeconds(intervalBetweenActions);
            }
        Debug.Log(ListOfCommands.Count);


        solving = false;
        lerping = false;
        SetWinBoolean();

    }

    IEnumerator ExcuteCommandsInList(float intervalBetweenActions, List<CubeCommands> ListOfCommands)
    {

        lerping = true;
        solving = true;
        Debug.Log(ListOfCommands.Count);
        yield return new WaitForSeconds(intervalBetweenActions);

        if (ListOfCommands.Count > 0)
            for (int i = ListOfCommands.Count - 1; i >= 0; i--)
            {
                ListOfCommands[i].Command.Execute(ListOfCommands[i].Trans, ListOfCommands[i].Shape);
                if (ListOfCommands[i].Shape == CubeOrFace.Cube)
                    rotateCubies.Invoke();
                ListOfCommands.Remove(ListOfCommands[i]);
                yield return new WaitForSeconds(intervalBetweenActions);
            }

        lerping = false;
        solving = false;
        shuffled = true;

        Debug.Log(ListOfCommands.Count);
        cubeUndoCommands.Clear();

    }
}


public enum CubeOrFace
{
    Cube,Face
}
