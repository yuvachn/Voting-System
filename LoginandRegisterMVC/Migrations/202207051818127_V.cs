namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Votes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "Votes");
        }
    }
}
