using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene_Basic : MonoBehaviour
{
    [SerializeField] int sceneNum;
    
    public void OnClick()
    {
        SceneManager.LoadScene(sceneNum);
    }

}
