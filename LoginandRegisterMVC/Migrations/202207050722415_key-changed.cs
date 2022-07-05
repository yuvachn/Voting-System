namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keychanged : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "UserEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "EmployeeId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "EmployeeId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "EmployeeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "UserEmail", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "UserEmail");
        }
    }
}
