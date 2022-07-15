namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        CandidateId = c.Int(nullable: false, identity: true),
                        ElectionId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Votes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CandidateId)
                .ForeignKey("dbo.Elections", t => t.ElectionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.ElectionId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Elections",
                c => new
                    {
                        ElectionId = c.Int(nullable: false, identity: true),
                        ElectionTitle = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ServiceLine = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ElectionId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false),
                        UserEmail = c.String(nullable: false),
                        Username = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        PhoneNo = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ServiceLine = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "EmployeeId", "dbo.Users");
            DropForeignKey("dbo.Candidates", "ElectionId", "dbo.Elections");
            DropIndex("dbo.Candidates", new[] { "EmployeeId" });
            DropIndex("dbo.Candidates", new[] { "ElectionId" });
            DropTable("dbo.Users");
            DropTable("dbo.Elections");
            DropTable("dbo.Candidates");
        }
    }
}
