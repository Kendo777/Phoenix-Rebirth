using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Public 
    // List of nodes
    public List<Node> nodes = new List<Node>();

    [Space]
    // Actually useful stuff
    public GameObject pivotPrefab;
    public GameObject nodeButtonPrefab;
    public Transform nodeButtonsContainer;
    private int currentStage = 0;

    [Space]

    [Header("Tween Settings")]
    public Camera mainCamera;
    public float transitionDuration = 1.5f;
    public Ease lookEase;
    public float cameraArc;
    public Vector3 planetOffset;

    [Space]
    // Offset for the planets to spawn to be inside the screen
    public Vector2 visualOffset;
    #endregion

    #region Private Attributes
    private Transform worldSphere;
    #endregion

    #region Monobehaviour
    void Start()
    {
        worldSphere = transform.GetChild(0);

        foreach (Node n in nodes)
        {
            SpawnNode(n);
        }

        if (nodes.Count > 0)
        {
            CameraLook(nodes[currentStage]);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(nodeButtonsContainer.GetChild(0).gameObject);
        }
    }

    void Update()
    {
        // bool pressedEnter = false;
        NodeSelection();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleSelection();
            // pressedEnter = true;
            Debug.Log("You selected stage " + (currentStage + 1) + "!");
        }

        // if (pressedEnter)
        // {
        //     mainCamera.transform.LookAt(worldSphere);
        // }
    }

    /// <summary>
    /// Draw the gizmos of where the planets will be on the editor (before running the programme)
    /// </summary>
    void OnDrawGizmos()
    {

#if UNITY_EDITOR
        Gizmos.color = Color.red;

        // Only draw if there is at least one stage
        if (nodes.Count > 0)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                // We create two empty objects to handle positions of the gizmos
                GameObject point = new GameObject();
                GameObject parent = new GameObject();

                // Move the object to the surface of the sphere and stablish the hierarchy 
                point.transform.position += - new Vector3(0.0f, 0.0f, 0.5f);
                point.transform.parent = parent.transform;

                // Set the visual offset
                parent.transform.eulerAngles = new Vector3(visualOffset.y, -visualOffset.x, 0.0f);

                // Spin the parent object, which will define the position of the gizmo, and then the object and draw the gizmo
                parent.transform.eulerAngles += new Vector3(nodes[i].y, -nodes[i].x, 0);
                Gizmos.DrawSphere(point.transform.position, 0.05f);

                // Destroy the objects, as we won't need them anymore
                DestroyImmediate(point);
                DestroyImmediate(parent);
            }
        }
