namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class primkey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "EmployeeId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "EmployeeId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "EmployeeId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "EmployeeId");
        }
    }
}
