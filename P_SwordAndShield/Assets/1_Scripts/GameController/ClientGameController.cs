using UnityEngine;

public class ClientGameController : MonoBehaviour
{
    //Needs to be Singleton
    public static ClientGameController Controller { get; private set; }
    [field: SerializeField] public DB_CharactersBase Characters { get; private set; }
    public void Awake()
    {
        Controller = this;
    }
}
