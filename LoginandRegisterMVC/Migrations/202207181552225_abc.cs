namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VotedUsers",
                c => new
                    {
                        EmpId = c.Int(nullable: false, identity: true),
                        ElectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmpId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VotedUsers");
        }
    }
}
