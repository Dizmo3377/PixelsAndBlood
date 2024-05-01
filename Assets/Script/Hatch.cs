public class Hatch : InteractObject
{
    private bool entered = false;
    protected override void OnInteract() => Transition();
    private void Transition()
    {
        if(entered) return;
        entered = true; 

        LevelData.instance.IterateLevelData();
        int stage = LevelData.instance.stage;

        if (stage == 2) SceneController.instance.StartSceneTransition("Final");

        FindFirstObjectByType<PlayerController>().Disable();
        SceneController.instance.StartSceneTransition($"Dungeon_{stage}");
    }
}