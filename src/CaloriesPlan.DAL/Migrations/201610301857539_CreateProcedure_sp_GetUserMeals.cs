namespace CaloriesPlan.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProcedure_sp_GetUserMeals : DbMigration
    {
        private readonly string procedureName = "dbo.sp_GetUserMeals";

        public override void Up()
        {
            CreateStoredProcedure(
                procedureName,
                p => new
                {
                    UserName = p.String(maxLength: 200),
                    DateFrom = p.DateTime(),
                    DateTo = p.DateTime(),
                    TimeFrom = p.DateTime(),
                    TimeTo = p.DateTime()
                },
                body:
@"
DECLARE @StartDayTime TIME = '00:00:00.0000000',
    @EndDayTime TIME = '23:59:59.9999999',
    @TimeFromInTime TIME = CAST(@TimeFrom AS TIME),
    @TimeToInTime TIME = CAST(@TimeTo AS TIME)
    
    
IF @TimeFromInTime < @TimeToInTime BEGIN
SELECT DISTINCT m.*
FROM Meal m
INNER JOIN AspNetUsers u ON u.Id = m.UserID
WHERE u.UserName = @UserName
AND m.EatingDate BETWEEN @DateFrom AND @DateTo
AND CAST(m.EatingDate AS TIME) BETWEEN @TimeFromInTime AND @TimeToInTime
ORDER BY m.EatingDate DESC
END
ELSE BEGIN
    
SELECT DISTINCT m.*
FROM Meal m
INNER JOIN AspNetUsers u ON u.Id = m.UserID
WHERE u.UserName = @UserName
AND m.EatingDate BETWEEN @DateFrom AND @DateTo
AND ((CAST(m.EatingDate AS TIME) BETWEEN @TimeFromInTime AND @EndDayTime)
    	OR (CAST(m.EatingDate AS TIME) BETWEEN @StartDayTime AND @TimeToInTime))
ORDER BY m.EatingDate DESC
    		
END"
            );
        }

        public override void Down()
        {
            DropStoredProcedure(procedureName);
        }
    }
}
