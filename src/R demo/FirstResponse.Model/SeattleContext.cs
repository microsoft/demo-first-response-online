namespace FirstResponse.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SeattleContext : DbContext
    {
        public SeattleContext()
            : base("name=SeattleContext")
        {
        }

        public virtual DbSet<DataSet> DataSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataSet>()
                .Property(e => e.Rain)
                .HasPrecision(5, 2);
        }
    }
}
