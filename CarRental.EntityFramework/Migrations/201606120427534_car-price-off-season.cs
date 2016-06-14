namespace CarRental.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class carpriceoffseason : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarDetailSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Value = c.String(nullable: false),
                        Car_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarSet", t => t.Car_Id, cascadeDelete: true)
                .Index(t => t.Car_Id);

            CreateTable(
                "dbo.CarSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Manufacturer = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        Office_Id = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        //PriceOffSeason = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OfficeSet", t => t.Office_Id, cascadeDelete: true)
                .Index(t => t.Office_Id);

            CreateTable(
                "dbo.OfficeSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(nullable: false),
                        Discount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ReservationSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateStarted = c.DateTime(nullable: false),
                        DateEnded = c.DateTime(nullable: false),
                        Car_Id = c.Int(nullable: false),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarSet", t => t.Car_Id)
                .Index(t => t.Car_Id);

            CreateTable(
                "dbo.ReviewSet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        Car_Id = c.Int(nullable: false),
                        Reservation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReservationSet", t => t.Reservation_Id)
                .ForeignKey("dbo.CarSet", t => t.Car_Id, cascadeDelete: true)
                .Index(t => t.Car_Id)
                .Index(t => t.Reservation_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.ReviewSet", "Car_Id", "dbo.CarSet");
            DropForeignKey("dbo.ReservationSet", "Car_Id", "dbo.CarSet");
            DropForeignKey("dbo.ReviewSet", "Reservation_Id", "dbo.ReservationSet");
            DropForeignKey("dbo.CarSet", "Office_Id", "dbo.OfficeSet");
            DropForeignKey("dbo.CarDetailSet", "Car_Id", "dbo.CarSet");
            DropIndex("dbo.ReviewSet", new[] { "Reservation_Id" });
            DropIndex("dbo.ReviewSet", new[] { "Car_Id" });
            DropIndex("dbo.ReservationSet", new[] { "Car_Id" });
            DropIndex("dbo.CarSet", new[] { "Office_Id" });
            DropIndex("dbo.CarDetailSet", new[] { "Car_Id" });
            DropTable("dbo.ReviewSet");
            DropTable("dbo.ReservationSet");
            DropTable("dbo.OfficeSet");
            DropTable("dbo.CarSet");
            DropTable("dbo.CarDetailSet");
        }
    }
}
