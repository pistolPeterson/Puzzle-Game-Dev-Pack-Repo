/// <summary>
/// The base connection object, holds data about the color type and the length of the connection. Note
/// that it is not inhereted from monobehavior, it must be instantiated. 
/// </summary>
public class Connection
{
    private TileEnum colorType = TileEnum.BLANK_TILE; 
    private int lengthOfConnection = 0;

    /// <summary>
    /// Simple connection constructor 
    /// </summary>
    public Connection(int lengthOfConnection, TileEnum colorType)
    {
       this.lengthOfConnection = lengthOfConnection;
       this.colorType = colorType;
    }

    //GETTERS AND SETTERS 
    public TileEnum GetColorType()
    {
        return colorType;
    }
   public int GetLengthOfConnection()
    {
        return lengthOfConnection;
    }
}