#endif

    }
    #endregion

    #region Methods
    /// <summary>
    /// This method rotates the camera over the planets and looks at the appropiate one according to the stage we are in
    /// </summary>
    /// <param name="n"> The node we are currently in </param>
    private void CameraLook(Node n)
    {
        Transform cameraParent = mainCamera.transform.parent;
        Transform cameraPivot = cameraParent.parent;

        cameraParent.DOLocalRotate(new Vector3(n.y, 0.0f, 0.0f), transitionDuration, RotateMode.Fast).SetEase(lookEase);
        cameraPivot.DOLocalRotate(new Vector3(0.0f, -n.x, 0.0f), transitionDuration, RotateMode.Fast).SetEase(lookEase);

        FindObjectOfType<FollowTarget>().target = n.visualPoint;
    }

    /// <summary>
    /// Handles the spawn of the planets on the correct position (mutable)
    /// </summary>
    /// <param name="n"> The node we are currently in </param>
    private void SpawnNode(Node n)
    {
        GameObject point = Instantiate(pivotPrefab, worldSphere);
        point.transform.eulerAngles = new Vector3(n.y + visualOffset.y, -n.x - visualOffset.x, 0.0f);
        n.visualPoint = point.transform.GetChild(0);

        SpawnNodeButton(n);
    }

    /// <summary>
    /// Handles the spawn of the buttons (mutable)
    /// </summary>
    /// <param name="n"></param>
    private void SpawnNodeButton(Node n)
    {
        Node node = n;
        Button nodeButton = Instantiate(nodeButtonPrefab, nodeButtonsContainer).GetComponent<Button>();
        nodeButton.onClick.AddListener(() => CameraLook(node));

        nodeButton.transform.GetChild(0).GetComponentInChildren<Text>().text = n.name;
    }

    /// <summary>
    /// Handles the selection of stage
    /// </summary>
    private void NodeSelection()
    {
        #region Buttons Control
        Button play = nodeButtonsContainer.GetChild(0).GetComponent<Button>();
        Button options = nodeButtonsContainer.GetChild(1).GetComponent<Button>();
        Button controls = nodeButtonsContainer.GetChild(2).GetComponent<Button>();
        Button exit = nodeButtonsContainer.GetChild(3).GetComponent<Button>();

        play.onClick.AddListener(() => HandleButtonSelection(play));
        options.onClick.AddListener(() => HandleButtonSelection(options));
        controls.onClick.AddListener(() => HandleButtonSelection(controls));
        exit.onClick.AddListener(() => HandleButtonSelection(exit));
        #endregion

        CameraLook(nodes[currentStage]);
    }

    /// <summary>
    /// Handles
    /// </summary>
    /// <param name="b"></param>
    private void HandleButtonSelection(Button b)
    {
        string buttonName = b.GetComponent<NodeButton>().text.text;

        if (buttonName == "Play")
        {
            currentStage = 0;
        }

        if (buttonName == "Options")
        {
            currentStage = 1;
        }

        if (buttonName == "Controls")
        {
            currentStage = 2;
        }

        if (buttonName == "Exit")
        {
            currentStage = 3;
        }
    }

    private void HandleSelection()
    {
        Vector3 middlePoint = new Vector3();

        switch (currentStage)
        {
            case 0:
                Sequence goToShipSelection = DOTween.Sequence();
                middlePoint = (mainCamera.transform.position - worldSphere.GetChild(0).transform.position) * 0.5f;

                goToShipSelection.Append(mainCamera.transform.DOJump(middlePoint, 0.6f, 1, transitionDuration));
                goToShipSelection.Join(mainCamera.transform.DORotate(worldSphere.GetChild(0).transform.eulerAngles, 0.7f, RotateMode.Fast));
                Invoke("LoadShipSelection", 0.7f);
                break;

            case 1:
                Sequence goToOptions = DOTween.Sequence();
                middlePoint = (mainCamera.transform.position - worldSphere.GetChild(1).transform.position) * 0.5f;

                goToOptions.Append(mainCamera.transform.DOJump(middlePoint, 0.6f, 1, transitionDuration));
                goToOptions.Join(mainCamera.transform.DORotate(worldSphere.GetChild(1).transform.eulerAngles, 0.7f, RotateMode.Fast));
                Invoke("LoadOptions", 0.7f);
                break;

            case 2:
                Sequence goToControls = DOTween.Sequence();
                middlePoint = (mainCamera.transform.position - worldSphere.GetChild(2).transform.position) * 0.5f;

                goToControls.Append(mainCamera.transform.DOJump(middlePoint, 0.6f, 1, transitionDuration));
                goToControls.Join(mainCamera.transform.DORotate(worldSphere.GetChild(2).transform.eulerAngles, 0.7f, RotateMode.Fast));
                Invoke("LoadControls", 0.7f);
                break;

            case 3:
                Sequence goToExit = DOTween.Sequence();
                middlePoint = (mainCamera.transform.position - worldSphere.GetChild(3).transform.position) * 0.5f;

                goToExit.Append(mainCamera.transform.DOJump(middlePoint, 0.6f, 1, transitionDuration));
                goToExit.Join(mainCamera.transform.DORotate(worldSphere.GetChild(3).transform.eulerAngles, 0.7f, RotateMode.Fast));
                Invoke("ExitGame", 0.7f);
                break;
        }
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    private void LoadOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    private void LoadShipSelection()
    {
        SceneManager.LoadScene("ShipSelection");
    }
    #endregion
}

/// <summary>
/// This class handles the node (future planet) data
/// </summary>
[System.Serializable]
public class Node
{
    public string name;

    [Range(-180, 180)]
    public float x;
    [Range(-89, 89)]
    public float y;

    [HideInInspector]
    public Transform visualPoint;
}