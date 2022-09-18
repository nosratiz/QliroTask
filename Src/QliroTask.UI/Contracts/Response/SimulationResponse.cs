namespace QliroTask.UI.Contracts.Response;

public class SimulationResponse : Message
{
    public int StayWins { get; set; }

    public int SwitchWins { get; set; }
}

public class Message
{
    public bool IsSuccess { get; set; } = true;

    public List<string> Messages { get; set; } = new();
}