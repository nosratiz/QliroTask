using QliroTask.UI.Contracts.Data;
using QliroTask.UI.Contracts.Request;
using QliroTask.UI.Helper;

namespace QliroTask.UI.Services;

public class SimulationService : ISimulationService
{
    public SimulationResultDto RunSimulationAsync(CreateSimulationRequest request)
    {
        var switchWins = 0;
        var stayWins = 0;
        int doorIndex = request.SelectedDoorNumber - 1;

        for (var i = 0; i < request.NumberOfSimulation; i++)
        {
            var doors = Utils.SetPrizeDoor();

            if (request.IsChangeDoor == false)
            {
                //just check if the first door is the prize door
                stayWins += doors[doorIndex];
                continue;
            }
            
            //if he choose the prize door and switch he will lose
            if (doors[doorIndex] == 1)
                continue;

            //find empty door index
            int emptyDoorIndex = Utils.OpenEmptyDoor(request.SelectedDoorNumber, doors);

            //in order to find switch door we need to remove the empty door and selected door to find index of the switch door
            switchWins += doors[3 - doorIndex - emptyDoorIndex];
        }

        return new SimulationResultDto
        {
            StayWins = request.IsChangeDoor
                ? request.NumberOfSimulation - switchWins
                : stayWins,
            SwitchWins = request.IsChangeDoor
                ? switchWins
                : request.NumberOfSimulation - stayWins
        };
    }
}