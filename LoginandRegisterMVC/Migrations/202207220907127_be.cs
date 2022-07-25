namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class be : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Elections", "ADM", c => c.Boolean(nullable: false));
            AddColumn("dbo.Elections", "QEA", c => c.Boolean(nullable: false));
            AddColumn("dbo.Elections", "MDU", c => c.Boolean(nullable: false));
            AddColumn("dbo.Elections", "CSD", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Elections", "CSD");
            DropColumn("dbo.Elections", "MDU");
            DropColumn("dbo.Elections", "QEA");
            DropColumn("dbo.Elections", "ADM");
        }
    }
}
