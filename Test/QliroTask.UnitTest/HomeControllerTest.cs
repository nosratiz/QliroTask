using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QliroTask.UI.Contracts.Data;
using QliroTask.UI.Contracts.Request;
using QliroTask.UI.Contracts.Response;
using QliroTask.UI.Helper;
using QliroTask.UI.Services;
using Xunit;

namespace QliroTask.UnitTest;

public class HomeControllerTest : BaseConfiguration
{
    [Fact]
    public void WhenGetIndex_ThenReturnView()
    {
        var controller = new BaseConfiguration().BuildHomeController();

        var result = controller.Index();

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<SimulationResponse>()
            .Which.IsSuccess.Should().Be(true);
    }


    [Fact]
    public async Task WhenInvalidNumberOfSimulation_Send_ThenReturnViewWithError()
    {
        var mockData = new Mock<IValidator<CreateSimulationRequest>>();

        mockData.Setup(x => x.ValidateAsync(new CreateSimulationRequest
                    (-299, 2, false)
                , CancellationToken.None))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new("NumberOfSimulation", ResponseMessage.NumberOfSimulationMustBeGreaterThanZero),
            }));

        var controller = new BaseConfiguration()
            .WithValidator(mockData.Object)
            .BuildHomeController();

        var result = await controller.Index(new CreateSimulationRequest
            (-299, 2, false), CancellationToken.None);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<SimulationResponse>()
            .Which.IsSuccess.Should().Be(false);

        result.Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<SimulationResponse>()
            .Which.Messages.Should().Contain(ResponseMessage.NumberOfSimulationMustBeGreaterThanZero);
    }

    [Fact]
    public async Task WhenInvalidSelectedDoorNumber_Send_ThenReturnViewWithError()
    {
        var mockData = new Mock<IValidator<CreateSimulationRequest>>();

        mockData.Setup(x => x.ValidateAsync(new CreateSimulationRequest
                    (299, -2, false)
                , CancellationToken.None))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new("SelectedDoorNumber", ResponseMessage.DoorNumberMustBeOneOfNumberOneTwoThree),
            }));

        var controller = new BaseConfiguration()
            .WithValidator(mockData.Object)
            .BuildHomeController();

        var result = await controller.Index(new CreateSimulationRequest
            (299, -2, false), CancellationToken.None);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<SimulationResponse>()
            .Which.IsSuccess.Should().Be(false);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should()
            .BeOfType<SimulationResponse>()
            .Which.Messages.Should().Contain(ResponseMessage.DoorNumberMustBeOneOfNumberOneTwoThree);
    }


    [Fact]
    public async Task WhenValidData_Send_ThenReturnViewWithSuccess()
    {
        var mockData = new Mock<IValidator<CreateSimulationRequest>>();

        var mockService = new Mock<ISimulationService>();

        var request = new CreateSimulationRequest(100, 2, false);

        mockData.Setup(x => x.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        mockService.Setup(x => x.RunSimulationAsync(request))
            .Returns(new SimulationResultDto { StayWins = 33, SwitchWins = 67 });

        var controller = new BaseConfiguration()
            .WithValidator(mockData.Object)
            .WithSimulationService(mockService.Object)
            .BuildHomeController();

        var result = await controller.Index(request, CancellationToken.None);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<SimulationResponse>()
            .Which.IsSuccess.Should().Be(true);

        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<SimulationResponse>()
            .Which.StayWins.Should().Be(33);
    }
}