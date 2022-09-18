namespace QliroTask.UI.Contracts.Request;

public sealed record CreateSimulationRequest(
        int NumberOfSimulation, 
        int SelectedDoorNumber, 
        bool IsChangeDoor);