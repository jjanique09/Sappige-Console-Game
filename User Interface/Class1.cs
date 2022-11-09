
    public class Class1
    {

    }







/// <ToDo>
///Je wil hebt een methode die bepaald of een deur open mag voor elk verschillend deur type - Fixed
/// Deze methode geeft een booelean terug maar hoe decorate je dan 2 verschillende deuren? - Fixed
/// Er moeten parameters worden doorgevoerd om bepaalde variabelen te setten zoals: color, no_of_stones.
/// het variabele "Type" word bepaald door de decorator
/// </ToDo>   private Door CreateDoors(JToken items)
/// 
/*{
    Door itemList = new Door();
    foreach (var jsonItem in items)
    {
        var type = jsonItem["type"].Value<string>();
        Door a = new Door();


        PlainDoor plainPizzaObj = new PlainDoor();
        DoorDecorator chickenPizzaDecorator = new ColoredDoorDecorator(plainPizzaObj);
        Door vegPizzaDecorator = new VegPizzaDecorator(chickenPizzaDecorator);

        Console.WriteLine("1 " + chickenPizzaDecorator.Open());
        Console.WriteLine("1 " + vegPizzaDecorator.Open());

    }
    return itemList;
}*/