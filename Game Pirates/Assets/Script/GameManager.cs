public static class setGame
{
    static bool isGame=false;
    static int coin = 0;
    static float distanceMove = 0;
    static int speed = 5;
    static public void setIsGame(bool value)
    {
        isGame = value;
    }
    static public bool getIsGame()
    {
        return isGame;
    }
    static public void plusCoin()
    {
        coin++;
    }
    static public int getCoin() 
    {
        return coin;
    }
    static public void plusDistanceMove(float DistanceTraved) 
    {
        distanceMove += DistanceTraved;
    }
    static public float getDistanceMove()
    {
        return distanceMove;
    }
    static public void setSpeed(int sp)
    {
        speed = sp;
    }
    static public int getSpeed()
    {
        return speed;
    }
}
