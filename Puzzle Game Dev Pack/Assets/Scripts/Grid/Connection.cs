public class Connection
{
    TileEnum colorType = TileEnum.BLANK_TILE; //define it as none to start? 
    private int lengthOfConnection = 0;

    public Connection(int lengthOfConnection, TileEnum colorType)
    {
       this.lengthOfConnection = lengthOfConnection;
       this.colorType = colorType;
    }

    //GETTERS AND SETTERS 
    public TileEnum getColorType()
    {
        return colorType;
    }
   public int getLengthOfConnection()
    {
        return lengthOfConnection;
    }
}
