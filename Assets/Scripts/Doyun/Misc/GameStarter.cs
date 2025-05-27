using Fusion;

public class GameStarter : NetworkBehaviour
{
    public void OnClickStart()
    {
        Runner.SessionInfo.IsOpen = false;
        Runner.LoadScene(SceneRef.FromIndex(3));
    }
}