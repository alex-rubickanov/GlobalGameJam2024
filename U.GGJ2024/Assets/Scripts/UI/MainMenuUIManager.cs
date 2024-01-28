using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance;
    [SerializeField] PlayerInputManager playerInputManager;
    [SerializeField] Volume volume;
    VolumeProfile profile;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject characterSelectionPanel;
    [SerializeField] GameObject cameraPicture;

    [Header("Character Selection")]
    [SerializeField] GameObject[] joinBtns;
    [SerializeField] GameObject[] arrows;
    [SerializeField] GameObject[] selectChrBtn;
    [SerializeField] GameObject[] readyBtn; 
    int playerCount;

    public int readyCount;
    private void Awake()
    {
        Instance = this;
        profile = volume.sharedProfile;
        playerInputManager.onPlayerJoined += DisableJoinBtns;
    }
    void Start()
    {
        mainMenuPanel.SetActive(true);
        characterSelectionPanel.SetActive(false);
        cameraPicture.SetActive(false);
        SetBlurBG(true);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SetBlurBG(false);
            mainMenuPanel.SetActive(false);
            characterSelectionPanel.SetActive(true);
            cameraPicture.SetActive(true);
        }

        if (readyCount == 3)
        {
            SceneManager.LoadScene(1);
        }
    }


    public void SetBlurBG(bool value)
    {
        if (!profile.TryGet<DepthOfField>(out var dof))
        {
            dof = profile.Add<DepthOfField>(false);
        }
        dof.active = value;
    }

    public void DisableJoinBtns(PlayerInput obj)
    {
        joinBtns[playerCount].SetActive(false);
        arrows[playerCount].SetActive(true);
        selectChrBtn[playerCount].SetActive(true);
        playerCount++;
    }
    
    public void EnableReadyBtn(int playerIndex)
    {
        arrows[playerIndex].SetActive(false);
        selectChrBtn[playerIndex].SetActive(false);
        readyBtn[playerIndex].SetActive(true);
        readyCount++;
    }
}
