namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elections",
                c => new
                    {
                        ElectionId = c.Int(nullable: false, identity: true),
                        ElectionTitle = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ServiceLine = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ElectionId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserEmail = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        PhoneNo = c.String(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Password = c.String(nullable: false),
                        ServiceLine = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserEmail);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Elections");
        }
    }
}
