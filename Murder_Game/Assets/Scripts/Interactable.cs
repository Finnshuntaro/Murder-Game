using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage; // Message shown to the player when they are near this interactable object

    // Called by external scripts to trigger interaction logic
    public void BaseInteract()
    {
        Interact(); // Calls the overridden interaction method
    }

    // Meant to be overridden by derived classes to define specific interaction behavior
    protected virtual void Interact() 
    {
        // Default is empty; override in subclasses
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
