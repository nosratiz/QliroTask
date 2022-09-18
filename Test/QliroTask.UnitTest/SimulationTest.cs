using FluentAssertions;
using Moq;
using QliroTask.UI.Contracts.Data;
using QliroTask.UI.Contracts.Request;
using QliroTask.UI.Contracts.Response;
using QliroTask.UI.Helper;
using QliroTask.UI.Services;
using Xunit;

namespace QliroTask.UnitTest;

public class SimulationTest
{
    private const int DoorPrize = 1;

    [Fact]
    public void DoorPrizeShouldBeOneOfTheThree()
    {
        var doors = Utils.SetPrizeDoor();

        doors.Should().BeOfType<List<int>>()
            .Which
            .Find(x => x == DoorPrize)
            .Should()
            .Be(1);
    }
    
    [Theory]
    [InlineData("0,0,1", 2, 0)]
    [InlineData("1,0,0",2,2)]
    [InlineData("0,1,0",1,2)]
    public void ShouldReturnCorrectDoorIndex(string doors, int doorChoice, int indexExpected)
    {
        var doorList = doors.Split(',').Select(int.Parse).ToList();
       
        var doorIndex = Utils.OpenEmptyDoor(doorChoice,doorList);
        
        doorIndex.Should().Be(indexExpected);
    }
    
    
    [Fact]
    public void WhenUserSwitchesDoor_ChanceOfWin_ShouldBeMoreThanStay()
    {
        var request = new CreateSimulationRequest(100, 1, true);

        var mockSimulator = new SimulationService();
        
        var response = mockSimulator.RunSimulationAsync(request);
        
        response.Should().BeOfType<SimulationResultDto>()
            .Which
            .SwitchWins
            .Should()
            .BeGreaterThan(response.StayWins);
    }
    
    
}