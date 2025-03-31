namespace synopcticsapi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _double : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PlantModel", "AreaId", c => c.Int());
            AlterColumn("dbo.PlantModel", "Level", c => c.Int());
            AlterColumn("dbo.PlantModel", "EquipmentDescription", c => c.String(maxLength: 255));
            AlterColumn("dbo.PlantModel", "EquipmentLongDescription", c => c.String(maxLength: 1024));
            AlterColumn("dbo.PlantModel", "EquipmentPathDescription", c => c.String(maxLength: 1024));
            AlterColumn("dbo.PlantModel", "SAPCode", c => c.String(maxLength: 50));
            AlterColumn("dbo.PlantModel", "SAPEnabled", c => c.Boolean());
            AlterColumn("dbo.PlantModel", "IsManual", c => c.Boolean());
            AlterColumn("dbo.PlantModel", "IsSettable", c => c.Boolean());
            AlterColumn("dbo.PlantModel", "DisplayOrder", c => c.Int());
            AlterColumn("dbo.PlantModel", "LogicalOrder", c => c.Int());
            AlterColumn("dbo.PlantModel", "ParallelLogicalOrder", c => c.Int());
            AlterColumn("dbo.PlantModel", "FlgMonitor", c => c.Boolean());
            AlterColumn("dbo.PlantModel", "IPAddress", c => c.String(maxLength: 15));
            AlterColumn("dbo.PlantModel", "LastUpdate", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlantModel", "LastUpdate", c => c.String());
            AlterColumn("dbo.PlantModel", "IPAddress", c => c.String());
            AlterColumn("dbo.PlantModel", "FlgMonitor", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PlantModel", "ParallelLogicalOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.PlantModel", "LogicalOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.PlantModel", "DisplayOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.PlantModel", "IsSettable", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PlantModel", "IsManual", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PlantModel", "SAPEnabled", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PlantModel", "SAPCode", c => c.String());
            AlterColumn("dbo.PlantModel", "EquipmentPathDescription", c => c.String());
            AlterColumn("dbo.PlantModel", "EquipmentLongDescription", c => c.String());
            AlterColumn("dbo.PlantModel", "EquipmentDescription", c => c.String());
            AlterColumn("dbo.PlantModel", "Level", c => c.Int(nullable: false));
            AlterColumn("dbo.PlantModel", "AreaId", c => c.Int(nullable: false));
        }
    }
}
