namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seli : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CheckBoxes", "Election_ElectionId", c => c.Int());
            CreateIndex("dbo.CheckBoxes", "Election_ElectionId");
            AddForeignKey("dbo.CheckBoxes", "Election_ElectionId", "dbo.Elections", "ElectionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckBoxes", "Election_ElectionId", "dbo.Elections");
            DropIndex("dbo.CheckBoxes", new[] { "Election_ElectionId" });
            DropColumn("dbo.CheckBoxes", "Election_ElectionId");
        }
    }
}
