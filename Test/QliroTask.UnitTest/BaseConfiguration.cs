using FluentValidation;
using Moq;
using QliroTask.UI.Contracts.Request;
using QliroTask.UI.Controllers;
using QliroTask.UI.Services;

namespace QliroTask.UnitTest;

public class BaseConfiguration
{
    private ISimulationService _simulationService;
    private IValidator<CreateSimulationRequest> _validator;

    internal BaseConfiguration()
    {
        _simulationService = new Mock<ISimulationService>().Object;
        _validator = new Mock<IValidator<CreateSimulationRequest>>().Object;
    }

    internal BaseConfiguration WithSimulationService(ISimulationService simulationService)
    {
        _simulationService = simulationService;
        return this;
    }
    
    internal BaseConfiguration WithValidator(IValidator<CreateSimulationRequest> validator)
    {
        _validator = validator;
        return this;
    }

    internal HomeController BuildHomeController() => new(_simulationService, _validator);
}