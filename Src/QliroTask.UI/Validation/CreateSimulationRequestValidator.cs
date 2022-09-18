using FluentValidation;
using QliroTask.UI.Contracts.Request;
using QliroTask.UI.Helper;

namespace QliroTask.UI.Validation;

public class CreateSimulationRequestValidator : AbstractValidator<CreateSimulationRequest>
{
    public CreateSimulationRequestValidator()
    {
        RuleFor(dto => dto.NumberOfSimulation)
            .GreaterThan(0).WithMessage(ResponseMessage.NumberOfSimulationMustBeGreaterThanZero)
            .LessThanOrEqualTo(1000000).WithMessage(ResponseMessage.NumberOfSimulationMustBeLessThanOneMillion);

        RuleFor(dto => dto.SelectedDoorNumber)
            .GreaterThanOrEqualTo(1).WithMessage(ResponseMessage.DoorNumberMustBeOneOfNumberOneTwoThree)
            .LessThanOrEqualTo(3).WithMessage(ResponseMessage.DoorNumberMustBeOneOfNumberOneTwoThree);
    }
}