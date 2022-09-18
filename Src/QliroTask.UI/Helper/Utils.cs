namespace QliroTask.UI.Helper;

public static class Utils
{
    public static List<int> SetPrizeDoor()
    {
        var doors = new List<int>(3){0, 0, 0};
        var rnd = new Random();
        var prizeDoor = rnd.Next(0, doors.Count);
        doors[prizeDoor] = 1;
        return doors;
    }

    public static int OpenEmptyDoor(int selectedDoor, List<int> doors)
    {
        var emptyDoor = 0;
        
        
        //find the index that not selected and not prize door
        for (var i = 0; i < doors.Count; i++)
        {
            if (doors[i] != 0 || i == (selectedDoor -1)) continue;
           
            emptyDoor = i;
            break;
        }

        return emptyDoor;
    }
}