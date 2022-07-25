namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkbox : DbMigration
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
                    })
                .PrimaryKey(t => t.Value);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CheckBoxes");
        }
    }
}
