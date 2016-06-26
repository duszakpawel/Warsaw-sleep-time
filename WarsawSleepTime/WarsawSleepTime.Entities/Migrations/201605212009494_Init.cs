namespace WarsawSleepTime.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalFeatureInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdditionalFeature = c.Int(nullable: false),
                        UserPreference_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserPreferences", t => t.UserPreference_Id)
                .Index(t => t.UserPreference_Id);
            
            CreateTable(
                "dbo.CouchsurfingOffers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Date = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Street = c.String(),
                        Number = c.String(),
                        District = c.String(),
                        Image = c.Binary(),
                        Latitude = c.Double(nullable: false),
                        Longtitude = c.Double(nullable: false),
                        Apartment = c.String(),
                        Client_Id = c.Int(),
                        Owner_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Client_Id)
                .ForeignKey("dbo.Customers", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Client_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.Binary(),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(),
                        PhoneNumber = c.Int(nullable: false),
                        PlaceOfResidence_Street = c.String(),
                        PlaceOfResidence_Number = c.String(),
                        PlaceOfResidence_Country = c.String(),
                        PlaceOfResidence_City = c.String(),
                        Email = c.String(),
                        User_Id = c.String(maxLength: 128),
                        UserPreference_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.UserPreferences", t => t.UserPreference_Id)
                .Index(t => t.User_Id)
                .Index(t => t.UserPreference_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserPreferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LanguageInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.Int(nullable: false),
                        UserPreference_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserPreferences", t => t.UserPreference_Id)
                .Index(t => t.UserPreference_Id);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendshipDate = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                        CustomerFriend_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Customers", t => t.CustomerFriend_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.CustomerFriend_Id);
            
            CreateTable(
                "dbo.HistoricalOffers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        Client = c.String(),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Friendships", "CustomerFriend_Id", "dbo.Customers");
            DropForeignKey("dbo.Friendships", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.CouchsurfingOffers", "Owner_Id", "dbo.Customers");
            DropForeignKey("dbo.CouchsurfingOffers", "Client_Id", "dbo.Customers");
            DropForeignKey("dbo.Customers", "UserPreference_Id", "dbo.UserPreferences");
            DropForeignKey("dbo.LanguageInfoes", "UserPreference_Id", "dbo.UserPreferences");
            DropForeignKey("dbo.AdditionalFeatureInfoes", "UserPreference_Id", "dbo.UserPreferences");
            DropForeignKey("dbo.Customers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Friendships", new[] { "CustomerFriend_Id" });
            DropIndex("dbo.Friendships", new[] { "Customer_Id" });
            DropIndex("dbo.LanguageInfoes", new[] { "UserPreference_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Customers", new[] { "UserPreference_Id" });
            DropIndex("dbo.Customers", new[] { "User_Id" });
            DropIndex("dbo.CouchsurfingOffers", new[] { "Owner_Id" });
            DropIndex("dbo.CouchsurfingOffers", new[] { "Client_Id" });
            DropIndex("dbo.AdditionalFeatureInfoes", new[] { "UserPreference_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.HistoricalOffers");
            DropTable("dbo.Friendships");
            DropTable("dbo.LanguageInfoes");
            DropTable("dbo.UserPreferences");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Customers");
            DropTable("dbo.CouchsurfingOffers");
            DropTable("dbo.AdditionalFeatureInfoes");
        }
    }
}
