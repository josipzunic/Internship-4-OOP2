namespace Infrastructure.Common;

public static class DistanceHelper
{
    public static double Distance(double user1Lat, double user1Lng, double user2Lat, double user2Lng)
    {
        double r = 6371;
        double dLat = DegreesToRadians(user1Lat - user2Lat);
        double dLng = DegreesToRadians(user1Lng - user2Lng);

        user1Lat = DegreesToRadians(user1Lat);
        user2Lat = DegreesToRadians(user2Lat);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(user1Lat) * Math.Cos(user2Lat) *
                   Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return r * c;
    }

    public static double DegreesToRadians(double degrees) => degrees*Math.PI/180;
}