using UnityEngine;

public class NPCTracker : MonoBehaviour
{
    public static NPCTracker Instance { get; private set; }

    private NPCScript[] allNPCs;

    void Awake()
    {
        // Singleton pattern using FindFirstObjectByType for compatibility across scenes
        if (Instance == null)
        {
            Instance = UnityEngine.Object.FindFirstObjectByType<NPCTracker>();

            if (Instance == null)
            {
                Debug.LogError("NPCTracker could not be found in the scene.");
            }
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Optional: persist this object across scene loads
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Automatically find all NPCs in the scene
        allNPCs = UnityEngine.Object.FindObjectsByType<NPCScript>(FindObjectsSortMode.None);
    }

    // Check if all NPCs in the scene have been spoken to
    public bool AllNPCsSpokenTo()
    {
        if (allNPCs == null || allNPCs.Length == 0)
        {
            Debug.LogWarning("No NPCs found by NPCTracker.");
            return false;
        }

        foreach (var npc in allNPCs)
        {
            if (!npc.hasSpoken)
                return false;
        }

        return true;
    }

    // Optionally expose NPCs if needed by other systems
    public NPCScript[] GetAllNPCs()
    {
        return allNPCs;
    }
}