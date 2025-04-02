namespace synopcticsapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSinopticoTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SinopticoTests",
                c => new
                    {
                        SinopticoTestId = c.Int(nullable: false, identity: true),
                        Text1 = c.String(nullable: false, maxLength: 5),
                        Text2 = c.String(nullable: false, maxLength: 5),
                        Text3 = c.String(nullable: false, maxLength: 5),
                        Status = c.Int(nullable: false),
                        LastUpdate = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SinopticoTestId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SinopticoTests");
        }
    }
}
