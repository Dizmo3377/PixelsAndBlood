using UnityEngine;

public class Portal : InteractObject
{
    [SerializeField] private string sceneName;

    protected override void OnInteract() => Transition();
    private void Transition()
    {
        FindObjectOfType<PlayerController>().Disable();
        SceneController.instance.StartSceneTransition(sceneName);
    }
}