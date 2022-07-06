namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Candidate : DbMigration
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
                    })
                .PrimaryKey(t => t.CandidateId)
                .ForeignKey("dbo.Elections", t => t.ElectionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.ElectionId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "EmployeeId", "dbo.Users");
            DropForeignKey("dbo.Candidates", "ElectionId", "dbo.Elections");
            DropIndex("dbo.Candidates", new[] { "EmployeeId" });
            DropIndex("dbo.Candidates", new[] { "ElectionId" });
            DropTable("dbo.Candidates");
        }
    }
}
