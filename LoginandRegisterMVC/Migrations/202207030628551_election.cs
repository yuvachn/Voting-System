namespace LoginandRegisterMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class election : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Elections",
                c => new
                    {
                        ElectionId = c.String(nullable: false, maxLength: 128),
                        ElectionTitle = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ServiceLine = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ElectionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Elections");
        }
    }
}
