namespace synopcticsapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlantModel",
                c => new
                    {
                        EquipmentId = c.Int(nullable: false, identity: true),
                        AreaId = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Parent = c.Int(),
                        EquipmentDescription = c.String(),
                        EquipmentLongDescription = c.String(),
                        EquipmentPathDescription = c.String(),
                        SAPCode = c.String(),
                        SAPEnabled = c.Boolean(nullable: false),
                        IsManual = c.Boolean(nullable: false),
                        IsSettable = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        LogicalOrder = c.Int(nullable: false),
                        ParallelLogicalOrder = c.Int(nullable: false),
                        FlgMonitor = c.Boolean(nullable: false),
                        IPAddress = c.String(),
                        LastUpdate = c.String(),
                    })
                .PrimaryKey(t => t.EquipmentId);
            
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
            DropTable("dbo.SynopticLayouts");
            DropTable("dbo.PlantModel");
        }
    }
}
