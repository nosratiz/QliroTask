using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using QliroTask.UI.Contracts.Request;
using QliroTask.UI.Contracts.Response;
using QliroTask.UI.Services;


namespace QliroTask.UI.Controllers;

public class HomeController : Controller
{
    private readonly ISimulationService _simulationService;
    private readonly IValidator<CreateSimulationRequest> _validator;

    public HomeController(ISimulationService simulationService,
        IValidator<CreateSimulationRequest> validator)
    {
        _simulationService = simulationService;
        _validator = validator;
    }


    public IActionResult Index() => View(new SimulationResponse());


    [HttpPost]
    public async Task<IActionResult> Index(CreateSimulationRequest request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return View(new SimulationResponse
                {Messages = validation.Errors.Select(x => x.ErrorMessage).ToList(), IsSuccess = false});
        
        var response = _simulationService.RunSimulationAsync(request);
        
        return View(new SimulationResponse
            {StayWins = response.StayWins, SwitchWins = response.SwitchWins });
    }


    public IActionResult Guideline() => View();
}