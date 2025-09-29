using SharedModels.EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.EF.AppDBContext
{
    public class AppDBContext:IdentityDbContext<clsUser>
    {
        public AppDBContext() : base()
        {
        }   
        public AppDBContext(DbContextOptions<AppDBContext> options) 
            : base(options)
        {
            var databaseCreator = Database.GetService<IRelationalDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator != null)
            {
           
                    Database.Migrate();
                if (!databaseCreator.CanConnect())
                {
                }
                if (!databaseCreator.HasTables())
                {
                    databaseCreator.CreateTables();
                }
            }
        }

        public DbSet<clsCustomer> Customers { get; set; }
        public DbSet<clsImportOrder>ImportOrders  { get; set; }
        public DbSet<clsImportOrderItem>ImportOrderItems  { get; set; }
        public DbSet<clsOrder>Orders  { get; set; }
        public DbSet<clsOrderItem>OrderItems  { get; set; }
        public DbSet<clsPerson>People  { get; set; }
        public DbSet<clsProduct>Products  { get; set; }
        public DbSet<clsSupplier>Suppliers  { get; set; }
        public DbSet<clsUser>Users  { get; set; }
        public DbSet<clsUnitOfMeasure>UnitOfMeasures  { get; set; }
        public DbSet<clsLogRegister> LogRegisters { get; set; }

        void SeedDataForUOM(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<clsUnitOfMeasure>().HasData(
                new clsUnitOfMeasure { ID = 1, Name = "قطعة", Seymbol = "قطعة" },
                new clsUnitOfMeasure { ID = 2, Name = "كيلوغرام", Seymbol = "كجم" },
                new clsUnitOfMeasure { ID = 3, Name = "جرام", Seymbol = "جم" },
                new clsUnitOfMeasure { ID = 4, Name = "لتر", Seymbol = "لتر" },
                new clsUnitOfMeasure { ID = 5, Name = "متر", Seymbol = "م" },
                new clsUnitOfMeasure { ID = 6, Name = "صندوق", Seymbol = "صندوق" },
                new clsUnitOfMeasure { ID = 7, Name = "علبة", Seymbol = "علبة" },
                new clsUnitOfMeasure { ID = 8, Name = "زجاجة", Seymbol = "زجاجة" }
            );
        }
        void SeedDataAspUser(ModelBuilder modelBuilder)
        {
            string IDGuid = Guid.NewGuid().ToString();

            clsUser user = new clsUser
            {
                Id = IDGuid,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PersonID = 1
            };
            user.PasswordHash = new PasswordHasher<clsUser>().HashPassword(user, "admin");
            modelBuilder.Entity<clsUser>().HasData(user);

            string RoleIDGuid = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = RoleIDGuid,
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    });
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = IDGuid,
                RoleId = RoleIDGuid
            });
        }
        void SeedDataPerson (ModelBuilder modelBuilder)
        {
            clsPerson person = new clsPerson();
            person.ID = 1;
            person.FirstName = "TestFirstName";
            person.LastName = "TestLastName";
            person.PhoneNumber = "12345678901";
            modelBuilder.Entity<clsPerson>().HasData(person);
        }
        //        void CreateTriggers()
        //        {
        //         string sql = @"DECLARE @TableName NVARCHAR(128);
        //DECLARE @SQL NVARCHAR(MAX);

        //-- جدول بأسماء الجداول المستهدفة
        //DECLARE @TargetTables TABLE (TableName NVARCHAR(128));
        //INSERT INTO @TargetTables (TableName)
        //VALUES 
        //    ('Orders'),
        //    ('Products'),
        //    ('ImportOrders');

        //DECLARE TableCursor CURSOR FOR
        //SELECT TableName FROM @TargetTables;

        //OPEN TableCursor;
        //FETCH NEXT FROM TableCursor INTO @TableName;

        //WHILE @@FETCH_STATUS = 0
        //BEGIN
        //    -- حذف التريجرات إن وجدت
        //    SET @SQL = '
        //    IF OBJECT_ID(''trg_' + @TableName + '_Insert'', ''TR'') IS NOT NULL
        //        DROP TRIGGER trg_' + @TableName + '_Insert;
        //    IF OBJECT_ID(''trg_' + @TableName + '_Update'', ''TR'') IS NOT NULL
        //        DROP TRIGGER trg_' + @TableName + '_Update;
        //    IF OBJECT_ID(''trg_' + @TableName + '_Delete'', ''TR'') IS NOT NULL
        //        DROP TRIGGER trg_' + @TableName + '_Delete;';
        //    EXEC sp_executesql @SQL;

        //    -- Trigger Insert
        //    SET @SQL = '
        //    CREATE TRIGGER trg_' + @TableName + '_Insert
        //    ON [' + @TableName + ']
        //    AFTER INSERT
        //    AS
        //    BEGIN
        //        SET NOCOUNT ON;
        //        INSERT INTO LogRegister (
        //            TableName,
        //            NewData,
        //            OldData,
        //            ActionDate,
        //            ActoinByUser,
        //            ActionType,
        //            Version
        //        )
        //        SELECT 
        //            ''' + @TableName + ''',
        //            (SELECT * FROM inserted FOR JSON PATH),
        //            NULL,
        //            MIN(ActionDate),
        //            MIN(ActionByUser),
        //            ''Insert'',
        //            1
        //        FROM inserted;
        //    END';
        //    EXEC(@SQL);

        //    -- Trigger Update
        //    SET @SQL = '
        //    CREATE TRIGGER trg_' + @TableName + '_Update
        //    ON [' + @TableName + ']
        //    AFTER UPDATE
        //    AS
        //    BEGIN
        //        SET NOCOUNT ON;
        //        INSERT INTO LogRegister (
        //            TableName,
        //            NewData,
        //            OldData,
        //            ActionDate,
        //            ActoinByUser,
        //            ActionType,
        //            Version
        //        )
        //        SELECT 
        //            ''' + @TableName + ''',
        //            (SELECT * FROM inserted FOR JSON PATH),
        //            (SELECT * FROM deleted FOR JSON PATH),
        //            MIN(ActionDate),
        //            MIN(ActionByUser),
        //            ''Update'',
        //            1
        //        FROM inserted;
        //    END';
        //    EXEC(@SQL);

        //    -- Trigger Delete
        //    SET @SQL = '
        //    CREATE TRIGGER trg_' + @TableName + '_Delete
        //    ON [' + @TableName + ']
        //    AFTER DELETE
        //    AS
        //    BEGIN
        //        SET NOCOUNT ON;
        //        DECLARE @UserID NVARCHAR(100) = CONVERT(NVARCHAR(100), SESSION_CONTEXT(N''UserID''));

        //        INSERT INTO LogRegister (
        //            TableName,
        //            NewData,
        //            OldData,
        //            ActionDate,
        //            ActoinByUser,
        //            ActionType,
        //            Version
        //        )
        //        SELECT 
        //            ''' + @TableName + ''',
        //            NULL,
        //            (SELECT * FROM deleted FOR JSON PATH),
        //            GETDATE(),
        //            ISNULL(@UserID, ''Unknown''),
        //            ''Delete'',
        //            1
        //        FROM deleted;
        //    END';
        //    EXEC(@SQL);

        //    FETCH NEXT FROM TableCursor INTO @TableName;
        //END;

        //CLOSE TableCursor;
        //DEALLOCATE TableCursor;
        //";
        //            Database.ExecuteSqlRaw(sql);
        //        }

        private void TrackChanges()
        {
            List<Type> TrackedTables = new List<Type>
{
    typeof(clsOrder),
    typeof(clsProduct),
    typeof(clsImportOrder)

};

            var trackedClrTypes = this.Model.GetEntityTypes()
                .Where(t => TrackedTables.Contains(t.ClrType))
                .Select(t => t.ClrType) // نرجع النوع نفسه مش اسم الجدول
                .ToList();

            List<EntityEntry> entries = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added
                          || e.State == EntityState.Modified
                          || e.State == EntityState.Deleted)
                          && trackedClrTypes.Contains(e.Entity.GetType()))
                .ToList();

            foreach (var e in entries)
            {
                clsLogRegister log = new clsLogRegister();
                log.TableName = e.Entity.GetType().Name; // اسم الجدول أو الكلاس
                log.ActionDate = DateTime.Now;
                log.ActoinByUser = e.CurrentValues["ActionByUser"].ToString();
                log.ActionType = e.State.ToString();
                log.Version = 1;
                log.NewData = e.State != EntityState.Deleted?
                   JsonSerializer.Serialize(e.CurrentValues.ToObject()) : "null";
                log.OldData = e.State != EntityState.Added ?
                     JsonSerializer.Serialize(e.OriginalValues.ToObject()) : "null";

                LogRegisters.Add(log);
            }
        }
        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


                // Seed data for UnitOfMeasure
                SeedDataForUOM(modelBuilder);
            SeedDataAspUser(modelBuilder);
            SeedDataPerson(modelBuilder);

        }

    }
}
