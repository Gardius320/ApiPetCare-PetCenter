using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCare.Domain.Identity;
using PetCare.Domain.Models;

namespace PetCare.Infrastructure.Data;

public partial class PetsDbContext : IdentityDbContext<ApplicationUser>
{
    public PetsDbContext(DbContextOptions<PetsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }
    public virtual DbSet<Owner> Owners { get; set; }
    public virtual DbSet<Pet> Pets { get; set; }
    public virtual DbSet<Species> Species { get; set; }
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Supply> Supplies { get; set; }
    public virtual DbSet<SupplyCategory> SupplyCategories { get; set; }
    public virtual DbSet<SupplyMovement> SupplyMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Esta línea es OBLIGATORIA cuando usas IdentityDbContext
        // Le dice a Identity que configure sus propias tablas primero
        base.OnModelCreating(modelBuilder);

        // Todo lo demás queda exactamente igual que antes
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__appointm__3213E83F7AACF5C9");
            entity.ToTable("appointments");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");
            entity.Property(e => e.CreateAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("create_at");
            entity.Property(e => e.Observation).HasColumnType("text").HasColumnName("observation");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.PetId).HasColumnName("pet_id");

            entity.HasOne(d => d.Owner).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__owner__33D4B598");

            entity.HasOne(d => d.State).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__state__32E0915F");

            entity.HasOne(d => d.Pet).WithMany()
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__owners__3213E83FD227B16C");
            entity.ToTable("owners");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OwnerName).HasMaxLength(25).HasColumnName("owner_name");
            entity.Property(e => e.Email).HasMaxLength(25).HasColumnName("email");
            entity.Property(e => e.PhoneNumber).HasMaxLength(25).HasColumnName("phone_number");
            entity.Property(e => e.Address).HasMaxLength(30).HasColumnName("address");
            entity.Property(e => e.Cedula).HasMaxLength(10).HasColumnName("cedula");
            entity.Property(e => e.Gender).HasColumnName("sexo");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pets__3213E83FB644E679");
            entity.ToTable("pets");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PetName).HasMaxLength(50).HasColumnName("pet_name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.SpecieId).HasColumnName("specie_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.HasOne(d => d.Owner).WithMany(p => p.Pets)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pets__owner_id__2B3F6F97");

            entity.HasOne(d => d.Specie).WithMany()
                .HasForeignKey(d => d.SpecieId);
        });

        modelBuilder.Entity<Species>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__species__3213E83FAABCB7CE");
            entity.ToTable("species");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SpecieName).HasMaxLength(25).HasColumnName("specie_name");
            entity.Property(e => e.CreateAt)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("create_at");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.IdState).HasName("PK__states__12FD6C4943565E84");
            entity.ToTable("states");
            entity.Property(e => e.IdState).HasColumnName("id_state");
            entity.Property(e => e.StateName).HasMaxLength(25).HasColumnName("state_name");
            entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
        });
        modelBuilder.Entity<SupplyCategory>(entity =>
        {
            entity.ToTable("supply_categories");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(150);
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });
        modelBuilder.Entity<Supply>(entity =>
        {
            entity.ToTable("supplies");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(150);
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.CurrentStock).HasColumnName("current_stock").HasPrecision(18, 2);
            entity.Property(e => e.MinimumStock).HasColumnName("minimum_stock").HasPrecision(18, 2);
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.SupplyCategoryId).HasColumnName("supply_category_id");
            entity.HasOne(d => d.Category).WithMany(p => p.Supplies)
                .HasForeignKey(d => d.SupplyCategoryId)
                .HasConstraintName("FK__supplies__supply___3A81B327")
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<SupplyMovement>(entity =>
        {
            entity.ToTable("supply_movements");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SupplyId).HasColumnName("supply_id");
            entity.Property(e => e.MovementType).HasColumnName("movement_type");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Reason).HasColumnName("reason").HasMaxLength(200);
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.MovementDate).HasColumnName("movement_date");

            entity.HasOne(d => d.Supply).WithMany(p => p.SupplyMovements)
                .HasForeignKey(d => d.SupplyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Appointment).WithMany()
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

    }
}