namespace WorkAutomatorDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.account",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(),
                        login = c.String(nullable: false, maxLength: 256, unicode: false),
                        password = c.String(nullable: false, maxLength: 256, unicode: false),
                        first_name = c.String(nullable: false, maxLength: 256),
                        last_name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id)
                .Index(t => t.login, unique: true);
            
            CreateTable(
                "dbo.task",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                        description = c.String(unicode: false, storeType: "text"),
                        company_id = c.Int(nullable: false),
                        assignee_account_id = c.Int(nullable: false),
                        reviewer_account_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .ForeignKey("dbo.account", t => t.assignee_account_id)
                .ForeignKey("dbo.account", t => t.reviewer_account_id)
                .Index(t => t.company_id)
                .Index(t => t.assignee_account_id)
                .Index(t => t.reviewer_account_id);
            
            CreateTable(
                "dbo.company",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 1024, unicode: false),
                        owner_id = c.Int(nullable: false),
                        plan_image_url = c.String(nullable: false, maxLength: 1024, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.account", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.company_plan_unique_point",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        x = c.Double(nullable: false),
                        y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.check_point",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_plan_unique_point1_id = c.Int(nullable: false),
                        company_plan_unique_point2_id = c.Int(nullable: false),
                        manufactory1_id = c.Int(nullable: false),
                        manufactory2_id = c.Int(nullable: false),
                        company_plan_unique_point1_id1 = c.Int(),
                        manufactory1_id1 = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company_plan_unique_point", t => t.company_plan_unique_point1_id1)
                .ForeignKey("dbo.manufactory", t => t.manufactory2_id)
                .ForeignKey("dbo.manufactory", t => t.manufactory1_id1)
                .ForeignKey("dbo.company_plan_unique_point", t => t.company_plan_unique_point2_id)
                .Index(t => t.company_plan_unique_point2_id)
                .Index(t => t.manufactory2_id)
                .Index(t => t.company_plan_unique_point1_id1)
                .Index(t => t.manufactory1_id1);
            
            CreateTable(
                "dbo.check_point_event",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        check_point_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        timespan = c.DateTime(nullable: false),
                        is_direct = c.Boolean(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.check_point", t => t.check_point_id)
                .ForeignKey("dbo.account", t => t.account_id)
                .Index(t => t.check_point_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.manufactory",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.manufactory_plan_point",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        manufactory_id = c.Int(nullable: false),
                        company_plan_unique_point_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.manufactory", t => t.manufactory_id)
                .ForeignKey("dbo.company_plan_unique_point", t => t.company_plan_unique_point_id)
                .Index(t => t.manufactory_id)
                .Index(t => t.company_plan_unique_point_id);
            
            CreateTable(
                "dbo.role",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.db_permission",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        table_name = c.String(nullable: false, maxLength: 256, unicode: false),
                        db_permission_type_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.db_permission_type", t => t.db_permission_type_id)
                .Index(t => t.db_permission_type_id);
            
            CreateTable(
                "dbo.db_permission_type",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.detector",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_prefab_id = c.Int(nullable: false),
                        pipeline_item_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_prefab", t => t.detector_prefab_id)
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item_id)
                .Index(t => t.detector_prefab_id)
                .Index(t => t.pipeline_item_id);
            
            CreateTable(
                "dbo.detector_data",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_id = c.Int(nullable: false),
                        detector_data_prefab_id = c.Int(nullable: false),
                        field_data_value_base64 = c.String(unicode: false, storeType: "text"),
                        timespan = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_data_prefab", t => t.detector_data_prefab_id)
                .ForeignKey("dbo.detector", t => t.detector_id)
                .Index(t => t.detector_id)
                .Index(t => t.detector_data_prefab_id);
            
            CreateTable(
                "dbo.detector_data_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_prefab_id = c.Int(nullable: false),
                        visualizer_type_id = c.Int(),
                        field_data_type_id = c.Int(nullable: false),
                        field_name = c.String(nullable: false, maxLength: 256, unicode: false),
                        field_description = c.String(unicode: false, storeType: "text"),
                        argument_name = c.String(maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.data_type", t => t.field_data_type_id)
                .ForeignKey("dbo.detector_prefab", t => t.detector_prefab_id)
                .ForeignKey("dbo.visualizer_type", t => t.visualizer_type_id)
                .Index(t => t.detector_prefab_id)
                .Index(t => t.visualizer_type_id)
                .Index(t => t.field_data_type_id);
            
            CreateTable(
                "dbo.data_type",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.detector_settings_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_prefab_id = c.Int(nullable: false),
                        option_data_type_id = c.Int(nullable: false),
                        option_name = c.String(nullable: false, maxLength: 256, unicode: false),
                        option_description = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_prefab", t => t.detector_prefab_id)
                .ForeignKey("dbo.data_type", t => t.option_data_type_id)
                .Index(t => t.detector_prefab_id)
                .Index(t => t.option_data_type_id);
            
            CreateTable(
                "dbo.detector_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                        image_url = c.String(nullable: false, maxLength: 1024, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.detector_fault_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_prefab_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                        fault_condition = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_prefab", t => t.detector_prefab_id)
                .Index(t => t.detector_prefab_id);
            
            CreateTable(
                "dbo.detector_fault",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_id = c.Int(nullable: false),
                        detector_fault_prefab_id = c.Int(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_fault_prefab", t => t.detector_fault_prefab_id)
                .ForeignKey("dbo.detector", t => t.detector_id)
                .Index(t => t.detector_id)
                .Index(t => t.detector_fault_prefab_id);
            
            CreateTable(
                "dbo.detector_fault_event",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_fault_id = c.Int(nullable: false),
                        timespan = c.DateTime(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_fault", t => t.detector_fault_id)
                .Index(t => t.detector_fault_id);
            
            CreateTable(
                "dbo.detector_settings_value",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_id = c.Int(nullable: false),
                        detector_settings_prefab_id = c.Int(nullable: false),
                        option_data_value_base64 = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector_settings_prefab", t => t.detector_settings_prefab_id)
                .ForeignKey("dbo.detector", t => t.detector_id)
                .Index(t => t.detector_id)
                .Index(t => t.detector_settings_prefab_id);
            
            CreateTable(
                "dbo.pipeline_item_settings_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item_prefab_id = c.Int(nullable: false),
                        option_data_type_id = c.Int(nullable: false),
                        option_name = c.String(nullable: false, maxLength: 256, unicode: false),
                        option_description = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pipeline_item_prefab", t => t.pipeline_item_prefab_id)
                .ForeignKey("dbo.data_type", t => t.option_data_type_id)
                .Index(t => t.pipeline_item_prefab_id)
                .Index(t => t.option_data_type_id);
            
            CreateTable(
                "dbo.pipeline_item_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 1024, unicode: false),
                        description = c.String(unicode: false, storeType: "text"),
                        image_url = c.String(nullable: false, maxLength: 1024, unicode: false),
                        input_x = c.Double(nullable: false),
                        input_y = c.Double(nullable: false),
                        output_x = c.Double(nullable: false),
                        output_y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.pipeline_item",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_id = c.Int(nullable: false),
                        pipeline_item_prefab_id = c.Int(nullable: false),
                        manufactory_id = c.Int(nullable: false),
                        x = c.Double(nullable: false),
                        y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pipeline", t => t.pipeline_id)
                .ForeignKey("dbo.pipeline_item_prefab", t => t.pipeline_item_prefab_id)
                .ForeignKey("dbo.manufactory", t => t.manufactory_id)
                .Index(t => t.pipeline_id)
                .Index(t => t.pipeline_item_prefab_id)
                .Index(t => t.manufactory_id);
            
            CreateTable(
                "dbo.pipeline",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.pipeline_item_connection",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item1_id = c.Int(nullable: false),
                        pipeline_item2_id = c.Int(nullable: false),
                        is_direct = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item1_id)
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item2_id)
                .Index(t => t.pipeline_item1_id)
                .Index(t => t.pipeline_item2_id);
            
            CreateTable(
                "dbo.pipeline_item_interaction_event",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        timespan = c.DateTime(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item_id)
                .ForeignKey("dbo.account", t => t.account_id)
                .Index(t => t.pipeline_item_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.pipeline_item_settings_value",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item_id = c.Int(nullable: false),
                        pipeline_item_settings_prefab_id = c.Int(nullable: false),
                        option_data_value_base64 = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item_id)
                .ForeignKey("dbo.pipeline_item_settings_prefab", t => t.pipeline_item_settings_prefab_id)
                .Index(t => t.pipeline_item_id)
                .Index(t => t.pipeline_item_settings_prefab_id);
            
            CreateTable(
                "dbo.pipeline_item_storage_connection",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        pipeline_item_id = c.Int(nullable: false),
                        storage_cell_id = c.Int(nullable: false),
                        is_direct = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.storage_cell", t => t.storage_cell_id)
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item_id)
                .Index(t => t.pipeline_item_id)
                .Index(t => t.storage_cell_id);
            
            CreateTable(
                "dbo.storage_cell",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        manufactory_id = c.Int(nullable: false),
                        storage_cell_prefab_id = c.Int(nullable: false),
                        x = c.Double(nullable: false),
                        y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.storage_cell_prefab", t => t.storage_cell_prefab_id)
                .ForeignKey("dbo.manufactory", t => t.manufactory_id)
                .Index(t => t.manufactory_id)
                .Index(t => t.storage_cell_prefab_id);
            
            CreateTable(
                "dbo.resource_storage_cell",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        resource_id = c.Int(nullable: false),
                        storage_cell_id = c.Int(nullable: false),
                        amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.resource", t => t.resource_id)
                .ForeignKey("dbo.storage_cell", t => t.storage_cell_id)
                .Index(t => t.resource_id)
                .Index(t => t.storage_cell_id);
            
            CreateTable(
                "dbo.resource",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        unit_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 1024, unicode: false),
                        description = c.String(unicode: false, storeType: "text"),
                        image_url = c.String(maxLength: 1024, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.unit", t => t.unit_id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id)
                .Index(t => t.unit_id);
            
            CreateTable(
                "dbo.unit",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                        short_name = c.String(nullable: false, maxLength: 16, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.storage_cell_event",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        storage_cell_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        timespan = c.DateTime(nullable: false),
                        amount = c.Double(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.storage_cell", t => t.storage_cell_id)
                .ForeignKey("dbo.account", t => t.account_id)
                .Index(t => t.storage_cell_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.storage_cell_prefab",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_id = c.Int(nullable: false),
                        image_url = c.String(nullable: false, maxLength: 1024, unicode: false),
                        input_x = c.Double(nullable: false),
                        input_y = c.Double(nullable: false),
                        output_x = c.Double(nullable: false),
                        output_y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company", t => t.company_id)
                .Index(t => t.company_id);
            
            CreateTable(
                "dbo.visualizer_type",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 256, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.detector_interaction_event",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        detector_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        timespan = c.DateTime(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.detector", t => t.detector_id)
                .ForeignKey("dbo.account", t => t.account_id)
                .Index(t => t.detector_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.enter_leave_point",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        company_plan_unique_point1_id = c.Int(nullable: false),
                        company_plan_unique_point2_id = c.Int(nullable: false),
                        company_plan_unique_point1_id1 = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.company_plan_unique_point", t => t.company_plan_unique_point1_id1)
                .ForeignKey("dbo.company_plan_unique_point", t => t.company_plan_unique_point2_id)
                .Index(t => t.company_plan_unique_point2_id)
                .Index(t => t.company_plan_unique_point1_id1);
            
            CreateTable(
                "dbo.enter_leave_point_event",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        enter_leave_point_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        timespan = c.DateTime(nullable: false),
                        is_enter = c.Boolean(nullable: false),
                        log = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.enter_leave_point", t => t.enter_leave_point_id)
                .ForeignKey("dbo.account", t => t.account_id)
                .Index(t => t.enter_leave_point_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.role_db_permission",
                c => new
                    {
                        db_permission_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.db_permission_id, t.role_id })
                .ForeignKey("dbo.db_permission", t => t.db_permission_id, cascadeDelete: true)
                .ForeignKey("dbo.role", t => t.role_id, cascadeDelete: true)
                .Index(t => t.db_permission_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.role_pipeline_item_permission",
                c => new
                    {
                        pipeline_item_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.pipeline_item_id, t.role_id })
                .ForeignKey("dbo.pipeline_item", t => t.pipeline_item_id, cascadeDelete: true)
                .ForeignKey("dbo.role", t => t.role_id, cascadeDelete: true)
                .Index(t => t.pipeline_item_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.role_storage_cell_permission",
                c => new
                    {
                        storage_cell_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.storage_cell_id, t.role_id })
                .ForeignKey("dbo.storage_cell", t => t.storage_cell_id, cascadeDelete: true)
                .ForeignKey("dbo.role", t => t.role_id, cascadeDelete: true)
                .Index(t => t.storage_cell_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.role_detector_permission",
                c => new
                    {
                        detector_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.detector_id, t.role_id })
                .ForeignKey("dbo.detector", t => t.detector_id, cascadeDelete: true)
                .ForeignKey("dbo.role", t => t.role_id, cascadeDelete: true)
                .Index(t => t.detector_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.role_manufactory_permission",
                c => new
                    {
                        manufactory_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.manufactory_id, t.role_id })
                .ForeignKey("dbo.manufactory", t => t.manufactory_id, cascadeDelete: true)
                .ForeignKey("dbo.role", t => t.role_id, cascadeDelete: true)
                .Index(t => t.manufactory_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.account_bosses_subs",
                c => new
                    {
                        sub_account_id = c.Int(nullable: false),
                        boss_account_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.sub_account_id, t.boss_account_id })
                .ForeignKey("dbo.account", t => t.sub_account_id)
                .ForeignKey("dbo.account", t => t.boss_account_id)
                .Index(t => t.sub_account_id)
                .Index(t => t.boss_account_id);
            
            CreateTable(
                "dbo.account_role",
                c => new
                    {
                        account_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.account_id, t.role_id })
                .ForeignKey("dbo.account", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.role", t => t.role_id, cascadeDelete: true)
                .Index(t => t.account_id)
                .Index(t => t.role_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.task", "reviewer_account_id", "dbo.account");
            DropForeignKey("dbo.storage_cell_event", "account_id", "dbo.account");
            DropForeignKey("dbo.account_role", "role_id", "dbo.role");
            DropForeignKey("dbo.account_role", "account_id", "dbo.account");
            DropForeignKey("dbo.pipeline_item_interaction_event", "account_id", "dbo.account");
            DropForeignKey("dbo.company", "id", "dbo.account");
            DropForeignKey("dbo.enter_leave_point_event", "account_id", "dbo.account");
            DropForeignKey("dbo.detector_interaction_event", "account_id", "dbo.account");
            DropForeignKey("dbo.check_point_event", "account_id", "dbo.account");
            DropForeignKey("dbo.account_bosses_subs", "boss_account_id", "dbo.account");
            DropForeignKey("dbo.account_bosses_subs", "sub_account_id", "dbo.account");
            DropForeignKey("dbo.task", "assignee_account_id", "dbo.account");
            DropForeignKey("dbo.unit", "company_id", "dbo.company");
            DropForeignKey("dbo.task", "company_id", "dbo.company");
            DropForeignKey("dbo.storage_cell_prefab", "company_id", "dbo.company");
            DropForeignKey("dbo.role", "company_id", "dbo.company");
            DropForeignKey("dbo.resource", "company_id", "dbo.company");
            DropForeignKey("dbo.pipeline", "company_id", "dbo.company");
            DropForeignKey("dbo.pipeline_item_prefab", "company_id", "dbo.company");
            DropForeignKey("dbo.account", "company_id", "dbo.company");
            DropForeignKey("dbo.manufactory", "company_id", "dbo.company");
            DropForeignKey("dbo.detector_prefab", "company_id", "dbo.company");
            DropForeignKey("dbo.company_plan_unique_point", "company_id", "dbo.company");
            DropForeignKey("dbo.manufactory_plan_point", "company_plan_unique_point_id", "dbo.company_plan_unique_point");
            DropForeignKey("dbo.enter_leave_point", "company_plan_unique_point2_id", "dbo.company_plan_unique_point");
            DropForeignKey("dbo.enter_leave_point_event", "enter_leave_point_id", "dbo.enter_leave_point");
            DropForeignKey("dbo.enter_leave_point", "company_plan_unique_point1_id1", "dbo.company_plan_unique_point");
            DropForeignKey("dbo.check_point", "company_plan_unique_point2_id", "dbo.company_plan_unique_point");
            DropForeignKey("dbo.check_point", "manufactory1_id1", "dbo.manufactory");
            DropForeignKey("dbo.storage_cell", "manufactory_id", "dbo.manufactory");
            DropForeignKey("dbo.pipeline_item", "manufactory_id", "dbo.manufactory");
            DropForeignKey("dbo.role_manufactory_permission", "role_id", "dbo.role");
            DropForeignKey("dbo.role_manufactory_permission", "manufactory_id", "dbo.manufactory");
            DropForeignKey("dbo.role_detector_permission", "role_id", "dbo.role");
            DropForeignKey("dbo.role_detector_permission", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_settings_value", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_interaction_event", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_fault", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_data", "detector_id", "dbo.detector");
            DropForeignKey("dbo.detector_data_prefab", "visualizer_type_id", "dbo.visualizer_type");
            DropForeignKey("dbo.detector_data", "detector_data_prefab_id", "dbo.detector_data_prefab");
            DropForeignKey("dbo.pipeline_item_settings_prefab", "option_data_type_id", "dbo.data_type");
            DropForeignKey("dbo.pipeline_item_settings_value", "pipeline_item_settings_prefab_id", "dbo.pipeline_item_settings_prefab");
            DropForeignKey("dbo.pipeline_item_settings_prefab", "pipeline_item_prefab_id", "dbo.pipeline_item_prefab");
            DropForeignKey("dbo.pipeline_item", "pipeline_item_prefab_id", "dbo.pipeline_item_prefab");
            DropForeignKey("dbo.pipeline_item_storage_connection", "pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.storage_cell", "storage_cell_prefab_id", "dbo.storage_cell_prefab");
            DropForeignKey("dbo.storage_cell_event", "storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.resource_storage_cell", "storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.resource", "unit_id", "dbo.unit");
            DropForeignKey("dbo.resource_storage_cell", "resource_id", "dbo.resource");
            DropForeignKey("dbo.pipeline_item_storage_connection", "storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.role_storage_cell_permission", "role_id", "dbo.role");
            DropForeignKey("dbo.role_storage_cell_permission", "storage_cell_id", "dbo.storage_cell");
            DropForeignKey("dbo.pipeline_item_settings_value", "pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_interaction_event", "pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_connection", "pipeline_item2_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item_connection", "pipeline_item1_id", "dbo.pipeline_item");
            DropForeignKey("dbo.pipeline_item", "pipeline_id", "dbo.pipeline");
            DropForeignKey("dbo.role_pipeline_item_permission", "role_id", "dbo.role");
            DropForeignKey("dbo.role_pipeline_item_permission", "pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.detector", "pipeline_item_id", "dbo.pipeline_item");
            DropForeignKey("dbo.detector_settings_prefab", "option_data_type_id", "dbo.data_type");
            DropForeignKey("dbo.detector_settings_value", "detector_settings_prefab_id", "dbo.detector_settings_prefab");
            DropForeignKey("dbo.detector_settings_prefab", "detector_prefab_id", "dbo.detector_prefab");
            DropForeignKey("dbo.detector_fault_prefab", "detector_prefab_id", "dbo.detector_prefab");
            DropForeignKey("dbo.detector_fault", "detector_fault_prefab_id", "dbo.detector_fault_prefab");
            DropForeignKey("dbo.detector_fault_event", "detector_fault_id", "dbo.detector_fault");
            DropForeignKey("dbo.detector_data_prefab", "detector_prefab_id", "dbo.detector_prefab");
            DropForeignKey("dbo.detector", "detector_prefab_id", "dbo.detector_prefab");
            DropForeignKey("dbo.detector_data_prefab", "field_data_type_id", "dbo.data_type");
            DropForeignKey("dbo.role_db_permission", "role_id", "dbo.role");
            DropForeignKey("dbo.role_db_permission", "db_permission_id", "dbo.db_permission");
            DropForeignKey("dbo.db_permission", "db_permission_type_id", "dbo.db_permission_type");
            DropForeignKey("dbo.manufactory_plan_point", "manufactory_id", "dbo.manufactory");
            DropForeignKey("dbo.check_point", "manufactory2_id", "dbo.manufactory");
            DropForeignKey("dbo.check_point", "company_plan_unique_point1_id1", "dbo.company_plan_unique_point");
            DropForeignKey("dbo.check_point_event", "check_point_id", "dbo.check_point");
            DropIndex("dbo.account_role", new[] { "role_id" });
            DropIndex("dbo.account_role", new[] { "account_id" });
            DropIndex("dbo.account_bosses_subs", new[] { "boss_account_id" });
            DropIndex("dbo.account_bosses_subs", new[] { "sub_account_id" });
            DropIndex("dbo.role_manufactory_permission", new[] { "role_id" });
            DropIndex("dbo.role_manufactory_permission", new[] { "manufactory_id" });
            DropIndex("dbo.role_detector_permission", new[] { "role_id" });
            DropIndex("dbo.role_detector_permission", new[] { "detector_id" });
            DropIndex("dbo.role_storage_cell_permission", new[] { "role_id" });
            DropIndex("dbo.role_storage_cell_permission", new[] { "storage_cell_id" });
            DropIndex("dbo.role_pipeline_item_permission", new[] { "role_id" });
            DropIndex("dbo.role_pipeline_item_permission", new[] { "pipeline_item_id" });
            DropIndex("dbo.role_db_permission", new[] { "role_id" });
            DropIndex("dbo.role_db_permission", new[] { "db_permission_id" });
            DropIndex("dbo.enter_leave_point_event", new[] { "account_id" });
            DropIndex("dbo.enter_leave_point_event", new[] { "enter_leave_point_id" });
            DropIndex("dbo.enter_leave_point", new[] { "company_plan_unique_point1_id1" });
            DropIndex("dbo.enter_leave_point", new[] { "company_plan_unique_point2_id" });
            DropIndex("dbo.detector_interaction_event", new[] { "account_id" });
            DropIndex("dbo.detector_interaction_event", new[] { "detector_id" });
            DropIndex("dbo.storage_cell_prefab", new[] { "company_id" });
            DropIndex("dbo.storage_cell_event", new[] { "account_id" });
            DropIndex("dbo.storage_cell_event", new[] { "storage_cell_id" });
            DropIndex("dbo.unit", new[] { "company_id" });
            DropIndex("dbo.resource", new[] { "unit_id" });
            DropIndex("dbo.resource", new[] { "company_id" });
            DropIndex("dbo.resource_storage_cell", new[] { "storage_cell_id" });
            DropIndex("dbo.resource_storage_cell", new[] { "resource_id" });
            DropIndex("dbo.storage_cell", new[] { "storage_cell_prefab_id" });
            DropIndex("dbo.storage_cell", new[] { "manufactory_id" });
            DropIndex("dbo.pipeline_item_storage_connection", new[] { "storage_cell_id" });
            DropIndex("dbo.pipeline_item_storage_connection", new[] { "pipeline_item_id" });
            DropIndex("dbo.pipeline_item_settings_value", new[] { "pipeline_item_settings_prefab_id" });
            DropIndex("dbo.pipeline_item_settings_value", new[] { "pipeline_item_id" });
            DropIndex("dbo.pipeline_item_interaction_event", new[] { "account_id" });
            DropIndex("dbo.pipeline_item_interaction_event", new[] { "pipeline_item_id" });
            DropIndex("dbo.pipeline_item_connection", new[] { "pipeline_item2_id" });
            DropIndex("dbo.pipeline_item_connection", new[] { "pipeline_item1_id" });
            DropIndex("dbo.pipeline", new[] { "company_id" });
            DropIndex("dbo.pipeline_item", new[] { "manufactory_id" });
            DropIndex("dbo.pipeline_item", new[] { "pipeline_item_prefab_id" });
            DropIndex("dbo.pipeline_item", new[] { "pipeline_id" });
            DropIndex("dbo.pipeline_item_prefab", new[] { "company_id" });
            DropIndex("dbo.pipeline_item_settings_prefab", new[] { "option_data_type_id" });
            DropIndex("dbo.pipeline_item_settings_prefab", new[] { "pipeline_item_prefab_id" });
            DropIndex("dbo.detector_settings_value", new[] { "detector_settings_prefab_id" });
            DropIndex("dbo.detector_settings_value", new[] { "detector_id" });
            DropIndex("dbo.detector_fault_event", new[] { "detector_fault_id" });
            DropIndex("dbo.detector_fault", new[] { "detector_fault_prefab_id" });
            DropIndex("dbo.detector_fault", new[] { "detector_id" });
            DropIndex("dbo.detector_fault_prefab", new[] { "detector_prefab_id" });
            DropIndex("dbo.detector_prefab", new[] { "company_id" });
            DropIndex("dbo.detector_settings_prefab", new[] { "option_data_type_id" });
            DropIndex("dbo.detector_settings_prefab", new[] { "detector_prefab_id" });
            DropIndex("dbo.detector_data_prefab", new[] { "field_data_type_id" });
            DropIndex("dbo.detector_data_prefab", new[] { "visualizer_type_id" });
            DropIndex("dbo.detector_data_prefab", new[] { "detector_prefab_id" });
            DropIndex("dbo.detector_data", new[] { "detector_data_prefab_id" });
            DropIndex("dbo.detector_data", new[] { "detector_id" });
            DropIndex("dbo.detector", new[] { "pipeline_item_id" });
            DropIndex("dbo.detector", new[] { "detector_prefab_id" });
            DropIndex("dbo.db_permission", new[] { "db_permission_type_id" });
            DropIndex("dbo.role", new[] { "company_id" });
            DropIndex("dbo.manufactory_plan_point", new[] { "company_plan_unique_point_id" });
            DropIndex("dbo.manufactory_plan_point", new[] { "manufactory_id" });
            DropIndex("dbo.manufactory", new[] { "company_id" });
            DropIndex("dbo.check_point_event", new[] { "account_id" });
            DropIndex("dbo.check_point_event", new[] { "check_point_id" });
            DropIndex("dbo.check_point", new[] { "manufactory1_id1" });
            DropIndex("dbo.check_point", new[] { "company_plan_unique_point1_id1" });
            DropIndex("dbo.check_point", new[] { "manufactory2_id" });
            DropIndex("dbo.check_point", new[] { "company_plan_unique_point2_id" });
            DropIndex("dbo.company_plan_unique_point", new[] { "company_id" });
            DropIndex("dbo.company", new[] { "id" });
            DropIndex("dbo.task", new[] { "reviewer_account_id" });
            DropIndex("dbo.task", new[] { "assignee_account_id" });
            DropIndex("dbo.task", new[] { "company_id" });
            DropIndex("dbo.account", new[] { "login" });
            DropIndex("dbo.account", new[] { "company_id" });
            DropTable("dbo.account_role");
            DropTable("dbo.account_bosses_subs");
            DropTable("dbo.role_manufactory_permission");
            DropTable("dbo.role_detector_permission");
            DropTable("dbo.role_storage_cell_permission");
            DropTable("dbo.role_pipeline_item_permission");
            DropTable("dbo.role_db_permission");
            DropTable("dbo.enter_leave_point_event");
            DropTable("dbo.enter_leave_point");
            DropTable("dbo.detector_interaction_event");
            DropTable("dbo.visualizer_type");
            DropTable("dbo.storage_cell_prefab");
            DropTable("dbo.storage_cell_event");
            DropTable("dbo.unit");
            DropTable("dbo.resource");
            DropTable("dbo.resource_storage_cell");
            DropTable("dbo.storage_cell");
            DropTable("dbo.pipeline_item_storage_connection");
            DropTable("dbo.pipeline_item_settings_value");
            DropTable("dbo.pipeline_item_interaction_event");
            DropTable("dbo.pipeline_item_connection");
            DropTable("dbo.pipeline");
            DropTable("dbo.pipeline_item");
            DropTable("dbo.pipeline_item_prefab");
            DropTable("dbo.pipeline_item_settings_prefab");
            DropTable("dbo.detector_settings_value");
            DropTable("dbo.detector_fault_event");
            DropTable("dbo.detector_fault");
            DropTable("dbo.detector_fault_prefab");
            DropTable("dbo.detector_prefab");
            DropTable("dbo.detector_settings_prefab");
            DropTable("dbo.data_type");
            DropTable("dbo.detector_data_prefab");
            DropTable("dbo.detector_data");
            DropTable("dbo.detector");
            DropTable("dbo.db_permission_type");
            DropTable("dbo.db_permission");
            DropTable("dbo.role");
            DropTable("dbo.manufactory_plan_point");
            DropTable("dbo.manufactory");
            DropTable("dbo.check_point_event");
            DropTable("dbo.check_point");
            DropTable("dbo.company_plan_unique_point");
            DropTable("dbo.company");
            DropTable("dbo.task");
            DropTable("dbo.account");
        }
    }
}
