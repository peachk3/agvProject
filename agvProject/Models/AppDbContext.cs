using agvProject.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// DB 테스트 하려고 만듦: MissionAddTest

namespace agvProject.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        // DB 저장 되는지 Test
        public virtual DbSet<MissionAddTest> MissionAddTest { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql(Common.CONNSTR, ServerVersion.AutoDetect(Common.CONNSTR));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            // Mission 테스트용 매핑
            modelBuilder.Entity<MissionAddTest>(entity =>
            {
                entity.ToTable("Mission");                // DB 테이블명  

                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();                

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.Property(e => e.Num)
                    .HasColumnName("num");
            });

        }
    }
}
