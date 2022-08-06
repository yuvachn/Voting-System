namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abcd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CheckBoxes", "Election_ElectionId", "dbo.Elections");
            DropIndex("dbo.CheckBoxes", new[] { "Election_ElectionId" });
            DropTable("dbo.CheckBoxes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Value);
            
            CreateIndex("dbo.CheckBoxes", "Election_ElectionId");
            AddForeignKey("dbo.CheckBoxes", "Election_ElectionId", "dbo.Elections", "ElectionId");
        }
    }
}
