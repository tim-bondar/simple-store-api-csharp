using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleStore.DB
{
    [Table("storeitems")]
    public class StoreItem
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("isavailable")]
        public bool IsAvailable { get; set; }
    }
}
