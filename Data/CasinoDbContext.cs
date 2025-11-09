using Microsoft.EntityFrameworkCore;

public class CasinoDbContext : DbContext
{
    public CasinoDbContext(DbContextOptions<CasinoDbContext> options) : base(options) { }

    public DbSet<Ruleta> Ruletas { get; set; }
    public DbSet<Apuesta> Apuestas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ruleta>().ToTable("Ruletas").HasKey(r => r.IdRuleta);
        modelBuilder.Entity<Apuesta>().ToTable("Apuestas").HasKey(a => a.IdApuesta);

       // modelBuilder.Entity<Apuesta>()
       //.HasOne(a => a.Ruleta)              
       //.WithMany(r => r.Apuestas)        
       //.HasForeignKey(a => a.IdRuleta)   
       //.HasConstraintName("FK_Apuestas_Ruletas_IdRuleta"); 
    }
}
