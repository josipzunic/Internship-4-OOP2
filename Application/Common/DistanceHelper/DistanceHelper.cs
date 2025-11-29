namespace Infrastructure.Common;

public static class DistanceHelper
{
    public static decimal Distance(decimal user1Lat, decimal user1Lng, decimal user2Lat, decimal user2Lng)
    {
        decimal R = 6371;
        decimal dLat = DegreesToRadians(user1Lat - user2Lat);
        decimal dLng = DegreesToRadians(user1Lng - user2Lng);

        user1Lat = DegreesToRadians(user1Lat);
        user2Lat = DegreesToRadians(user2Lat);

        double a = Math.Sin((double)dLat / 2) * Math.Sin((double)dLat / 2) +
                   Math.Cos((double)user1Lat) * Math.Cos((double)user2Lat) *
                   Math.Sin((double)dLng / 2) * Math.Sin((double)dLng / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * (decimal)c;
    }

    public static decimal DegreesToRadians(decimal degrees) => degrees*(decimal)Math.PI/180;
}