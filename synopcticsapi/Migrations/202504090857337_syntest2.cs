namespace synopcticsapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class syntest2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlantModel",
                c => new
                    {
                        EquipmentId = c.Int(nullable: false, identity: true),
                        AreaId = c.Int(),
                        Level = c.Int(),
                        Parent = c.Int(),
                        EquipmentDescription = c.String(maxLength: 255),
                        EquipmentLongDescription = c.String(maxLength: 1024),
                        EquipmentPathDescription = c.String(maxLength: 1024),
                        SAPCode = c.String(maxLength: 50),
                        SAPEnabled = c.Boolean(),
                        IsManual = c.Boolean(),
                        IsSettable = c.Boolean(),
                        DisplayOrder = c.Int(),
                        LogicalOrder = c.Int(),
                        ParallelLogicalOrder = c.Int(),
                        FlgMonitor = c.Boolean(),
                        IPAddress = c.String(maxLength: 15),
                        LastUpdate = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
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
            
            CreateTable(
                "dbo.SynopticDatas",
                c => new
                    {
                        ElementId = c.String(nullable: false, maxLength: 50),
                        SynopticLayout = c.String(nullable: false, maxLength: 50),
                        Text1 = c.String(),
                        Text2 = c.String(),
                        Text3 = c.String(),
                        Status = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ElementId)
                .ForeignKey("dbo.SynopticLayouts", t => t.SynopticLayout, cascadeDelete: true)
                .Index(t => t.SynopticLayout);
            
            CreateTable(
                "dbo.SynopticLayouts",
                c => new
                    {
                        Layout = c.String(nullable: false, maxLength: 50),
                        AreaId = c.String(maxLength: 50),
                        Svg = c.String(),
                        SvgBackup = c.String(),
                    })
                .PrimaryKey(t => t.Layout);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SynopticDatas", "SynopticLayout", "dbo.SynopticLayouts");
            DropIndex("dbo.SynopticDatas", new[] { "SynopticLayout" });
            DropTable("dbo.SynopticLayouts");
            DropTable("dbo.SynopticDatas");
            DropTable("dbo.SinopticoTests");
            DropTable("dbo.PlantModel");
        }
    }
}
