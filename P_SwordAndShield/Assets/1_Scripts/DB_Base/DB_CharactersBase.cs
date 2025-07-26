using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Databases/Character Database")]
public class DB_CharactersBase : DB_ScriptableBase
{
    [SerializeField] private List<CharacterEntry> CharacterList = new List<CharacterEntry>();
    public List<CharacterEntry> Characters => CharacterList;

    public override void Initialize()
    {

    }

    public override int GetEntryCount()
    {
        return CharacterList.Count;
    }

    // Helper method to find a character by ID
    public CharacterEntry GetCharacterByID(Character PassedID)
    {
        return CharacterList.Find(c => c.Get_ID == PassedID);
    }
}
