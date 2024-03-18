using ImageApplication.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace ImageUploader.Domain.Models
{
    //Classe principale pour entity framework
    public class ImageUploaderDbContext : DbContext
    {
        public ImageUploaderDbContext(DbContextOptions<ImageUploaderDbContext> options) : 
            base(options)
        {

        }
        public virtual DbSet<ImageRecord> ImageRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ImageRecord>()
               .ToContainer(nameof(ImageRecord)) //Ma table a ce nom
               .HasPartitionKey(c => c.Id)
               .HasNoDiscriminator();
        }
    }
}
