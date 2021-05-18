using System.Data.Entity;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess
{
    internal class WorkAutomatorDBContext : DbContext
    {
        public WorkAutomatorDBContext() : base("WorkAutomatorDBContext")
        {

        }

        public virtual DbSet<AccountEntity> Account { get; set; }
        public virtual DbSet<CheckPointEntity> CheckPoint { get; set; }
        public virtual DbSet<CheckPointEventEntity> CheckPointEvent { get; set; }
        public virtual DbSet<CompanyEntity> Company { get; set; }
        public virtual DbSet<CompanyPlanUniquePointEntity> CompanyPlanUniquePoint { get; set; }
        public virtual DbSet<DataTypeEntity> DataType { get; set; }
        public virtual DbSet<DbPermissionEntity> DbPermission { get; set; }
        public virtual DbSet<DbPermissionTypeEntity> DbPermissionType { get; set; }
        public virtual DbSet<DetectorEntity> Detector { get; set; }
        public virtual DbSet<DetectorDataEntity> DetectorData { get; set; }
        public virtual DbSet<DetectorDataPrefabEntity> DetectorDataPrefab { get; set; }
        public virtual DbSet<DetectorFaultEntity> DetectorFault { get; set; }
        public virtual DbSet<DetectorFaultEventEntity> DetectorFaultEvent { get; set; }
        public virtual DbSet<DetectorFaultPrefabEntity> DetectorFaultPrefab { get; set; }
        public virtual DbSet<DetectorInteractionEventEntity> DetectorInteractionEvent { get; set; }
        public virtual DbSet<DetectorPrefabEntity> DetectorPrefab { get; set; }
        public virtual DbSet<DetectorSettingsPrefabEntity> DetectorSettingsPrefab { get; set; }
        public virtual DbSet<DetectorSettingsValueEntity> DetectorSettingsValue { get; set; }
        public virtual DbSet<EnterLeavePointEntity> EnterLeavePoint { get; set; }
        public virtual DbSet<EnterLeavePointEventEntity> EnterLeavePointEvent { get; set; }
        public virtual DbSet<ManufactoryEntity> Manufactory { get; set; }
        public virtual DbSet<ManufactoryPlanPointEntity> ManufactoryPlanPoint { get; set; }
        public virtual DbSet<PipelineEntity> Pipeline { get; set; }
        public virtual DbSet<PipelineItemEntity> PipelineItem { get; set; }
        public virtual DbSet<PipelineItemConnectionEntity> PipelineItemConnection { get; set; }
        public virtual DbSet<PipelineItemInteractionEventEntity> PipelineItemInteractionEvent { get; set; }
        public virtual DbSet<PipelineItemPrefabEntity> PipelineItemPrefab { get; set; }
        public virtual DbSet<PipelineItemSettingsPrefabEntity> PipelineItemSettingsPrefab { get; set; }
        public virtual DbSet<PipelineItemSettingsValueEntity> PipelineItemSettingsValue { get; set; }
        public virtual DbSet<PipelineItemStorageConnectionEntity> PipelineItemStorageConnection { get; set; }
        public virtual DbSet<ResourceEntity> Resource { get; set; }
        public virtual DbSet<ResourceStorageCellEntity> ResourceStorageCell { get; set; }
        public virtual DbSet<RoleEntity> Role { get; set; }
        public virtual DbSet<StorageCellEntity> StorageCell { get; set; }
        public virtual DbSet<StorageCellEventEntity> StorageCellEvent { get; set; }
        public virtual DbSet<StorageCellPrefabEntity> StorageCellPrefab { get; set; }
        public virtual DbSet<TaskEntity> Task { get; set; }
        public virtual DbSet<UnitEntity> Unit { get; set; }
        public virtual DbSet<VisualizerTypeEntity> VisualizerType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>()
                .ToTable("account");

            modelBuilder.Entity<CheckPointEntity>()
                .ToTable("check_point");

            modelBuilder.Entity<CheckPointEventEntity>()
                .ToTable("check_point_event");

            modelBuilder.Entity<CompanyEntity>()
                .ToTable("company");

            modelBuilder.Entity<CompanyPlanUniquePointEntity>()
                .ToTable("company_plan_unique_point");

            modelBuilder.Entity<DataTypeEntity>()
                .ToTable("data_type");

            modelBuilder.Entity<DbPermissionEntity>()
                .ToTable("db_permission");

            modelBuilder.Entity<DbPermissionTypeEntity>()
                .ToTable("db_permission_type");

            modelBuilder.Entity<DetectorDataEntity>()
                .ToTable("detector_data");

            modelBuilder.Entity<DetectorDataPrefabEntity>()
                .ToTable("detector_data_prefab");

            modelBuilder.Entity<DetectorEntity>()
                .ToTable("detector");

            modelBuilder.Entity<DetectorFaultEntity>()
                .ToTable("detector_fault");

            modelBuilder.Entity<DetectorFaultEventEntity>()
                .ToTable("detector_fault_event");

            modelBuilder.Entity<DetectorFaultPrefabEntity>()
                .ToTable("detector_fault_prefab");

            modelBuilder.Entity<DetectorInteractionEventEntity>()
                .ToTable("detector_interaction_event");

            modelBuilder.Entity<DetectorPrefabEntity>()
                .ToTable("detector_prefab");

            modelBuilder.Entity<DetectorSettingsPrefabEntity>()
                .ToTable("detector_settings_prefab");

            modelBuilder.Entity<DetectorSettingsValueEntity>()
                .ToTable("detector_settings_value");

            modelBuilder.Entity<EnterLeavePointEntity>()
                .ToTable("enter_leave_point");

            modelBuilder.Entity<EnterLeavePointEventEntity>()
                .ToTable("enter_leave_point_event");

            modelBuilder.Entity<ManufactoryEntity>()
                .ToTable("manufactory");

            modelBuilder.Entity<ManufactoryPlanPointEntity>()
                .ToTable("manufactory_plan_point");

            modelBuilder.Entity<PipelineEntity>()
                .ToTable("pipeline");

            modelBuilder.Entity<PipelineItemConnectionEntity>()
                .ToTable("pipeline_item_connection");

            modelBuilder.Entity<PipelineItemEntity>()
                .ToTable("pipeline_item");

            modelBuilder.Entity<PipelineItemInteractionEventEntity>()
                .ToTable("pipeline_item_interaction_event");

            modelBuilder.Entity<PipelineItemPrefabEntity>()
                .ToTable("pipeline_item_prefab");

            modelBuilder.Entity<PipelineItemSettingsPrefabEntity>()
                .ToTable("pipeline_item_settings_prefab");

            modelBuilder.Entity<PipelineItemSettingsValueEntity>()
                .ToTable("pipeline_item_settings_value");

            modelBuilder.Entity<PipelineItemStorageConnectionEntity>()
                .ToTable("pipeline_item_storage_connection");

            modelBuilder.Entity<ResourceEntity>()
                .ToTable("resource");

            modelBuilder.Entity<ResourceStorageCellEntity>()
                .ToTable("resource_storage_cell");

            modelBuilder.Entity<RoleEntity>()
                .ToTable("role");

            modelBuilder.Entity<StorageCellEntity>()
                .ToTable("storage_cell");

            modelBuilder.Entity<StorageCellEventEntity>()
                .ToTable("storage_cell_event");

            modelBuilder.Entity<StorageCellPrefabEntity>()
                .ToTable("storage_cell_prefab");

            modelBuilder.Entity<TaskEntity>()
                .ToTable("task");

            modelBuilder.Entity<UnitEntity>()
                .ToTable("unit");

            modelBuilder.Entity<VisualizerTypeEntity>()
                .ToTable("visualizer_type");

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Accounts)
                .Map(cs => {
                    cs.MapLeftKey("account_id");
                    cs.MapRightKey("role_id");
                    cs.ToTable("account_role");
                });

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.Bosses)
                .WithMany(e => e.Subs)
                .Map(cs =>
                {
                    cs.MapLeftKey("sub_account_id");
                    cs.MapRightKey("boss_account_id");
                    cs.ToTable("account_bosses_subs");
                });

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.check_point_event)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasOptional(e => e.OwnedCompany)
                .WithRequired(e => e.Owner);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.detector_interaction_event)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.enter_leave_point_event)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.pipeline_item_interaction_event)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.storage_cell_event)
                .WithRequired(e => e.account)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.AssignedTasks)
                .WithRequired(e => e.Assignee)
                .HasForeignKey(e => e.assignee_account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.TasksToReview)
                .WithRequired(e => e.Reviewer)
                .HasForeignKey(e => e.reviewer_account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckPointEntity>()
                .HasMany(e => e.check_point_event)
                .WithRequired(e => e.check_point)
                .HasForeignKey(e => e.check_point_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckPointEventEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyEntity>()
                .Property(e => e.plan_image_url)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.account)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.company_plan_unique_point)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.detector_prefab)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.manufactory)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.pipeline_item_prefab)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.pipeline)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.resource)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.role)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.storage_cell_prefab)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.task)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(e => e.unit)
                .WithRequired(e => e.company)
                .HasForeignKey(e => e.company_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyPlanUniquePointEntity>()
                .HasMany(e => e.check_point)
                .WithRequired(e => e.company_plan_unique_point1)
                .HasForeignKey(e => e.company_plan_unique_point1_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyPlanUniquePointEntity>()
                .HasMany(e => e.check_point1)
                .WithRequired(e => e.company_plan_unique_point2)
                .HasForeignKey(e => e.company_plan_unique_point2_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyPlanUniquePointEntity>()
                .HasMany(e => e.enter_leave_point)
                .WithRequired(e => e.company_plan_unique_point1)
                .HasForeignKey(e => e.company_plan_unique_point1_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyPlanUniquePointEntity>()
                .HasMany(e => e.enter_leave_point1)
                .WithRequired(e => e.company_plan_unique_point2)
                .HasForeignKey(e => e.company_plan_unique_point2_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyPlanUniquePointEntity>()
                .HasMany(e => e.manufactory_plan_point)
                .WithRequired(e => e.company_plan_unique_point)
                .HasForeignKey(e => e.company_plan_unique_point_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DataTypeEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<DataTypeEntity>()
                .HasMany(e => e.detector_data_prefab)
                .WithRequired(e => e.data_type)
                .HasForeignKey(e => e.field_data_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DataTypeEntity>()
                .HasMany(e => e.detector_settings_prefab)
                .WithRequired(e => e.data_type)
                .HasForeignKey(e => e.option_data_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DataTypeEntity>()
                .HasMany(e => e.pipeline_item_settings_prefab)
                .WithRequired(e => e.data_type)
                .HasForeignKey(e => e.option_data_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DbPermissionEntity>()
                .Property(e => e.table_name)
                .IsUnicode(false);

            modelBuilder.Entity<DbPermissionEntity>()
                .HasMany(e => e.Granted)
                .WithMany(e => e.DbPermissions)
                .Map(cs => {
                    cs.MapLeftKey("db_permission_id");
                    cs.MapRightKey("role_id");
                    cs.ToTable("role_db_permission");
                });

            modelBuilder.Entity<DbPermissionTypeEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<DbPermissionTypeEntity>()
                .HasMany(e => e.db_permission)
                .WithRequired(e => e.db_permission_type)
                .HasForeignKey(e => e.db_permission_type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorEntity>()
                .HasMany(e => e.detector_interaction_event)
                .WithRequired(e => e.detector)
                .HasForeignKey(e => e.detector_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorEntity>()
                .HasMany(e => e.detector_fault)
                .WithRequired(e => e.detector)
                .HasForeignKey(e => e.detector_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorEntity>()
                .HasMany(e => e.detector_data)
                .WithRequired(e => e.detector)
                .HasForeignKey(e => e.detector_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorEntity>()
                .HasMany(e => e.detector_settings_value)
                .WithRequired(e => e.detector)
                .HasForeignKey(e => e.detector_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorEntity>()
                .HasMany(e => e.PermissionsGranted)
                .WithMany(e => e.DetectorPermissions)
                .Map(cs =>
                {
                    cs.MapLeftKey("detector_id");
                    cs.MapRightKey("role_id");
                    cs.ToTable("role_detector_permission");
                });

            modelBuilder.Entity<DetectorDataEntity>()
                .Property(e => e.field_data_value_base64)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorDataPrefabEntity>()
                .Property(e => e.field_name)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorDataPrefabEntity>()
                .Property(e => e.field_description)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorDataPrefabEntity>()
                .Property(e => e.argument_name)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorDataPrefabEntity>()
                .HasMany(e => e.detector_data)
                .WithRequired(e => e.detector_data_prefab)
                .HasForeignKey(e => e.detector_data_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorFaultEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorFaultEntity>()
                .HasMany(e => e.detector_fault_events)
                .WithRequired(e => e.detector_fault)
                .HasForeignKey(e => e.detector_fault_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorFaultEventEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorFaultPrefabEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorFaultPrefabEntity>()
                .Property(e => e.fault_condition)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorFaultPrefabEntity>()
                .HasMany(e => e.detector_fault)
                .WithRequired(e => e.detector_fault_prefab)
                .HasForeignKey(e => e.detector_fault_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorInteractionEventEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorPrefabEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorPrefabEntity>()
                .Property(e => e.image_url)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorPrefabEntity>()
                .HasMany(e => e.detector)
                .WithRequired(e => e.detector_prefab)
                .HasForeignKey(e => e.detector_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorPrefabEntity>()
                .HasMany(e => e.detector_data_prefab)
                .WithRequired(e => e.detector_prefab)
                .HasForeignKey(e => e.detector_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorPrefabEntity>()
                .HasMany(e => e.detector_fault_prefab)
                .WithRequired(e => e.detector_prefab)
                .HasForeignKey(e => e.detector_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorPrefabEntity>()
                .HasMany(e => e.detector_settings_prefab)
                .WithRequired(e => e.detector_prefab)
                .HasForeignKey(e => e.detector_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorSettingsPrefabEntity>()
                .Property(e => e.option_name)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorSettingsPrefabEntity>()
                .Property(e => e.option_description)
                .IsUnicode(false);

            modelBuilder.Entity<DetectorSettingsPrefabEntity>()
                .HasMany(e => e.detector_settings_value)
                .WithRequired(e => e.detector_settings_prefab)
                .HasForeignKey(e => e.detector_settings_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DetectorSettingsValueEntity>()
                .Property(e => e.option_data_value_base64)
                .IsUnicode(false);

            modelBuilder.Entity<EnterLeavePointEntity>()
                .HasMany(e => e.enter_leave_point_event)
                .WithRequired(e => e.enter_leave_point)
                .HasForeignKey(e => e.enter_leave_point_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EnterLeavePointEventEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<ManufactoryEntity>()
                .HasMany(e => e.check_point)
                .WithRequired(e => e.manufactory1)
                .HasForeignKey(e => e.manufactory1_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ManufactoryEntity>()
                .HasMany(e => e.check_point1)
                .WithRequired(e => e.manufactory2)
                .HasForeignKey(e => e.manufactory2_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ManufactoryEntity>()
                .HasMany(e => e.manufactory_plan_point)
                .WithRequired(e => e.manufactory)
                .HasForeignKey(e => e.manufactory_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ManufactoryEntity>()
                .HasMany(e => e.pipeline_item)
                .WithRequired(e => e.manufactory)
                .HasForeignKey(e => e.manufactory_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ManufactoryEntity>()
                .HasMany(e => e.PermissionsGranted)
                .WithMany(e => e.ManufactoryPermissions)
                .Map(cs =>
                {
                    cs.MapLeftKey("manufactory_id");
                    cs.MapRightKey("role_id");
                    cs.ToTable("role_manufactory_permission");
                });

            modelBuilder.Entity<ManufactoryEntity>()
                .HasMany(e => e.storage_cell)
                .WithRequired(e => e.manufactory)
                .HasForeignKey(e => e.manufactory_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineEntity>()
                .HasMany(e => e.pipeline_item)
                .WithRequired(e => e.pipeline)
                .HasForeignKey(e => e.pipeline_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.detector)
                .WithRequired(e => e.pipeline_item)
                .HasForeignKey(e => e.pipeline_item_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.pipeline_item_interaction_event)
                .WithRequired(e => e.pipeline_item)
                .HasForeignKey(e => e.pipeline_item_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.pipeline_item_settings_value)
                .WithRequired(e => e.pipeline_item)
                .HasForeignKey(e => e.pipeline_item_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.pipeline_item_connection)
                .WithRequired(e => e.SourcePipelineItem)
                .HasForeignKey(e => e.pipeline_item1_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.pipeline_item_connection1)
                .WithRequired(e => e.TargetPipelineItem)
                .HasForeignKey(e => e.pipeline_item2_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.pipeline_item_storage_connection)
                .WithRequired(e => e.pipeline_item)
                .HasForeignKey(e => e.pipeline_item_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemEntity>()
                .HasMany(e => e.PermissionsGranted)
                .WithMany(e => e.PipelineItemPermissions)
                .Map(cs =>
                {
                    cs.MapLeftKey("pipeline_item_id");
                    cs.MapRightKey("role_id");
                    cs.ToTable("role_pipeline_item_permission");
                });

            modelBuilder.Entity<PipelineItemInteractionEventEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<PipelineItemPrefabEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<PipelineItemPrefabEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<PipelineItemPrefabEntity>()
                .Property(e => e.image_url)
                .IsUnicode(false);

            modelBuilder.Entity<PipelineItemPrefabEntity>()
                .HasMany(e => e.pipeline_item)
                .WithRequired(e => e.pipeline_item_prefab)
                .HasForeignKey(e => e.pipeline_item_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemPrefabEntity>()
                .HasMany(e => e.pipeline_item_settings_prefab)
                .WithRequired(e => e.pipeline_item_prefab)
                .HasForeignKey(e => e.pipeline_item_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemSettingsPrefabEntity>()
                .Property(e => e.option_name)
                .IsUnicode(false);

            modelBuilder.Entity<PipelineItemSettingsPrefabEntity>()
                .Property(e => e.option_description)
                .IsUnicode(false);

            modelBuilder.Entity<PipelineItemSettingsPrefabEntity>()
                .HasMany(e => e.pipeline_item_settings_value)
                .WithRequired(e => e.pipeline_item_settings_prefab)
                .HasForeignKey(e => e.pipeline_item_settings_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PipelineItemSettingsValueEntity>()
                .Property(e => e.option_data_value_base64)
                .IsUnicode(false);

            modelBuilder.Entity<ResourceEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<ResourceEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<ResourceEntity>()
                .Property(e => e.image_url)
                .IsUnicode(false);

            modelBuilder.Entity<ResourceEntity>()
                .HasMany(e => e.resource_storage_cell)
                .WithRequired(e => e.resource)
                .HasForeignKey(e => e.resource_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<StorageCellEntity>()
                .HasMany(e => e.pipeline_item_storage_connection)
                .WithRequired(e => e.storage_cell)
                .HasForeignKey(e => e.storage_cell_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StorageCellEntity>()
                .HasMany(e => e.resource_storage_cell)
                .WithRequired(e => e.storage_cell)
                .HasForeignKey(e => e.storage_cell_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StorageCellEntity>()
                .HasMany(e => e.PermissionsGranted)
                .WithMany(e => e.StorageCellPermissions)
                .Map(cs =>
                {
                    cs.MapLeftKey("storage_cell_id");
                    cs.MapRightKey("role_id");
                    cs.ToTable("role_storage_cell_permission");
                });

            modelBuilder.Entity<StorageCellEntity>()
                .HasMany(e => e.storage_cell_event)
                .WithRequired(e => e.storage_cell)
                .HasForeignKey(e => e.storage_cell_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StorageCellEventEntity>()
                .Property(e => e.log)
                .IsUnicode(false);

            modelBuilder.Entity<StorageCellPrefabEntity>()
                .Property(e => e.image_url)
                .IsUnicode(false);

            modelBuilder.Entity<StorageCellPrefabEntity>()
                .HasMany(e => e.storage_cell)
                .WithRequired(e => e.storage_cell_prefab)
                .HasForeignKey(e => e.storage_cell_prefab_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaskEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<TaskEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<UnitEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<UnitEntity>()
                .Property(e => e.short_name)
                .IsUnicode(false);

            modelBuilder.Entity<UnitEntity>()
                .HasMany(e => e.resource)
                .WithRequired(e => e.unit)
                .HasForeignKey(e => e.unit_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VisualizerTypeEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<VisualizerTypeEntity>()
                .HasMany(e => e.detector_data_prefab)
                .WithOptional(e => e.visualizer_type)
                .HasForeignKey(e => e.visualizer_type_id);
        }
    }
}
