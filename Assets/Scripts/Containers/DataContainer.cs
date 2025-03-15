using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public static DataContainer Instance { get; private set; }

    [SerializeField] private DiceFaceSpriteSO[] defaultFaces;
    public DiceFaceSpriteSO[] DefaultFaces => defaultFaces;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
