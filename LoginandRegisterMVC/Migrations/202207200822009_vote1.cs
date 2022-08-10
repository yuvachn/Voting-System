namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vote1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VotedUsers", "ElecId", c => c.Int(nullable: false));
            DropColumn("dbo.VotedUsers", "ElectionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VotedUsers", "ElectionId", c => c.Int(nullable: false));
            DropColumn("dbo.VotedUsers", "ElecId");
        }
    }
}
