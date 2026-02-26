public class Denny_CitiesBlueprint : Blueprint
{
    // Makes the cities Ideas work on the cities board and caps tesla tower to only 1 per board

    public override void Init(GameDataLoader loader)
    {
        base.Init(loader);

        if (this.Id == "denny_blueprint_tesla_tower")
        {
            HasMaxAmountOnBoard = true;
            MaxAmountOnBoard = 1;
        }


        CardUpdateType = CardUpdateType.Cities; 
    }
}