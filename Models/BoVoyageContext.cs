using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bo_Voyage_Final.Models
{
    public partial class BoVoyageContext : DbContext
    {
        public BoVoyageContext()
        {
        }

        public BoVoyageContext(DbContextOptions<BoVoyageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Destination> Destination { get; set; }
        public virtual DbSet<Dossierresa> Dossierresa { get; set; }
        public virtual DbSet<Etatdossier> Etatdossier { get; set; }
        public virtual DbSet<Personne> Personne { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Voyage> Voyage { get; set; }
        public virtual DbSet<Voyageur> Voyageur { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BoVoyage;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Client)
                    .HasForeignKey<Client>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client_Personne_Fk");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Niveau).HasComment(@"1 : Continent
2 : Pays
3 : Région");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdParenteNavigation)
                    .WithMany(p => p.InverseIdParenteNavigation)
                    .HasForeignKey(d => d.IdParente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Destination_Destination_Fk");
            });

            modelBuilder.Entity<Dossierresa>(entity =>
            {
                entity.Property(e => e.NumeroCb)
                    .HasColumnName("NumeroCB")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Dossierresa)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Dossierresa_Client_Fk");

                entity.HasOne(d => d.IdEtatDossierNavigation)
                    .WithMany(p => p.Dossierresa)
                    .HasForeignKey(d => d.IdEtatDossier)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Dossierresa_Etatdossier_Fk");

                entity.HasOne(d => d.IdVoyageNavigation)
                    .WithMany(p => p.Dossierresa)
                    .HasForeignKey(d => d.IdVoyage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Dossierresa_Voyage_Fk");
            });

            modelBuilder.Entity<Etatdossier>(entity =>
            {
                entity.Property(e => e.Libelle)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Personne>(entity =>
            {
                entity.Property(e => e.Civilite)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Datenaissance).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Prenom)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(16)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.NomFichier)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.IdDestinationNavigation)
                    .WithMany(p => p.Photo)
                    .HasForeignKey(d => d.IdDestination)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Photo_Destination_Fk");
            });

            modelBuilder.Entity<Voyage>(entity =>
            {
                entity.Property(e => e.DateDepart).HasColumnType("date");

                entity.Property(e => e.DateRetour).HasColumnType("date");

                entity.Property(e => e.Descriptif).HasMaxLength(500);

                entity.Property(e => e.PrixHt)
                    .HasColumnName("PrixHT")
                    .HasColumnType("decimal(16, 4)");

                entity.Property(e => e.Reduction).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.IdDestinationNavigation)
                    .WithMany(p => p.Voyage)
                    .HasForeignKey(d => d.IdDestination)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Voyage_Destination_Fk");
            });

            modelBuilder.Entity<Voyageur>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Idvoyage })
                    .HasName("Voyageur_Pk");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Voyageur)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Voyageur_Personne_Fk");

                entity.HasOne(d => d.IdvoyageNavigation)
                    .WithMany(p => p.Voyageur)
                    .HasForeignKey(d => d.Idvoyage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Voyageur_Voyage_Fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
