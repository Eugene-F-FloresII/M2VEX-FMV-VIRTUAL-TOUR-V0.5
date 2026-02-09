using UnityEngine;


namespace Data

{
    
    [CreateAssetMenu (fileName = "PlayerNameData", menuName = "Data/Player Name")]
    public class PlayerNameScriptableObject : ScriptableObject
    {
        public string _name;
    }

}
