namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pwd : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.VotedUsers");
            AlterColumn("dbo.VotedUsers", "EmpId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.VotedUsers", "EmpId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.VotedUsers");
            AlterColumn("dbo.VotedUsers", "EmpId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.VotedUsers", "EmpId");
        }
    }
}
