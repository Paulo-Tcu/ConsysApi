using ConsysApi.Data.Mappings;
using ConsysApi.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace ConsysApi.Data.Context
{
    public class ConsysContext : DbContext
    {
        private EventHandlerList? _events;
        private EventHandlerList Events => _events ?? new EventHandlerList();

        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new UsuariosMap());
        }

        protected virtual void OnDisposed(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[nameof(Disposed)];
            handler?.Invoke(this, e);
        }

        public override void Dispose()
        {
            OnDisposed(EventArgs.Empty);
        }

        public event EventHandler Disposed
        {
            add => Events.AddHandler(nameof(Disposed), value);
            remove => Events.RemoveHandler(nameof(Disposed), value);
        }
    }
}
