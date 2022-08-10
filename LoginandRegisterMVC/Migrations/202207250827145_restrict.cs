namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restrict : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckBoxes",
                c => new
                    {
                        Value = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        IsChecked = c.Boolean(nullable: false),
                        Election_ElectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Value)
                .ForeignKey("dbo.Elections", t => t.Election_ElectionId)
                .Index(t => t.Election_ElectionId);
            
            AddColumn("dbo.Elections", "ADM", c => c.Boolean(nullable: false));
            AddColumn("dbo.Elections", "QEA", c => c.Boolean(nullable: false));
            AddColumn("dbo.Elections", "MDU", c => c.Boolean(nullable: false));
            AddColumn("dbo.Elections", "CSD", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckBoxes", "Election_ElectionId", "dbo.Elections");
            DropIndex("dbo.CheckBoxes", new[] { "Election_ElectionId" });
            DropColumn("dbo.Elections", "CSD");
            DropColumn("dbo.Elections", "MDU");
            DropColumn("dbo.Elections", "QEA");
            DropColumn("dbo.Elections", "ADM");
            DropTable("dbo.CheckBoxes");
        }
    }
}
