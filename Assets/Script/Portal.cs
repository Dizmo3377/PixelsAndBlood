using UnityEngine;

public class Portal : InteractObject
{
    [SerializeField] private string sceneName;

    protected override void OnInteract() => Transition();
    private void Transition() => SceneController.instance.StartSceneTransition(sceneName);
}