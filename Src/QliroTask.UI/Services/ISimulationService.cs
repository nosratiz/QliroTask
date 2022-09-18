using QliroTask.UI.Contracts.Data;
using QliroTask.UI.Contracts.Request;

namespace QliroTask.UI.Services;

public interface ISimulationService
{
    SimulationResultDto RunSimulationAsync(CreateSimulationRequest request);
}