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
    }
}