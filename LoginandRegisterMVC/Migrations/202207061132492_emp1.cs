namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emp1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Candidates", "EmployeeId", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "EmployeeId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Users", "EmployeeId");
            AddForeignKey("dbo.Candidates", "EmployeeId", "dbo.Users", "EmployeeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidates", "EmployeeId", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "EmployeeId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "EmployeeId");
            AddForeignKey("dbo.Candidates", "EmployeeId", "dbo.Users", "EmployeeId", cascadeDelete: true);
        }
    }
}
