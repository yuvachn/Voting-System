namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserEmail", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Users", "UserEmail", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "UserEmail" });
            AlterColumn("dbo.Users", "UserEmail", c => c.String(nullable: false));
        }
    }
}
