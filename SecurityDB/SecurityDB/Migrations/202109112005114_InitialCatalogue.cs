namespace SecurityDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCatalogue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        WebForms = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Hoursno = c.Int(nullable: false),
                        MinutesNo = c.Int(),
                        SiteId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Desc = c.String(maxLength: 400),
                        IsApproved = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false, maxLength: 150),
                        SiteAddress = c.String(nullable: false, maxLength: 350),
                        ContactPerson = c.String(maxLength: 150),
                        Telephone = c.String(maxLength: 100),
                        Mobile = c.String(maxLength: 100),
                        Email = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JoiningDate = c.DateTime(),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        SIAno = c.String(),
                        SIAType = c.String(),
                        SIAExpiryDate = c.DateTime(),
                        Email = c.String(nullable: false, maxLength: 150),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 350),
                        Mobile = c.String(maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Shifts", "SiteId", "dbo.Sites");
            DropIndex("dbo.Shifts", new[] { "UserId" });
            DropIndex("dbo.Shifts", new[] { "SiteId" });
            DropTable("dbo.Users");
            DropTable("dbo.Sites");
            DropTable("dbo.Shifts");
            DropTable("dbo.Roles");
        }
    }
}
