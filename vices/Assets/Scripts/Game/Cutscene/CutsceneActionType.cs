namespace Vices.Scripts.Game
{
    public enum CutsceneActionType
    {
        None                = 0,
        TargetMove          = 1,
        CameraMove          = 2,
        PlayDialogue        = 3,
        PlayPlayerAnimation = 4,
        PlayAnimation       = 5,
        PlaySound           = 6,
        WaitForInput        = 7,
        ExecuteEvent        = 8,
        EnableGameObject    = 9,
        LoadScene           = 10,
        EndCutscene         = 11,
        ReturnToPlayer      = 12,
        MovePlayer          = 13,
        BlackScreenShow     = 14,
        BlackScreenHide     = 15
    }
}